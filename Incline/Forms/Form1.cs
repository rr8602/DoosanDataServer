using CommonIniFile;

using Incline.Comm;
using Incline.Forms;
using Incline.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Incline
{
    public partial class Form1 : Form
    {
        public const int MOTOR = 0;  // 0번 출력: 모터 ON/OFF
        public const int BUZZER = 1;  // 1번 출력: 경보 부저
        public const int LIFT_UP = 2;  // 2번 출력: 리프트 상승
        public const int LIFT_DOWN = 3;  // 3번 출력: 리프트 하강
        public const int LAMP_GREEN = 4;  // 4번 출력: 초록 램프
        public const int LAMP_YELLOW = 5;  // 5번 출력: 노란 램프
        public const int LAMP_RED = 6;  // 6번 출력: 빨간 램프
        public const int SPARE = 7;  // 7번 출력: 예비(미사용/기타)

        SerialPort serialPort;

        private float currentValue = 0.0f;
        private IoBoard ioBoard;
        private Sensor sensor;
        private System.Windows.Forms.Timer ioTimer;
        private System.Windows.Forms.Timer sensorTimer;
        private byte currentOutputState = 0;
        private bool requestSent = false;
        private SettingDb db;

        // INI 파일 데이터
        private string ipAddress;
        private int portNumber;
        private double maxInclineAngle;// 최대 경사각도
        private double minInclineAngle; // 최소 경사각도
        private string ioPortName;
        private string sensorPortName;
        private string iniFilePath = Path.Combine(Application.StartupPath, "Config", "Incline.ini");

        // TCP 연결 관련 변수
        private TcpClient tcpClient;
        private NetworkStream clientStream;
        private bool isConnected = false;
        private Thread listenThread;
        private bool isListening = false;

        // 통신 활동 상태를 위한 변수
        private DateTime lastIoBoardRxTime = DateTime.MinValue;
        private DateTime lastSensorRxTime = DateTime.MinValue;
        private System.Windows.Forms.Timer commActivityTimer;

        private Bitmap backgroundImage;
        private bool isBackgroundInitialized = false;

        public string acceptNo { get; set; }
        public string vinNo { get; set; }
        public string model { get; set; }
        public DateTime meaDate { get; set; }
        public double inclineAngle { get; set; }
        public bool okNg { get; set; }
        public bool inspectionStatus { get; set; }

        public void MotorOn() => SetOutputPin(MOTOR, true);
        public void MotorOff() => SetOutputPin(MOTOR, false);

        public void BuzzerOn() => SetOutputPin(BUZZER, true);
        public void BuzzerOff() => SetOutputPin(BUZZER, false);

        public void LiftUpOn() => SetOutputPin(LIFT_UP, true);
        public void LiftUpOff() => SetOutputPin(LIFT_UP, false);

        public void LiftDownOn() => SetOutputPin(LIFT_DOWN, true);
        public void LiftDownOff() => SetOutputPin(LIFT_DOWN, false);

        public void LampGreenOn() => SetOutputPin(LAMP_GREEN, true);
        public void LampGreenOff() => SetOutputPin(LAMP_GREEN, false);
        public void LampYellowOn() => SetOutputPin(LAMP_YELLOW, true);
        public void LampYellowOff() => SetOutputPin(LAMP_YELLOW, false);
        public void LampRedOn() => SetOutputPin(LAMP_RED, true);
        public void LampRedOff() => SetOutputPin(LAMP_RED, false);

        public Form1()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.Size = new Size(1920, 1080);

            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, panel_arcGauge, new object[] { true });

            commActivityTimer = new System.Windows.Forms.Timer();
            commActivityTimer.Interval = 1000;
            commActivityTimer.Tick += CommActivityTimer_Tick;
            commActivityTimer.Start();

            db = new SettingDb(this);

            LoadConfigFromIni();
            db.SetupDatabaseConnection();
            InitializeIOBoard();
            InitializeSensor();
        }

        private void InitializeArcGauge()
        {
            if (backgroundImage == null || backgroundImage.Width != panel_arcGauge.Width ||
                backgroundImage.Height != panel_arcGauge.Height)
            {
                if (backgroundImage !=null)
                    backgroundImage.Dispose();

                backgroundImage = new Bitmap(panel_arcGauge.Width, panel_arcGauge.Height);
                isBackgroundInitialized = false;
            }
        }

        public void LoadConfigFromIni()
        {
            try
            {
                IniFile iniFile = new IniFile(iniFilePath);

                ipAddress = iniFile.ReadString("Network", "IP");
                portNumber = iniFile.ReadInteger("Network", "Port");
                maxInclineAngle = iniFile.ReadDouble("Angle", "MaxAngle", 90.0);
                minInclineAngle = iniFile.ReadDouble("Angle", "MinAngle", 0.0);
                ioPortName = iniFile.ReadString("Network", "IoPort");
                sensorPortName = iniFile.ReadString("Network", "SensorPort");
            }
            catch (Exception ex)
            {
                MessageBox.Show("INI 파일을 읽는 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeIOBoard()
        {
            ioBoard = new IoBoard();

            ioBoard.OnInputDataReceived += IoBoard_OnInputDataReceived;
            ioBoard.onOutputDataSent += IoBoard_OnOutputDataSent;

            ioTimer = new System.Windows.Forms.Timer();
            ioTimer.Interval = 200;
            ioTimer.Tick += IoTimer_Tick;
        }

        private void InitializeSensor()
        {
            sensor = new Sensor();

            sensor.OnRxDataReceived += Sensor_OnRxDataReceived;
            sensor.OnAngleChanged += Sensor_OnAngleChanged;

            sensorTimer = new System.Windows.Forms.Timer();
            sensorTimer.Interval = 100;
            sensorTimer.Tick += sensorTimer_Tick;
        }

        private void CommActivityTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;

            TimeSpan ioBoardElapsed = now - lastIoBoardRxTime;
            
            if (ioBoardElapsed.TotalSeconds > 3)
            {
                lbl_ioBoardComm.ForeColor = Color.Red;
            }
            else
            {
                lbl_ioBoardComm.ForeColor = Color.Green;
            }

            TimeSpan sensorElapsed = now - lastSensorRxTime;

            if (sensorElapsed.TotalSeconds > 3)
            {
                lbl_sensorComm.ForeColor = Color.Red;
            }
            else
            {
                lbl_sensorComm.ForeColor = Color.Green;
            }
        }

        private void IoTimer_Tick(object sender, EventArgs e)
        {
            if (ioBoard.IsConnected && !requestSent)
            {
                ioBoard.RequestInputStatus();
                requestSent = true;
            }
        }

        private void sensorTimer_Tick(object sender, EventArgs e)
        {
            if (sensor.IsConnected)
            {
                sensor.SendCommand();
            }
        }

        private void Sensor_OnRxDataReceived(RxData rxData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Sensor.RxDataReceivedHandler(Sensor_OnRxDataReceived), rxData);

                return;
            }

            lastSensorRxTime = DateTime.Now;

            if (float.TryParse(rxData.DataAsString, out float angle))
            {
                currentValue = angle;
                lbl_arcGaugeValue.Text = currentValue.ToString("0.0");
                lbl_incAngle.Text = currentValue.ToString("0.0");
                inclineAngle = angle;

                ControlByInclineAngle(angle);

                panel_arcGauge.Invalidate();
            }
        }

        private void IoBoard_OnInputDataReceived(byte data)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new IoBoard.DataReceivedHandler(IoBoard_OnInputDataReceived), data);

                return;
            }

            lastIoBoardRxTime = DateTime.Now;
            requestSent = false;

            bool[] inputs = new bool[8];

            for (int i = 0; i < 8; i++)
            {
                inputs[i] = (data & (1 << i)) != 0;
            }
        }

        private void IoBoard_OnOutputDataSent(byte outputData, bool success)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new IoBoard.OutputDataHandler(IoBoard_OnOutputDataSent), outputData, success);

                return;
            }
        }

        public bool ConnectIOBoard(string portName)
        {
            bool result = ioBoard.Connect(portName);

            if (result)
            {
                requestSent = false;
                ioTimer.Start();

                lbl_message.Text = "IO 보드 연결 성공";
            }
            else
            {
                MessageBox.Show("IO 보드 연결에 실패했습니다.", "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        public bool ConnectSensor(string portName)
        {
            bool result = sensor.Connect(portName);
            sensor.SendCommand();

            if (result)
            {
                sensorTimer.Start();

                lbl_message.Text = "센서 연결 성공";
            }
            else
            {
                MessageBox.Show("센서 연결에 실패했습니다.", "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        public void DisconnectIOBoard()
        {
            ioTimer.Stop();
            ioBoard.Disconnect();
        }

        public void SetOutput(byte outputValue)
        {
            if (ioBoard.IsConnected)
            {
                currentOutputState = outputValue;
                ioBoard.SetOutputStatus(outputValue);
            }
        }

        // 특정 출력 핀 제어 (0 - 7번 핀)
        public void SetOutputPin(int pinNumber, bool state)
        {
            if (pinNumber < 0 || pinNumber > 7) return;

            if (state)
                currentOutputState |= (byte)(1 << pinNumber);
            else
                currentOutputState &= (byte)~(1 << pinNumber);

            SetOutput(currentOutputState);
        }

        private void panel_arcGauge_Paint(object sender, PaintEventArgs e)
        {
            // 배경 초기화 확인
            if (!isBackgroundInitialized)
            {
                InitializeArcGauge();
                DrawBackgroundOnce();
            }

            // 배경 이미지 그리기
            e.Graphics.DrawImage(backgroundImage, 0, 0);

            // 포인터만 그리기
            int width = panel_arcGauge.Width;
            int height = panel_arcGauge.Height;
            int centerX = width - 350;
            int centerY = height - 60;
            int radius = width - 550;

            DrawPointer(e.Graphics, centerX, centerY, radius, currentValue);
        }

        private void UpdatePointer()
        {
            // 포인터가 움직이는 영역만 갱신
            int width = panel_arcGauge.Width;
            int height = panel_arcGauge.Height;
            int centerX = width - 350;
            int centerY = height - 60;
            int radius = width - 550;

            using (Graphics g = panel_arcGauge.CreateGraphics())
            {
                Rectangle pointerArea = new Rectangle(
                    centerX - radius,
                    centerY - radius,
                    radius * 2,
                    radius * 2);

                g.DrawImage(backgroundImage, pointerArea, pointerArea, GraphicsUnit.Pixel);

                DrawPointer(g, centerX, centerY, radius, currentValue);
            }
        }

        private void DrawColoredSegments(Graphics g, int centerX, int centerY, int radius)
        {
            int innerRadius = radius - 40; // 안쪽 반지름
            int outerRadius = radius - 5;  // 바깥쪽 반지름 (약간 안쪽으로)

            // 각 눈금 사이의 영역에 색상 채우기
            for (int angle = 0; angle < 90; angle += 1)
            {
                // 각도에 따른 색상 결정
                Color segmentColor;

                if (angle >= 45 && angle <= 90)
                    segmentColor = Color.Red;
                else if (angle >= 35 && angle < 45)
                    segmentColor = Color.Green;
                else
                    segmentColor = Color.Yellow;

                // 각도 사이의 세그먼트 그리기
                double startRadians = (180 - angle) * Math.PI / 180;
                double endRadians = (180 - (angle + 1)) * Math.PI / 180;

                // 세그먼트의 네 꼭지점
                PointF[] points = new PointF[4];

                // 안쪽 호의 시작점
                points[0] = new PointF(
                    centerX + (float)(Math.Cos(startRadians) * innerRadius),
                    centerY - (float)(Math.Sin(startRadians) * innerRadius));

                // 안쪽 호의 끝점
                points[1] = new PointF(
                    centerX + (float)(Math.Cos(endRadians) * innerRadius),
                    centerY - (float)(Math.Sin(endRadians) * innerRadius));

                // 바깥쪽 호의 끝점
                points[2] = new PointF(
                    centerX + (float)(Math.Cos(endRadians) * outerRadius),
                    centerY - (float)(Math.Sin(endRadians) * outerRadius));

                // 바깥쪽 호의 시작점
                points[3] = new PointF(
                    centerX + (float)(Math.Cos(startRadians) * outerRadius),
                    centerY - (float)(Math.Sin(startRadians) * outerRadius));

                // 세그먼트 채우기
                g.FillPolygon(new SolidBrush(segmentColor), points);
            }

            // 눈금선 그리기 (색상 위에 겹치도록)
            for (int angle = 0; angle <= 90; angle += 10)
            {
                double radians = (180 - angle) * Math.PI / 180;
                float startX = centerX + (float)(Math.Cos(radians) * innerRadius);
                float startY = centerY - (float)(Math.Sin(radians) * innerRadius);
                float endX = centerX + (float)(Math.Cos(radians) * radius);
                float endY = centerY - (float)(Math.Sin(radians) * radius);

                Pen tickPen = (angle % 10 == 0) ? new Pen(Color.Black, 2) : new Pen(Color.Gray, 1);
                g.DrawLine(tickPen, startX, startY, endX, endY);
            }
        }

        private void Sensor_OnAngleChanged(double gatAngle)
        {
            if (this.InvokeRequired)
            {

                this.Invoke(new Sensor.AngleChangedHandler(Sensor_OnAngleChanged), gatAngle);
                return;
            }

            Debug.WriteLine($"각도 변경 감지: {gatAngle}°");

            // UI 업데이트
            currentValue = (float)gatAngle;
            lbl_arcGaugeValue.Text = currentValue.ToString("0.0");
            lbl_incAngle.Text = currentValue.ToString("0.0");
            inclineAngle = gatAngle;

            // OK/NG 판정
            ControlByInclineAngle(gatAngle);

            // 포인터만 갱신
            UpdatePointer();
        }

        private void DrawBackgroundOnce()
        {
            if (isBackgroundInitialized)
                return;

            using (Graphics g = Graphics.FromImage(backgroundImage))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(panel_arcGauge.BackColor);

                // 배경 설정
                int width = panel_arcGauge.Width;
                int height = panel_arcGauge.Height;
                int centerX = width - 350;
                int centerY = height - 60;
                int radius = width - 550;

                // 기본 아크 배경
                g.FillPie(Brushes.Ivory, centerX - radius, centerY - radius, radius * 2, radius * 2, 90, 90);

                // 외곽선
                g.DrawArc(new Pen(Color.Black, 2), centerX - radius, centerY - radius, radius * 2, radius * 2, 90, 90);

                // 색상 세그먼트 그리기
                DrawColoredSegments(g, centerX, centerY, radius);

                // 눈금 숫자
                for (int angle = 0; angle <= 90; angle += 10)
                {
                    double radians = (180 - angle) * Math.PI / 180;
                    float labelRadius = radius - 40;
                    float x = centerX + (float)(Math.Cos(radians) * labelRadius);
                    float y = centerY - (float)(Math.Sin(radians) * labelRadius);

                    StringFormat format = new StringFormat();
                    if (angle == 0)
                    {
                        format.Alignment = StringAlignment.Near;
                        x -= 5;
                    }
                    else if (angle == 90)
                    {
                        format.Alignment = StringAlignment.Far;
                        x += 5;
                    }
                    else
                    {
                        format.Alignment = StringAlignment.Center;
                    }

                    g.DrawString(angle.ToString(), new Font("Arial", 25, FontStyle.Bold), Brushes.Black, x, y, format);
                }

                // 수평선 그리기
                float horizontalLineY = centerY;
                g.FillRectangle(Brushes.Black, centerX - radius, horizontalLineY - 2, radius, 4);
            }

            isBackgroundInitialized = true;
        }


        private void DrawPointer(Graphics g, int centerX, int centerY, int radius, float value)
        {
            double radians = (180 - currentValue) * Math.PI / 180;

            float pointerEndX = centerX + (float)(Math.Cos(radians) * (radius - 40));
            float pointerEndY = centerY - (float)(Math.Sin(radians) * (radius - 40));

            g.DrawLine(new Pen(Color.Blue, 3), centerX, centerY, pointerEndX, pointerEndY);
            g.FillEllipse(Brushes.Black, centerX - 5, centerY - 5, 10, 10);
        }

        // 경사각도에 따른 동작 제어 (센서가 있을 시, IoTimer_Tick에 구현)
        public void ControlByInclineAngle(double angle)
        {
            if (angle >= 35 && angle <= 45)
            {
                lbl_message.Text = "정상 동작";

                lbl_okNg.BackColor = Color.Green;
                lbl_okNg.ForeColor = Color.White;
            }
            else if (angle < 35)
            {
                lbl_message.Text = "각도 낮음";

                lbl_okNg.BackColor = Color.Yellow;
                lbl_okNg.ForeColor = Color.White;
            }
            else
            {
                lbl_message.Text = "경고: 각도 초과!";

                lbl_okNg.BackColor = Color.Red;
                lbl_okNg.ForeColor = Color.White;
            }
        }

        private void SetupTcpConnection()
        {
            try
            {
                tcpClient = new TcpClient();
                tcpClient.Connect(ipAddress, portNumber);
                clientStream = tcpClient.GetStream();
                isConnected = true;

                // 연결 후 서버 응답 수신을 위한 스레드 시작
                StartListening();

                Console.WriteLine("서버에 연결되었습니다.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"서버 연결 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StartListening()
        {
            if (isListening)
                return;

            isListening = true;
            listenThread = new Thread(new ThreadStart(ListenForData));
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ListenForData()
        {
            byte[] buffer = new byte[64];

            try
            {
                while (isListening && isConnected && tcpClient != null && tcpClient.Connected)
                {
                    // 데이터를 수신할 수 있는지 확인
                    if (clientStream.DataAvailable)
                    {
                        int bytesRead = clientStream.Read(buffer, 0, buffer.Length);

                        if (bytesRead > 0)
                        {
                            string receivedData = Encoding.GetEncoding("EUC-KR").GetString(buffer, 0, bytesRead);
                            Console.WriteLine($"[{DateTime.Now}] 데이터 수신: {receivedData}");
                            ProcessReceivedData(receivedData);
                        }
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{DateTime.Now}] ListenForData 예외 발생: {ex.Message}");

                try
                {
                    // 핸들이 생성되었는지 확인 후 Invoke 호출
                    if (this.IsHandleCreated)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            Console.WriteLine($"서버와의 연결이 끊어졌습니다: {ex.Message}");
                        });
                    }
                    else
                    {
                        Console.WriteLine("폼 핸들이 생성되지 않아 UI 업데이트를 건너뜁니다.");
                    }
                }
                catch (Exception invokeEx)
                {
                    Console.WriteLine($"UI 업데이트 중 오류: {invokeEx.Message}");
                }

                isConnected = false;
            }
            finally
            {
                Console.WriteLine($"[{DateTime.Now}] ListenForData 메서드 종료됨");
            }
        }

        private void ProcessReceivedData(string data)
        {
            try
            {
                if (this.IsHandleCreated)
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        Console.WriteLine($"수신된 데이터: {data}");

                        if (data.Contains("WHO"))
                        {
                            SendResponse("<CON/ANG/OK/3>");
                        }
                        else if (data.Contains("REG"))
                        {
                            string[] result = data.Split('/');

                            acceptNo = result[2];
                            vinNo = result[3];
                            model = result[4];

                            // 서버에서 Rx 값을 받아도 바로 검사 진행하지 않고, 먼저 DB에 저장 후 나중에 사용자가 선택해서 검사할 수 있도록 함
                            db.SaveMeasurementDataToMDB(
                                acceptNo: acceptNo,
                                vinNo: vinNo,
                                model: model);

                            // lbl_currentVehicle.Text = vinNo;

                            Console.WriteLine($"REG 수신된 데이터: {data}");
                        }
                    });
                }
                else
                {
                    Console.WriteLine($"[{DateTime.Now}] 데이터 수신됨 (UI 업데이트 불가): {data}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ProcessReceivedData 예외: {ex.Message}");
            }
        }

        private void SendResponse(string response)
        {
            try
            {
                if (tcpClient != null && tcpClient.Connected)
                {
                    byte[] data = Encoding.GetEncoding("EUC-KR").GetBytes(response);
                    clientStream.Write(data, 0, data.Length);
                    clientStream.Flush();

                    Console.WriteLine($"응답 전송 성공: {response}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"응답 전송 실패: {ex.Message}");
            }
        }

        public void SendInclineDataToServer()
        {
            string incData = string.Format(CultureInfo.InvariantCulture, "{0}/{1:F2}", acceptNo, inclineAngle);

            string packet = $"<RST/ANG/{incData}>";
            int dataCount = packet.Split('/').Length + 1;

            string packetResult = $"<RST/ANG/{incData}/{dataCount}>";

            try
            {
                // 기존 연결을 사용하여 데이터 전송
                if (isConnected && tcpClient != null && tcpClient.Connected)
                {
                    byte[] bytes = Encoding.GetEncoding("EUC-KR").GetBytes(packetResult);
                    clientStream.Write(bytes, 0, bytes.Length);
                    clientStream.Flush();

                    Console.WriteLine("데이터 전송 성공 : " + packetResult);
                }
                else
                {
                    // 연결이 없는 경우 새로 연결 시도
                    SetupTcpConnection();

                    if (isConnected)
                    {
                        byte[] bytes = Encoding.GetEncoding("EUC-KR").GetBytes(packetResult);
                        clientStream.Write(bytes, 0, bytes.Length);
                        clientStream.Flush();

                        Console.WriteLine("데이터 전송 성공 : " + packetResult);
                    }
                    else
                    {
                        MessageBox.Show("서버에 연결되어 있지 않습니다.", "전송 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버 전송 실패: " + ex.Message, "전송 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateVinNo(string vinNo)
        {
            // 차대번호 유효성 검사
            if (string.IsNullOrEmpty(vinNo) || vinNo.Length < 6)
            {
                return false;
            }

            return true;
        }

        private void SetMultipleOutputPins(int[] pinNumbers, bool state)
        {
            if (pinNumbers == null|| pinNumbers.Length == 0) return;

            foreach (int pin in pinNumbers)
            {
                if (pin >= 0 && pin <= 7)
                {
                    if (state)
                        currentOutputState |= (byte)(1 << pin);
                    else
                        currentOutputState &= (byte)~(1 << pin);
                }
            }

            SetOutput(currentOutputState);
        }

        // 0,1,3 핀 동시 출력
        public void LiftUpOnSignal(bool state)
        {
            if (state)
            {
                int[] pins = { MOTOR, BUZZER, LIFT_DOWN };
                SetMultipleOutputPins(pins, state);
            }
        }

        // 0,2,3 핀 동시 출력
        public void LiftDownOnSignal(bool state)
        {
            if (state)
            {
                int[] pins = { MOTOR, LIFT_UP, LIFT_DOWN };
                SetMultipleOutputPins(pins, state);
            }
        }

        public void LiftOffSignal()
        {
            currentOutputState = 0;
            SetOutput(currentOutputState);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lbl_message.Text = "초기화중";
            lbl_arcGaugeValue.Text = "0.0";
            lbl_incAngle.Text = "0.0";

            lbl_ioBoardComm.ForeColor = Color.Green;
            lbl_sensorComm.ForeColor = Color.Green;

            currentValue = 0.0f;

            InitializeArcGauge();
            panel_arcGauge.Invalidate();

            ConnectIOBoard("COM11");
            ConnectSensor("COM12");
            LiftOffSignal();
        }

        private void btn_liftUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (ioBoard != null && ioBoard.IsConnected)
            {
                LiftUpOnSignal(true);
            }
            else
            {
                MessageBox.Show("IO 보드가 연결되어 있지 않습니다.\n설정 메뉴에서 IO 보드를 먼저 연결해주세요.",
                               "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_liftUp_MouseUp(object sender, MouseEventArgs e)
        {
            LiftOffSignal();
        }

        private void btn_liftDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (ioBoard != null && ioBoard.IsConnected)
            {
                LiftUpOnSignal(true);
            }
            else
            {
                MessageBox.Show("IO 보드가 연결되어 있지 않습니다.\n설정 메뉴에서 IO 보드를 먼저 연결해주세요.",
                               "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_liftDown_MouseUp(object sender, MouseEventArgs e)
        {
            LiftOffSignal();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormClosing -= Form1_FormClosing;

            if (ioBoard != null && ioBoard.IsConnected)
            {
                try
                {
                    ioTimer?.Stop();
                    ioBoard?.Disconnect();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"폼 종료 중 오류: {ex.Message}");
                }
            }

            try
            {
                isListening = false;

                if (listenThread != null && listenThread.IsAlive)
                {
                    listenThread.Join(1000);
                }

                if (clientStream != null)
                {
                    clientStream.Close();
                }

                if (tcpClient != null)
                {
                    tcpClient.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"TCP 연결 종료 중 오류: {ex.Message}");
            }

            commActivityTimer?.Stop();
            commActivityTimer?.Dispose();
            ioTimer?.Dispose();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_config_Click(object sender, EventArgs e)
        {
            using (ConfigForm configForm = new ConfigForm(this))
            {
                configForm.ShowDialog();
            }
        }

        private void btn_selectVehicle_Click(object sender, EventArgs e)
        {
            using (ListForm listform = new ListForm(this, db))
            {
                if (listform.ShowDialog() == DialogResult.OK)
                {
                    lbl_currentVehicle.Text = listform.SelectedVinNo;

                    db.SetListForm(listform);
                }
            }
        }
        //"#READ"
        private void btn_inspectionStart_Click(object sender, EventArgs e)
        {
            vinNo = lbl_currentVehicle.Text;

            if (ioBoard.IsConnected == false)
            {
                MessageBox.Show("IO 보드에 연결되어 있지 않습니다. 먼저 연결해주세요.", "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(vinNo))
                {
                    if (ValidateVinNo(vinNo))
                    {
                        lbl_currentVehicle.Text = vinNo;

                        SendInclineDataToServer();
                    }
                    else
                    {
                        MessageBox.Show("유효하지 않은 바코드입니다. 다시 시도해주세요.", "바코드 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("차량 바코드가 입력되지 않았습니다. 차량을 선택하거나 바코드를 입력해주세요.", "바코드 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool confirmOkNg()
        {
            double angle;
            string text = lbl_incAngle.Text.Replace("°", "");

            if (double.TryParse(text, out angle))
            {
                if (angle >= 35 && angle <= 45)
                {
                    lbl_message.Text = "검사 합격";

                    return true;
                }
            }

            return false;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (acceptNo != "" && vinNo != "")
                {
                    okNg = confirmOkNg();
                    inspectionStatus = true;
                    meaDate = DateTime.Now;

                    db.SaveMeasurementDataToMDB(acceptNo, vinNo, model, okNg, inspectionStatus, meaDate);
                }
                else
                {
                    MessageBox.Show("접수번호나 차대번호가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 저장 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btn_io_Click(object sender, EventArgs e)
        {
            if (ioBoard != null && ioBoard.IsConnected)
            {
                IoBoardForm ioBoardForm = new IoBoardForm(this);
                ioBoardForm.Show();
            }
            else
            {
                MessageBox.Show("IO 보드가 연결되어 있지 않습니다.\n설정 메뉴에서 IO 보드를 먼저 연결해주세요.",
                       "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panel_arcGauge_Resize(object sender, EventArgs e)
        {
            isBackgroundInitialized = false;
            panel_arcGauge.Invalidate();
        }

        private void btn_allPause_Click(object sender, EventArgs e)
        {
            currentOutputState = 0;
            ioBoard.SetOutputStatus(0);

            ioBoard.Disconnect();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            LiftOffSignal();
        }
    }
}
