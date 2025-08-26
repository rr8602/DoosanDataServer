using CommonIniFile;
using Incline.Comm;
using Incline.Forms;
using Incline.Models;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace Incline
{
    public partial class Incline : Form
    {
        public const int MOTOR = 0;
        public const int BUZZER = 1;
        public const int LIFT_UP = 2;
        public const int LIFT_DOWN = 3;
        public const int LAMP_GREEN = 4;
        public const int LAMP_YELLOW = 5;
        public const int LAMP_RED = 6;
        public const int SPARE = 7;

        private IoBoard ioBoard;
        private Sensor sensor;
        private SettingDb db;
        private InclineTcpClient inclineTcpClient;

        private Timer sensorTimer;
        private Timer ioTimer;
        private Timer commActivityTimer;

        private InclineMeasurement currentMeasurement;

        private DateTime lastIoBoardRxTime = DateTime.MinValue;
        private DateTime lastSensorRxTime = DateTime.MinValue;

        private float currentValue = 0.0f;
        private byte currentOutputState = 0;
        private bool requestSent = false;

        // INI 파일 데이터
        private string ipAddress;
        private int portNumber;
        private double maxInclineAngle;
        private double minInclineAngle;
        private string ioPortName;
        private string sensorPortName;
        private string iniFilePath = Path.Combine(Application.StartupPath, "Config", "Incline.ini");

        private Bitmap backgroundImage;
        private bool isBackgroundInitialized = false;

        private bool[] inputStates = new bool[8];

        public Incline()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;

            typeof(Panel).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic,
                null, panel_arcGauge, new object[] { true });

            commActivityTimer = new Timer();
            commActivityTimer.Interval = 1000;
            commActivityTimer.Tick += CommActivityTimer_Tick;
            commActivityTimer.Start();

            currentMeasurement = new InclineMeasurement();
            db = new SettingDb();

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
                if (backgroundImage != null)
                    backgroundImage.Dispose();

                backgroundImage = new Bitmap(panel_arcGauge.Width, panel_arcGauge.Height);
                isBackgroundInitialized = false;
            }
        }

        public void ReloadSettingsAndRedrawGauge()
        {
            LoadConfigFromIni();
            isBackgroundInitialized = false;
            panel_arcGauge.Invalidate();
        }

        public void LoadConfigFromIni()
        {
            try
            {
                IniFile iniFile = new IniFile(iniFilePath);

                ipAddress = iniFile.ReadString("Network", "IP");
                portNumber = iniFile.ReadInteger("Network", "Port");
                maxInclineAngle = iniFile.ReadDouble("Angle", "MaxAngle", 60.0);
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
            ioTimer = new Timer();
            ioTimer.Interval = 200;
            ioTimer.Tick += IoTimer_Tick;
        }

        private void InitializeSensor()
        {
            sensor = new Sensor();
            sensor.OnRxDataReceived += Sensor_OnRxDataReceived;
            sensor.OnAngleChanged += Sensor_OnAngleChanged;
            sensorTimer = new Timer();
            sensorTimer.Interval = 100;
            sensorTimer.Tick += sensorTimer_Tick;
        }

        private void CommActivityTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            TimeSpan ioBoardElapsed = now - lastIoBoardRxTime;
            lbl_ioBoardComm.ForeColor = (ioBoardElapsed.TotalSeconds > 3) ? Color.Red : Color.Green;
            TimeSpan sensorElapsed = now - lastSensorRxTime;
            lbl_sensorComm.ForeColor = (sensorElapsed.TotalSeconds > 3) ? Color.Red : Color.Green;
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
                lbl_incAngle.Text = currentValue.ToString("0.0");
                currentMeasurement.InclineAngle = angle;
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

            for (int i = 0; i < 8; i++)
            {
                inputStates[i] = (data & (1 << i)) != 0;
            }
        }

        private void IoBoard_OnOutputDataSent(byte outputData, bool success) { }

        public bool ConnectIOBoard(string portName)
        {
            bool result = ioBoard.Connect(portName);

            if (result)
            {
                requestSent = false;
                ioTimer.Start();
                lbl_message.Text = "IO 보드 연결\n성공";
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
            if (result)
            {
                sensor.SendCommand();
                sensorTimer.Start();
                lbl_message.Text = "센서 연결 성공";
            }
            else
            {
                MessageBox.Show("센서 연결에 실패했습니다.", "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return result;
        }

        public void SetOutput(byte outputValue)
        {
            if (ioBoard.IsConnected)
            {
                currentOutputState = outputValue;
                ioBoard.SetOutputStatus(outputValue);
            }
        }

        private void panel_arcGauge_Paint(object sender, PaintEventArgs e)
        {
            if (!isBackgroundInitialized)
            {
                InitializeArcGauge();
                DrawBackgroundOnce();
            }
            e.Graphics.DrawImage(backgroundImage, 0, 0);

            int radius = (int)(panel_arcGauge.ClientSize.Width / 1.7);
            int centerX = (panel_arcGauge.ClientSize.Width / 2) + 300;
            int centerY = panel_arcGauge.ClientSize.Height - 20;

            DrawPointer(e.Graphics, centerX, centerY, radius, currentValue);
        }

        private Rectangle UpdatePointer()
        {
            int radius = (int)(panel_arcGauge.ClientSize.Width / 1.7);
            int centerX = (panel_arcGauge.ClientSize.Width / 2) + 300;
            int centerY = panel_arcGauge.ClientSize.Height - 20;
            
            return new Rectangle(centerX - radius, centerY - radius, radius * 2, radius * 2);
        }

        private double MapAngleToDegrees(double angle)
        {
            double range = maxInclineAngle - minInclineAngle;
            if (range <= 0) return 180;
            double normalized = (angle - minInclineAngle) / range;
            return 180 - (normalized * 90.0);
        }

        private void DrawColoredSegmentsAndTicks(Graphics g, int centerX, int centerY, int radius)
        {
            int innerRadius = radius - 40;
            int outerRadius = radius - 5;

            for (double angle = minInclineAngle; angle < maxInclineAngle; angle += 0.5)
            {
                Color segmentColor;
                if (angle >= 45 && angle <= maxInclineAngle) segmentColor = Color.Red;
                else if (angle >= 35 && angle < 45) segmentColor = Color.Green;
                else segmentColor = Color.Yellow;

                double startRadians = MapAngleToDegrees(angle) * Math.PI / 180;
                double endRadians = MapAngleToDegrees(angle + 0.5) * Math.PI / 180;

                PointF[] points = {
                    new PointF(centerX + (float)(Math.Cos(startRadians) * innerRadius), centerY - (float)(Math.Sin(startRadians) * innerRadius)),
                    new PointF(centerX + (float)(Math.Cos(endRadians) * innerRadius), centerY - (float)(Math.Sin(endRadians) * innerRadius)),
                    new PointF(centerX + (float)(Math.Cos(endRadians) * outerRadius), centerY - (float)(Math.Sin(endRadians) * outerRadius)),
                    new PointF(centerX + (float)(Math.Cos(startRadians) * outerRadius), centerY - (float)(Math.Sin(startRadians) * outerRadius))
                };

                g.FillPolygon(new SolidBrush(segmentColor), points);
            }

            for (int angle = (int)minInclineAngle; angle <= (int)maxInclineAngle; angle += 10)
            {
                 if (angle < minInclineAngle) continue;

                double radians = MapAngleToDegrees(angle) * Math.PI / 180;
                float startX = centerX + (float)(Math.Cos(radians) * innerRadius);
                float startY = centerY - (float)(Math.Sin(radians) * innerRadius);
                float endX = centerX + (float)(Math.Cos(radians) * radius);
                float endY = centerY - (float)(Math.Sin(radians) * radius);
                Pen tickPen = (angle % 10 == 0) ? new Pen(Color.Black, 2) : new Pen(Color.Gray, 1);
                g.DrawLine(tickPen, startX, startY, endX, endY);

                if (angle % 10 == 0)
                {
                    float labelRadius = radius - 70;
                    float x = centerX + (float)(Math.Cos(radians) * labelRadius);
                    float y = centerY - (float)(Math.Sin(radians) * labelRadius);
                    StringFormat format = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                    g.DrawString(angle.ToString(), new Font("Arial", 20, FontStyle.Bold), Brushes.Black, x, y, format);
                }
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
            currentValue = (float)gatAngle;
            lbl_incAngle.Text = currentValue.ToString("0.0");
            currentMeasurement.InclineAngle = gatAngle;
            ControlByInclineAngle(gatAngle);
            Rectangle invalidateRect = UpdatePointer();
            panel_arcGauge.Invalidate(invalidateRect);
        }

        private void DrawBackgroundOnce()
        {
            if (isBackgroundInitialized) return;

            using (Graphics g = Graphics.FromImage(backgroundImage))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(panel_arcGauge.BackColor);

                int radius = (int)(panel_arcGauge.ClientSize.Width / 1.7);
                int centerX = (panel_arcGauge.ClientSize.Width / 2) + 300;
                int centerY = panel_arcGauge.ClientSize.Height - 20;

                g.FillPie(Brushes.Ivory, centerX - radius, centerY - radius, radius * 2, radius * 2, 180, 90);
                g.DrawArc(new Pen(Color.Black, 2), centerX - radius, centerY - radius, radius * 2, radius * 2, 180, 90);

                DrawColoredSegmentsAndTicks(g, centerX, centerY, radius);

                float horizontalLineY = centerY;
                g.DrawLine(new Pen(Color.Black, 4), centerX - radius, horizontalLineY, centerX, horizontalLineY);
            }

            isBackgroundInitialized = true;
        }

        private void DrawPointer(Graphics g, int centerX, int centerY, int radius, float value)
        {
            float clampedValue = Math.Max((float)minInclineAngle, Math.Min(value, (float)maxInclineAngle));
            
            double radians = MapAngleToDegrees(clampedValue) * Math.PI / 180;

            float pointerEndX = centerX + (float)(Math.Cos(radians) * (radius - 40));
            float pointerEndY = centerY - (float)(Math.Sin(radians) * (radius - 40));
            float perpX = (float)Math.Sin(radians) * 12;
            float perpY = (float)Math.Cos(radians) * 12;
            PointF[] pointerShape = { new PointF(centerX + perpX, centerY + perpY), new PointF(centerX - perpX, centerY - perpY), new PointF(pointerEndX, pointerEndY) };
            g.FillPolygon(Brushes.Blue, pointerShape);
            g.FillEllipse(Brushes.DarkBlue, centerX - 10, centerY - 10, 20, 20);
            g.DrawEllipse(new Pen(Color.Black, 1), centerX - 10, centerY - 10, 20, 20);
            g.FillEllipse(Brushes.Silver, centerX - 5, centerY - 5, 10, 10);
            g.DrawEllipse(new Pen(Color.Black, 1), centerX - 5, centerY - 5, 10, 10);
        }

        public void ControlByInclineAngle(double angle)
        {
            if (angle > maxInclineAngle || angle < minInclineAngle || (inputStates[2] && inputStates[3] && inputStates[4] && inputStates[5]))
            {
                LiftOffSignal();
            }
        }

        private void InclineTcpClient_VehicleInfoReceived(object sender, VehicleInfoEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new EventHandler<VehicleInfoEventArgs>(InclineTcpClient_VehicleInfoReceived), sender, e);
                return;
            }

            currentMeasurement = new InclineMeasurement
            {
                AcceptNo = e.AcceptNo,
                VinNo = e.VinNo,
                Model = e.Model
            };

            lbl_currentVehicle.Text = currentMeasurement.VinNo;

            Console.WriteLine($"REG 수신된 데이터: AcceptNo={currentMeasurement.AcceptNo}, VinNo={currentMeasurement.VinNo}, Model={currentMeasurement.Model}");
        }

        public void SendInclineDataToServer(string acceptNo, double data)
        {
            string incData = string.Format(CultureInfo.InvariantCulture, "{0}/{1:F2}", acceptNo, data);
            string packet = $"<RST/ANG/{incData}>";
            int dataCount = packet.Split('/').Length + 1;
            string packetResult = $"<RST/ANG/{incData}/{dataCount}>";

            if (inclineTcpClient != null && inclineTcpClient.IsConnected)
            {
                inclineTcpClient.SendData(packetResult);
            }
            else
            {
                MessageBox.Show("서버에 연결되어 있지 않습니다.", "전송 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetMultipleOutputPins(int[] pinNumbers, bool state)
        {
            if (pinNumbers == null || pinNumbers.Length == 0) return;

            foreach (int pin in pinNumbers)
            {
                if (pin >= 0 && pin <= 7)
                {
                    if (state) currentOutputState |= (byte)(1 << pin);
                    else currentOutputState &= (byte)~(1 << pin);
                }
            }

            SetOutput(currentOutputState);
        }

        public void LiftUpOnSignal(bool state) { if (state) SetMultipleOutputPins(new int[] { MOTOR, BUZZER, LIFT_DOWN }, state); }
        public void LiftDownOnSignal(bool state) { if (state) SetMultipleOutputPins(new int[] { MOTOR, LIFT_UP, LIFT_DOWN }, state); }
        public void LiftOffSignal() { currentOutputState = 0; SetOutput(currentOutputState); }

        private void Incline_Load(object sender, EventArgs e)
        {
            lbl_message.Text = "검사시작 버튼을 눌러주세요.";
            lbl_incAngle.Text = "0.0";
            lbl_ioBoardComm.ForeColor = Color.Green;
            lbl_sensorComm.ForeColor = Color.Green;
            btn_inspectionCompelete.Enabled = false;
            currentValue = 0.0f;
            InitializeArcGauge();
            panel_arcGauge.Invalidate();
            ConnectIOBoard("COM1");
            ConnectSensor("COM2");
            LiftOffSignal();
            inclineTcpClient = new InclineTcpClient();
            inclineTcpClient.VehicleInfoReceived += InclineTcpClient_VehicleInfoReceived;
            inclineTcpClient.LogMessage += (s, msg) => Console.WriteLine(msg);
            inclineTcpClient.Connect(ipAddress, portNumber);
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
                if (ioBoard != null && ioBoard.IsConnected) { ioTimer?.Stop(); ioBoard?.Disconnect(); }
                if (sensor != null && sensor.IsConnected) { sensorTimer?.Stop(); sensor?.Disconnect(); }
                inclineTcpClient?.Disconnect();
                commActivityTimer?.Stop();
                commActivityTimer?.Dispose();
                ioTimer?.Dispose();
                sensorTimer?.Dispose();
            }
            catch (Exception ex) { Debug.WriteLine($"종료 중 오류: {ex.Message}"); }
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
                    this.currentMeasurement = listform.SelectedMeasurement;
                    lbl_currentVehicle.Text = this.currentMeasurement.VinNo;
                }
            }
        }

        private void btn_inspectionStart_Click(object sender, EventArgs e)
        {
            if (ioBoard.IsConnected == false)
            {
                MessageBox.Show("IO 보드에 연결되어 있지 않습니다. 먼저 연결해주세요.", "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrEmpty(currentMeasurement.VinNo))
            {
                lbl_currentVehicle.Text = currentMeasurement.VinNo;
                lbl_message.Text = "검사를 시작합니다.";
                btn_inspectionStart.BackColor = Color.Red;
                btn_inspectionStart.Enabled = false;
                btn_inspectionCompelete.Enabled = true;
            }
            else
            {
                MessageBox.Show("차량 정보가 없습니다. 검사할 차량을 먼저 선택해 주세요.", "데이터 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool confirmOkNg()
        {
            double angle = currentMeasurement.InclineAngle;

            if (angle >= 35 && angle <= 45)
            {
                lbl_message.Text = "검사 합격";
                return true;
            }
            
            return false;
        }

        private void btn_inspectionCompelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(currentMeasurement.VinNo))
                {
                    currentMeasurement.OkNg = confirmOkNg();
                    currentMeasurement.InspectionStatus = true;
                    currentMeasurement.MeaDate = DateTime.Now;
                    btn_inspectionStart.Enabled = true;
                    btn_inspectionCompelete.Enabled = false;
                    lbl_result.Text = currentMeasurement.InclineAngle.ToString("0.0");
                    lbl_message.Text = "검사 종료 되었습니다.";

                    if (currentMeasurement.OkNg)
                        lbl_result.ForeColor = Color.Green;
                    else
                        lbl_result.ForeColor = Color.Red;

                    db.SaveMeasurementDataToMDB(currentMeasurement);

                    SendInclineDataToServer(currentMeasurement.AcceptNo, currentMeasurement.InclineAngle);
                }
                else
                {
                    MessageBox.Show("접수번호나 차대번호가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 저장 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show("IO 보드가 연결되어 있지 않습니다.\n설정 메뉴에서 IO 보드를 먼저 연결해주세요.", "연결 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void panel_arcGauge_Resize(object sender, EventArgs e) { isBackgroundInitialized = false; panel_arcGauge.Invalidate(); }
        private void Incline_FormClosed(object sender, FormClosedEventArgs e) { LiftOffSignal(); }
    }
}