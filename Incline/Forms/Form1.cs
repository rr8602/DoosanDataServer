using CommonIniFile;

using Incline.Comm;
using Incline.Forms;
using Incline.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
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

        private TcpServer tcpServer;
        private string currentChassisNumber;
        private string currentModel;
        private string currentManufacturer;

        private float currentValue = 0.0f;
        private IoBoard ioBoard;
        private System.Windows.Forms.Timer ioTimer;
        private byte currentOutputState = 0;
        private int currentSequence = 1;
        private bool requestSent = false;
        private SettingDb db;

        // INI 파일 데이터
        private string ipAddress;
        private int portNumber;
        private double maxInclineAngle;// 최대 경사각도
        private double minInclineAngle; // 최소 경사각도
        private string iniFilePath = Path.Combine(Application.StartupPath, "Config", "Incline.ini");

        // TCP 연결 관련 변수
        private TcpClient tcpClient;
        private NetworkStream clientStream;
        private bool isConnected = false;
        private Thread listenThread;
        private bool isListening = false;

        public string acceptNo { get; private set; }
        public string vinNo { get; private set; }
        public string model { get; private set; }
        public DateTime meaDate { get; private set; }
        public double inclineAngle { get; private set; }

        public bool okNg { get; private set; }

        public bool inspectionStatus { get; private set; }

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

            db = new SettingDb(this);

            LoadConfigFromIni();
            db.SetupDatabaseConnection();
            InitializeIOBoard();
        }

        public void LoadConfigFromIni()
        {
            try
            {
                IniFile iniFile = new IniFile(iniFilePath);

                ipAddress = iniFile.ReadString("Network", "IP");
                portNumber = iniFile.ReadInteger("Network", "Port");
                maxInclineAngle = iniFile.ReadDouble("Network", "MaxAngle", 90.0);
                minInclineAngle = iniFile.ReadDouble("Network", "MinAngle", 0.0);
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
            ioBoard.OnRxDataReceived += IoBoard_OnRxDataReceived;

            ioTimer = new System.Windows.Forms.Timer();
            ioTimer.Interval = 200;
            ioTimer.Tick += IoTimer_Tick;
        }

        private void IoTimer_Tick(object sender, EventArgs e)
        {
            if (ioBoard.IsConnected && !requestSent)
            {
                ioBoard.RequestInputStatus();
                requestSent = true;
            }
        }

        private void IoBoard_OnRxDataReceived(RxData rxData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new IoBoard.RxDataReceivedHandler(IoBoard_OnRxDataReceived), rxData);

                return;
            }

            if (float.TryParse(rxData.DataAsString, out float angle))
            {
                currentValue = angle;
                lbl_arcGaugeValue.Text = currentValue.ToString("0.0");
                lbl_incAngle.Text = currentValue.ToString("0.0");
                inclineAngle = angle;

                if (angle > maxInclineAngle)
                {
                    lbl_okNg.Text = "NG";
                    lbl_okNg.BackColor = Color.Red;
                    lbl_okNg.ForeColor = Color.White;
                }
                else
                {
                    lbl_okNg.Text = "OK";
                    lbl_okNg.BackColor = Color.Green;
                    lbl_okNg.ForeColor = Color.White;
                }

                panel_arcGauge.Invalidate();
            }

            ControlByInclineAngle(angle);
        }

        private void IoBoard_OnInputDataReceived(byte data)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new IoBoard.DataReceivedHandler(IoBoard_OnInputDataReceived), data);

                return;
            }

            bool[] inputs = new bool[8];

            for (int i = 0; i < 8; i++)
            {
                inputs[i] = (data & (1 << i)) != 0;
            }

            UpdateUIBasedOnInputs(inputs);

            int inputValue = data & 0x7F; // 하위 7비트만 사용

            if (Math.Abs(currentValue - ((inputValue / 127.0f) * 90.0f)) > 0.1f)
            {
                currentValue = (inputValue / 127.0f) * 90.0f; // 0-127 값을 0-90도로 변환
                lbl_arcGaugeValue.Text = currentValue.ToString("0.0");
                lbl_incAngle.Text = currentValue.ToString("0.0");
                panel_arcGauge.Invalidate();
            }
        }

        private void IoBoard_OnOutputDataSent(byte outputData, bool success)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new IoBoard.OutputDataHandler(IoBoard_OnOutputDataSent), outputData, success);

                return;
            }

            if (success)
                lbl_message.Text = "출력 성공";
            else
                lbl_message.Text = "출력 실패";
        }

        private void UpdateUIBasedOnInputs(bool[] inputs)
        {
            if (inputs[0])
            {
                lbl_message.Text = "입력 0 활성화";
            }

            // *** 다음 입력에 대한 처리 추가 필요
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
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // 배경
            int width = panel_arcGauge.Width;
            int height = panel_arcGauge.Height;
            int centerX = width - 350;
            int centerY = height - 60;
            int radius = width - 550;

            // 기본 아크 배경 (아이보리)
            g.FillPie(Brushes.Ivory, centerX - radius, centerY - radius, radius * 2, radius * 2, 90, 90);

            // 외곽선
            g.DrawArc(new Pen(Color.Black, 2), centerX - radius, centerY - radius, radius * 2, radius * 2, 90, 90);

            // 색상 세그먼트 그리기
            DrawColoredSegments(g, centerX, centerY, radius);

            // 눈금 숫자
            for (int angle = 0; angle <= 90; angle += 10)
            {
                double radians = (180 - angle) * Math.PI / 180;

                float labelRadius = radius - 40; // 숫자를 표시할 반지름 (눈금보다 안쪽에 위치)
                float x= centerX + (float)(Math.Cos(radians) * labelRadius);
                float y = centerY - (float)(Math.Sin(radians) * labelRadius);

                StringFormat format = new StringFormat();

                if (angle == 0)
                {
                    format.Alignment = StringAlignment.Near;
                    x -= 5;
                }
                else  if (angle == 90)
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

            // 수평선 그리기 (파란색 영역)
            float horizontalLineY = centerY;
            g.FillRectangle(Brushes.Black, centerX - radius, horizontalLineY - 2, radius, 4);

            // 현재 값에 해당하는 포인터
            DrawPointer(g, centerX, centerY, radius, currentValue);
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

        private void DrawPointer(Graphics g, int centerX, int centerY, int radius, float value)
        {
            double radians = (180 - currentValue) * Math.PI / 180;

            float pointerEndX = centerX + (float)(Math.Cos(radians) * (radius - 40));
            float pointerEndY = centerY - (float)(Math.Sin(radians) * (radius - 40));

            g.DrawLine(new Pen(Color.Blue, 3), centerX, centerY, pointerEndX, pointerEndY);
            g.FillEllipse(Brushes.Black, centerX - 5, centerY - 5, 10, 10);
        }

        // 센서가 없을 때, 사용하는 경사각도 값 계산 메서드
        private double GetCurrentInclination()
        {
            Random rand = new Random();
            double angle = Math.Round(rand.NextDouble() * 90, 2);

            this.inclineAngle = angle;
            this.lbl_incAngle.Text = angle.ToString("0.00") + "°";
            this.currentValue = (float)angle;
            this.lbl_arcGaugeValue.Text = currentValue.ToString("0.0");
            this.inspectionStatus = true;

            panel_arcGauge.Invalidate();

            return angle;
        }

        // 경사각도에 따른 동작 제어 (센서가 있을 시, IoTimer_Tick에 구현)
        public void ControlByInclineAngle(double angle)
        {
            if (angle >= maxInclineAngle)
            {
                MotorOff();
                BuzzerOn();
                LampRedOn();
                LampGreenOff();
                lbl_message.Text = "경고: 각도 초과!";

                lbl_okNg.Text = "NG";
                lbl_okNg.BackColor = Color.Red;
                lbl_okNg.ForeColor = Color.White;
            }
            else
            {
                MotorOn();
                BuzzerOff();
                LampRedOff();
                LampGreenOn();
                lbl_message.Text = "정상 동작";

                lbl_okNg.Text = "OK";
                lbl_okNg.BackColor = Color.Green;
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

                        if (data.Contains("<WHO/1>"))
                        {
                            SendResponse("<CON/ANG/OK/3>");
                        }
                        else if (data.Contains("REG"))
                        {
                            string[] result = data.Split('/');

                            acceptNo = result[2];
                            vinNo = result[3];
                            model = result[4];

                            lbl_currentVehicle.Text = vinNo;

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

        private bool ValidateBarcode(string barcode)
        {
            // 바코드 유효성 검사
            if (string.IsNullOrEmpty(barcode) || barcode.Length < 5)
            {
                return false;
            }

            // *** 추가적인 바코드 형식 검사 로직 필요 (DB에 있는 유효한 바코드인지 등)
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

            currentValue = 0.0f;

            panel_arcGauge.Invalidate();

            SetupTcpConnection();
        }

        private void btn_liftUp_MouseDown(object sender, MouseEventArgs e)
        {
            LiftUpOnSignal(true);
        }

        private void btn_liftUp_MouseUp(object sender, MouseEventArgs e)
        {
            LiftOffSignal();
        }

        private void btn_liftDown_MouseDown(object sender, MouseEventArgs e)
        {
            LiftDownOnSignal(true);
        }

        private void btn_liftDown_MouseUp(object sender, MouseEventArgs e)
        {
            LiftOffSignal();
        }

        // 기존 FormClosing 이벤트 핸들러를 수정
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.FormClosing -= Form1_FormClosing;

            if (ioBoard != null && ioBoard.IsConnected)
            {
                try
                {
                    ioTimer?.Stop();
                    ioBoard?.Disconnect();
                    tcpServer.Stop();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"폼 종료 중 오류: {ex.Message}");
                }
            }

            try
            {
                isListening = false;

                if (listenThread != null && listenThread.IsAlive)
                {
                    listenThread.Join(1000); // 최대 1초 대기
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
                System.Diagnostics.Debug.WriteLine($"TCP 연결 종료 중 오류: {ex.Message}");
            }

            ioTimer?.Dispose();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            DisconnectIOBoard();
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

        private void btn_inspectionStart_Click(object sender, EventArgs e)
        {
            double angle;

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
                    if (ValidateBarcode(vinNo))
                    {
                        lbl_currentVehicle.Text = vinNo;

                        //ioBoard.SendCommand();
                        lbl_message.Text = "센서 데이터 요청 중...";

                        inspectionStatus = true;

                        // 센서 연결 시, 실제 검사 결과 처리는 IoBoard_OnRxDataReceived 에서 처리됨

                        angle = GetCurrentInclination();
                        ControlByInclineAngle(angle);
                        SendInclineDataToServer();
                        meaDate = DateTime.Now;
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

        /*private void TcpServer_VehicleInfoReceived(object sender, VehicleInfoEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { TcpServer_VehicleInfoReceived(sender, e); });
                return;
            }

            try
            {
                currentChassisNumber = e.ChassisNumber;
                currentModel = e.Model;
                currentManufacturer = e.Manufacturer;

                btn_inspectionStart.Enabled = !string.IsNullOrEmpty(currentChassisNumber);

                lbl_currentVehicle.Text = currentChassisNumber;

                UpdateStatusMessage("차량 정보 수신", $"차량 정보: {currentChassisNumber}, {currentModel}, {currentManufacturer}");

                MessageBox.Show($"차량 정보가 수신되었습니다.\n차대번호: {currentChassisNumber}\n모델: {currentModel}\n제조사: {currentManufacturer}",
                              "차량 정보 수신", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"차량 정보 수신 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TcpServer_PacketReceived(object sender, PacketReceivedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { TcpServer_PacketReceived(sender, e); });
                return;
            }

            try
            {
                Console.WriteLine($"패킷 수신됨: 명령어={e.Command}, 소스={e.Source}, 데이터={e.Data}, 데이터 카운트={e.DataCount}");

                switch (e.Command)
                {
                    case Packet.CMD_WHO:
                        UpdateStatusMessage("", "설비 인증 요청");
                        break;

                    case Packet.CMD_REG:
                        UpdateStatusMessage("", "차량 정보 등록");
                        break;

                    default:
                        UpdateStatusMessage("", "알 수 없는 명령");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"패킷 처리 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }*/

        private void TcpServer_ServerLog(object sender, string e)
        {
            Console.WriteLine(e);
        }

        /*private void UpdateStatusMessage(string title, string message)
        {
            if (lbl_title != null && lbl_message != null)
            {
                lbl_title.Text = title;
                lbl_message.Text = message;
            }

            Console.WriteLine($"[{DateTime.Now}] State : {title} - {message}");
        }*/

        private bool confirmOkNg()
        {
            double angle;
            string text = lbl_incAngle.Text.Replace("°", "");

            if (double.TryParse(text, out angle))
            {
                return angle <= maxInclineAngle;
            }

            return false;
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                okNg = confirmOkNg(); 

                if (acceptNo != "" && vinNo != "")
                {
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
            IoBoardForm ioBoardForm = new IoBoardForm();

            ioBoardForm.Show();
        }
    }
}
