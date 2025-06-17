using CommonIniFile;

using Incline.Forms;

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
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
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

        private float currentValue = 0.0f;
        private IOBoard ioBoard;
        private Timer ioTimer;
        private byte currentOutputState = 0;
        private int currentSequence = 1;
        private bool requestSent = false;
        private SettingDb db;


        // INI파일 데이터
        private string ipAddress;
        private string portNumber;
        private string iniFilePath = Path.Combine(Application.StartupPath, "Config", "Incline.ini");

        public string vehicleBarcode { get; private set; }
        public string formattedBarcode { get; private set; }
        public double inclineAngle { get; private set; }

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
                portNumber = iniFile.ReadString("Network", "Port");
            }
            catch (Exception ex)
            {
                MessageBox.Show("INI 파일을 읽는 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeIOBoard()
        {
            ioBoard = new IOBoard();

            ioBoard.OnInputDataReceived += IoBoard_OnInputDataReceived;
            ioBoard.onOutputDataSent += IoBoard_OnOutputDataSent;

            ioTimer = new Timer();
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

        private void IoBoard_OnInputDataReceived(byte data)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new IOBoard.DataReceivedHandler(IoBoard_OnInputDataReceived), data);

                return;
            }

            bool[] inputs = new bool[8];

            for (int i = 0; i < 8; i++)
            {
                inputs[i] = (data & (1 << i)) != 0;
            }

            UpdateUIBasedOnInputs(inputs);

            int inputValue = data & 0x7F; // 하위 7비트만 사용
            currentValue = (inputValue / 127.0f) * 90.0f; // 0-127 값을 0-90도로 변환

            lbl_arcGaugeValue.Text = currentValue.ToString("0.0");
            lbl_incAngle.Text = inputValue.ToString("0.0");
            panel_arcGauge.Invalidate();
        }

        private void IoBoard_OnOutputDataSent(byte outputData, bool success)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new IOBoard.OutputDataHandler(IoBoard_OnOutputDataSent), outputData, success);

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
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 배경
            int width = panel_arcGauge.Width;
            int height = panel_arcGauge.Height;
            int centerX = width / 3;
            int centerY = height - 50;
            int radius = (int)(Math.Min(width, height) * 2);

            if (centerX + radius > width)
            {
                radius = width - centerX - 20;
            }
            if (centerY - radius < 0)
            {
                radius = centerY - 20;
            }
            if (centerX - radius < 0)
            {
                centerX = radius + 20;
            }

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
            float horizontalLineY = centerY + 2;
            g.FillRectangle(Brushes.Blue, centerX - radius, horizontalLineY - 5, radius, 10);

            // 현재 값에 해당하는 포인터
            DrawPointer(g, centerX, centerY, radius, 0);
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

            g.DrawLine(new Pen(Color.DarkBlue, 3), centerX, centerY, pointerEndX, pointerEndY);

            float horizontalLineEndX = centerX;
            float horizontalLineY = centerY + 2;

            g.FillEllipse(Brushes.Blue, horizontalLineEndX - 10, horizontalLineY - 10, 20, 20);
        }

        private double GetCurrentInclination()
        {
            Random rand = new Random();
            double angle = Math.Round(rand.NextDouble() * 90, 2);

            this.inclineAngle = angle;
            this.lbl_incAngle.Text = angle.ToString("0.00") + "°";

            return angle;
        }

        // 경사각도에 따른 동작 제어 (센서가 있을 시, IoTimer_Tick에 구현)
        public void ControlByInclineAngle(double angle)
        {
            if (angle >= 40.0)
            {
                MotorOff();
                BuzzerOn();
                LampRedOn();
                LampGreenOff();
                lbl_message.Text = "경고: 각도 초과! 모터 OFF, 경보 ON";
            }
            else
            {
                MotorOn();
                BuzzerOff();
                LampRedOff();
                LampGreenOn();
                lbl_message.Text = "정상 동작";
            }
        }

        public string SendWeightDataToServer()
        {
            string datePrefix = DateTime.Now.ToString("yyyyMMdd");
            string sequentialNumber = currentSequence.ToString("D4"); // 4자리 순차번호
            currentSequence++;
            formattedBarcode = datePrefix + sequentialNumber;

            string incData = string.Format(CultureInfo.InvariantCulture, "{0}/{1:F2}", formattedBarcode, inclineAngle);

            int dataCount = 2;
            string packet = $"<RST/ANG/{incData}/{dataCount}>";

            try
            {
                using (TcpClient client = new TcpClient())
                {
                    client.Connect(ipAddress, int.Parse(portNumber));

                    using (NetworkStream stream = client.GetStream())
                    {
                        byte[] bytes = Encoding.GetEncoding("EUC-KR").GetBytes(packet);
                        stream.Write(bytes, 0, bytes.Length);
                        stream.Flush();
                    }
                }

                Console.WriteLine("데이터 전송 성공 : " + packet);

                return formattedBarcode;
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버 전송 실패: " + ex.Message, "전송 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return "";
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

        private void Form1_Load(object sender, EventArgs e)
        {
            lbl_message.Text = "초기화중";
            lbl_arcGaugeValue.Text = "0.0";
            lbl_incAngle.Text = "0.0";

            panel_arcGauge.Invalidate();

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            cmb_ports.Items.Clear();
            cmb_ports.Items.AddRange(ports);
            if (ports.Length > 0)
                cmb_ports.SelectedIndex = 0;
        }

        private void btn_motorOn_Click(object sender, EventArgs e)
        {
            MotorOn();
        }

        private void btn_motorOff_Click(object sender, EventArgs e)
        {
            MotorOff();
        }

        private void btn_refreshPorts_Click(object sender, EventArgs e)
        {
            cmb_ports.Items.Clear();
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            cmb_ports.Items.AddRange(ports);

            if (ports.Length > 0)
                cmb_ports.SelectedIndex = 0;
            else
                MessageBox.Show("사용 가능한 COM 포트가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"폼 종료 중 오류: {ex.Message}");
                }
            }

            ioTimer?.Dispose();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            DisconnectIOBoard();
            this.Close();
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            string portName = cmb_ports.SelectedItem as string;

            ConnectIOBoard(portName);
        }

        private void btn_config_Click(object sender, EventArgs e)
        {
            using (ConfigForm configForm = new ConfigForm())
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
                    lbl_currentVehicle.Text = listform.SelectedAccpetNo;

                    db.SetListForm(listform);
                }
            }
        }

        private void btn_inspectionStart_Click(object sender, EventArgs e)
        {
            string acceptNo = string.Empty;
            double angle;
            vehicleBarcode = txt_acceptNo.Text.Trim();

            if (ioBoard.isConnected == false)
            {
                MessageBox.Show("IO 보드에 연결되어 있지 않습니다. 먼저 연결해주세요.", "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(vehicleBarcode))
                {
                    if (ValidateBarcode(vehicleBarcode))
                    {
                        lbl_currentVehicle.Text = txt_acceptNo.Text;

                        angle = GetCurrentInclination();
                        ControlByInclineAngle(angle);

                        acceptNo = SendWeightDataToServer();
                        db.SaveMeasurementDataToMDB(acceptNo);
                    }
                    else
                    {
                        MessageBox.Show("유효하지 않은 바코드입니다. 다시 시도해주세요.", "바코드 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_acceptNo.Clear();
                        txt_acceptNo.Focus();
                    }
                }
                else
                {
                    if (ValidateBarcode(lbl_currentVehicle.Text.Trim()))
                    {
                        vehicleBarcode = lbl_currentVehicle.Text.Trim();
                        txt_acceptNo.Text = vehicleBarcode;

                        angle = GetCurrentInclination();
                        ControlByInclineAngle(angle);

                        acceptNo = SendWeightDataToServer();
                        db.SaveMeasurementDataToMDB(acceptNo);
                    }
                    else
                    {
                        MessageBox.Show("유효하지 않은 바코드입니다. 다시 시도해주세요.", "바코드 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        txt_acceptNo.Clear();
                        txt_acceptNo.Focus();
                    }
                }
            }
        }
    }
}
