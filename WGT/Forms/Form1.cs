using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Net.Sockets;
using System.Globalization;
using CommonIniFile;

using WGT.Forms;

namespace WGT
{
    public partial class Form1 : Form
    {
        // 프로세스 상태 관리
        private enum ProcessState
        {
            StandBy, // 대기 모드
            BarcodeInput, // 바코드 입력 모드
            FrontWheelEntry, // 전륜 진입
            FrontWheelMeasure, // 전륜 측정
            VehicleMoving1, // 차량 이동 (전륜)
            RearWheelEntry, // 후륜 진입
            RearWheelMeasure, // 후륜 측정
            VehicleMoving2, // 차량 이동 (후륜)
            DataSaving // 데이터 저장
        }

        private ProcessState currentState = ProcessState.StandBy;
        public string vehicleBarcode { get; private set; }
        private bool frontWheelmeasured = false;
        private bool rearWheelMeasured = false;
        private Timer processTimer;
        private Timer measureTimer;
        
        private int currentSequence = 1;
        public string formattedBarcode { get; private set; }

        // 측정 데이터 저장
        public double frontLeftWeight { get; private set; }
        public double frontRightWeight { get; private set; }
        public double frontTotalWeight { get; private set; }
        public double rearLeftWeight { get; private set; }
        public double rearRightWeight { get; private set; }
        public double rearTotalWeight { get; private set; }
        public double totalWeight { get; private set; }

        // INI파일 데이터
        private string ipAddress;
        private string portNumber;
        private double minimumWeight = 300.0;
        private double runoutWeight = 10.0;
        private string iniFilePath = Path.Combine(Application.StartupPath, "Config", "WGT.ini");
        private SettingDb db;

        public Form1()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.Size = new Size(1920, 1080);

            db = new SettingDb(this);

            LoadConfigFromIni();
            db.SetupDatabaseConnection();

            // 프로세스 타이머 설정
            processTimer = new Timer();
            processTimer.Interval = 1000;
            processTimer.Tick += ProcessTimer_Tick;

            // 측정 타이머 설정
            measureTimer = new Timer();
            measureTimer.Interval = 1000;
            measureTimer.Tick += MeasureTimer_Tick;

            // 초기 상태 설정
            SetState(ProcessState.StandBy);
        }

        // INI 파일에서 설정 로드
        private void LoadConfigFromIni()
        {
            try
            {
                IniFile iniFile = new IniFile(iniFilePath);

                ipAddress = iniFile.ReadString("Network", "IP");
                portNumber = iniFile.ReadString("Network", "Port");
                minimumWeight = iniFile.ReadDouble("Weight", "MinimumWeight", minimumWeight);
                runoutWeight = iniFile.ReadDouble("Weight", "Runout", runoutWeight);
            }
            catch (Exception ex)
            {
                MessageBox.Show("INI 파일을 읽는 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 프로세스에 따른 작업 수행
        private void ProcessTimer_Tick(object sender, EventArgs e)
        {
            switch (currentState)
            {
                case ProcessState.StandBy:
                    break;

                case ProcessState.BarcodeInput:
                    break;

                case ProcessState.FrontWheelEntry:
                    if (GetCurrentWeight() >= minimumWeight) // 전륜 진입 조건 (300kg 이상)
                    {
                        SetState(ProcessState.FrontWheelMeasure);
                    }

                    break;

                case ProcessState.FrontWheelMeasure:
                    break;

                case ProcessState.VehicleMoving1:
                    if (GetCurrentWeight() <= runoutWeight)
                    {
                        SetState(ProcessState.RearWheelEntry);
                    }

                    break;

                case ProcessState.RearWheelEntry:
                    if (GetCurrentWeight() >= minimumWeight) // 후륜 진입 조건 (300kg 이상)
                    {
                        SetState(ProcessState.RearWheelMeasure);
                    }

                    break;

                case ProcessState.RearWheelMeasure:
                    break;

                case ProcessState.VehicleMoving2:
                    if (GetCurrentWeight() <= runoutWeight)
                    {
                        SetState(ProcessState.DataSaving);
                    }

                    break;

                case ProcessState.DataSaving:
                    SetState(ProcessState.StandBy);
                    break;
            }
        }

        // 측정 상태에 따른 데이터 수집
        private void MeasureTimer_Tick(object sender, EventArgs e)
        {
            if (currentState == ProcessState.FrontWheelMeasure)
            {
                if (CollectFrontWheelData())
                {
                    frontWheelmeasured = true;
                    measureTimer.Stop();
                    SetState(ProcessState.VehicleMoving1);
                }
            }
            else if (currentState == ProcessState.RearWheelMeasure)
            {
                if (CollectRearWheelData())
                {
                    rearWheelMeasured = true;
                    measureTimer.Stop();
                    SetState(ProcessState.VehicleMoving2);
                }
            }
        }

        private void ReloadConfig()
        {
            LoadConfigFromIni();
        }

        // 상태 전환
        private void SetState(ProcessState newState)
        {
            currentState = newState;

            switch (newState)
            {
                case ProcessState.StandBy:
                    UpdateStatusMessage("대기모드", "바코드 대기 중입니다");
                    InitializeValues();
                    frontWheelmeasured = false;
                    rearWheelMeasured = false;
                    txt_acceptNo.Enabled = true;

                    processTimer.Start();
                    break;

                case ProcessState.BarcodeInput:
                    UpdateStatusMessage("바코드 입력", vehicleBarcode + " 차량 측정 대기중 입니다.");
                    break;

                case ProcessState.FrontWheelEntry:
                    UpdateStatusMessage("전륜 진입", "전륜 측정 준비 중입니다.");
                    break;

                case ProcessState.FrontWheelMeasure:
                    UpdateStatusMessage("전륜 측정", "측정중");
                    measureTimer.Start();
                    break;

                case ProcessState.VehicleMoving1:
                    UpdateStatusMessage("차량 이동 (전륜)", "전륜 측정 완료!\n 차량을 이동해주세요.");
                    break;

                case ProcessState.RearWheelEntry:
                    UpdateStatusMessage("후륜 진입", "후륜 측정 준비 중입니다.");
                    break;

                case ProcessState.RearWheelMeasure:
                    UpdateStatusMessage("후륜 측정", "측정중");
                    measureTimer.Start();
                    break;

                case ProcessState.VehicleMoving2:
                    UpdateStatusMessage("차량 이동 (후륜)", "후륜 측정 완료!\n 차량을 이동해주세요.");
                    break;

                case ProcessState.DataSaving:
                    UpdateStatusMessage("데이터 저장", "측정 데이터를 저장 중입니다.");
                    SendWeightDataToServer();
                    db.SaveMeasurementDataToMDB(vehicleBarcode);
                    break;
            }
        }

        private void UpdateStatusMessage(string title, string message)
        {
            if (lbl_processTitle != null && lbl_processMessage != null)
            {
                lbl_processTitle.Text = title;
                lbl_processMessage.Text = message;
            }

            Console.WriteLine($"[{DateTime.Now}] State : {title} - {message}");
        }

        // 현재 무게 측정 (*** 센서에서 무게 읽어오는 코드로 수정 필요, 흔들림 10kg 이하 측정 필요)
        private double GetCurrentWeight()
        {
            Random rand = new Random();

            return rand.Next(0, 1000);
        }

        // 전륜 측정 데이터 수집 (3회 측정을 가정)
        private int frontMeasureCount = 0;
        private bool CollectFrontWheelData()
        {
            frontMeasureCount++;

            // (*** 센서에서 무게 읽어오는 코드로 수정 필요)
            Random rand = new Random();
            double leftWeight = 250 + rand.NextDouble() * 50; // 250kg ~ 300kg 사이의 무게
            double rightWeight = 250 + rand.NextDouble() * 50;

            lbl_frontLeft.Text = leftWeight.ToString("F1");
            lbl_frontRight.Text = rightWeight.ToString("F1");
            lbl_frontCenter.Text = (leftWeight + rightWeight).ToString("F1");

            if (frontMeasureCount >= 3)
            {
                frontLeftWeight = double.Parse(lbl_frontLeft.Text);
                frontRightWeight = double.Parse(lbl_frontRight.Text);
                frontTotalWeight = double.Parse(lbl_frontCenter.Text);

                frontMeasureCount = 0;

                return true; // 측정 완료
            }

            return false; // 측정 중
        }

        // 후륜 측정 데이터 수집
        private int rearMeasureCount = 0;
        private bool CollectRearWheelData()
        {
            rearMeasureCount++;

            Random rand = new Random();
            double leftWeight = 300 + rand.NextDouble() * 50;
            double rightWeight = 300 + rand.NextDouble() * 50;

            lbl_rearLeft.Text = leftWeight.ToString("F1");
            lbl_rearRight.Text = rightWeight.ToString("F1");
            lbl_rearCenter.Text = (leftWeight + rightWeight).ToString("F1");

            if (rearMeasureCount >= 3)
            {
                rearLeftWeight = double.Parse(lbl_rearLeft.Text);
                rearRightWeight = double.Parse(lbl_rearRight.Text);
                rearTotalWeight = double.Parse(lbl_rearCenter.Text);

                totalWeight = frontTotalWeight + rearTotalWeight;

                lbl_processMessage.Text = totalWeight.ToString("F1");

                rearMeasureCount = 0;

                return true;
            }

            return false;
        }

        private void InitializeValues()
        {
            // 초기값 설정
            lbl_frontLeft.Text = "0.0";
            lbl_frontCenter.Text = "0.0";
            lbl_frontRight.Text = "0.0";
            lbl_rearLeft.Text = "0.0";
            lbl_rearCenter.Text = "0.0";
            lbl_rearRight.Text = "0.0";
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

        private void ProcessBarcodeAndProceed()
        {
            try
            {
                Console.WriteLine($"[{DateTime.Now}] 바코드 처리 완료 : {vehicleBarcode}");

                txt_acceptNo.Enabled = false;

                UpdateStatusMessage("바코드 확인 완료", $"{vehicleBarcode} 차량 측정을 시작합니다.");

                Timer transitionTimer = new Timer();
                transitionTimer.Interval = 2000; // 2초 후에 상태 전환
                transitionTimer.Tick += (s, e) =>
                {
                    transitionTimer.Stop();

                    SetState(ProcessState.FrontWheelEntry);
                };

                transitionTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("바코드 처리 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_acceptNo.Clear();
                txt_acceptNo.Focus();
            }
        }

        public void SendWeightDataToServer()
        {
            string datePrefix = DateTime.Now.ToString("yyyyMMdd");
            string sequentialNumber = currentSequence.ToString("D4"); // 4자리 순차번호
            currentSequence++;
            formattedBarcode = datePrefix + sequentialNumber;

            string weightData = string.Format(CultureInfo.InvariantCulture, "{0}/{1:F1}/{2:F1}/{3:F1}/{4:F1}/{5:F1}/{6:F1}", formattedBarcode,
                frontLeftWeight, frontRightWeight, frontTotalWeight,
                rearLeftWeight, rearRightWeight, rearTotalWeight);

            int dataCount = 7;
            string packet = $"<RST/WGT/{weightData}/{dataCount}>";

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
            }
            catch (Exception ex)
            {
                MessageBox.Show("서버 전송 실패: " + ex.Message, "전송 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 수동 입력
        private void btn_manualInput_Click(object sender, EventArgs e)
        {
            using (ManualInputForm manualInputForm = new ManualInputForm())
            {
                if (manualInputForm.ShowDialog() == DialogResult.OK)
                {
                    // 전륜 데이터 설정
                    frontLeftWeight = manualInputForm.FrontLeft;
                    frontRightWeight = manualInputForm.FrontRight;
                    frontTotalWeight = frontLeftWeight + frontRightWeight;

                    // 후륜 데이터 설정
                    rearLeftWeight = manualInputForm.RearLeft;
                    rearRightWeight = manualInputForm.RearRight;
                    rearTotalWeight = rearLeftWeight + rearRightWeight;

                    // 총 중량 계산
                    totalWeight = frontTotalWeight + rearTotalWeight;

                    // UI 업데이트
                    lbl_frontLeft.Text = manualInputForm.FrontLeft.ToString("F1");
                    lbl_frontRight.Text = manualInputForm.FrontRight.ToString("F1");
                    lbl_frontCenter.Text = frontTotalWeight.ToString("F1");
                    lbl_rearLeft.Text = manualInputForm.RearLeft.ToString("F1");
                    lbl_rearRight.Text = manualInputForm.RearRight.ToString("F1");
                    lbl_rearCenter.Text = rearTotalWeight.ToString("F1");
                    lbl_processMessage.Text = totalWeight.ToString("F1");

                    frontWheelmeasured = true;
                    rearWheelMeasured = true;
                }
            }
        }

        private void btn_setAllZero_Click(object sender, EventArgs e)
        {
            InitializeValues();
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_config_Click(object sender, EventArgs e)
        {
            using (ConfigForm configForm = new ConfigForm())
            {
                if (configForm.ShowDialog() == DialogResult.OK)
                {
                    ReloadConfig();
                }
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
            vehicleBarcode = txt_acceptNo.Text.Trim();

            if (!string.IsNullOrEmpty(vehicleBarcode))
            {
                // 바코드 유효성 검사 및 처리
                if (ValidateBarcode(vehicleBarcode))
                {
                    // 바코드가 유효한 경우 BarcodeInput 상태로 전환
                    SetState(ProcessState.BarcodeInput);

                    // 추가 처리 후 FrontWheelEntry 상태로 전환
                    ProcessBarcodeAndProceed();

                    lbl_currentVehicle.Text = txt_acceptNo.Text;
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

                    SetState(ProcessState.BarcodeInput);
                    ProcessBarcodeAndProceed();
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
