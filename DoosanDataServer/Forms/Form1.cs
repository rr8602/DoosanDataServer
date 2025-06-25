using DoosanDataServer.Comm;
using DoosanDataServer.Database;
using DoosanDataServer.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoosanDataServer
{
    public partial class Form1 : Form
    {
        private SerialBarcodeReader barcodeReader;
        private TcpServer tcpServer;
        private string currentChassisNumber;
        private DatabaseManager dbManager;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public Form1()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.Size = new Size(1920, 1080);

            dataGridView1.AllowUserToAddRows = false;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            //barcodeReader = new SerialBarcodeReader("COM7");
            tcpServer = new TcpServer("192.168.10.98", 5000);
            dbManager = new DatabaseManager();

            tcpServer.InspectionResultReceived += TcpServer_InspectionResultReceived;
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //barcodeReader.Open();
                tcpServer.Start();

                //InitializeDataGridView();
                LoadAllAcceptNumbers();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"초기화 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                tcpServer.Stop();
                //barcodeReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"종료 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TcpServer_InspectionResultReceived(object sender, InspectionResultEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { TcpServer_InspectionResultReceived(sender, e); });

                return;
            }

            string vinNo = txt_vinNo.Text.Trim();

            if (e.EquipmentType == EquipmentType.HLT)
            {
                currentChassisNumber = vinNo;

                ProcessHeadlightData(e.Result);
            }
            else if (e.EquipmentType == EquipmentType.WGT)
            {
                currentChassisNumber = vinNo;

                ProcessWgtData(e.Result);
            }
            else if (e.EquipmentType == EquipmentType.ANG)
            {
                currentChassisNumber = vinNo;

                ProcessAngData(e.Result);
            }
            else if (e.EquipmentType == EquipmentType.ABS)
            {
                currentChassisNumber = vinNo;

                ProcessAbsData(e.Result);
            }
        }

        private void ProcessHeadlightData(string[] parts)
        {
            /*if (parts.Length < 20)
            {
                MessageBox.Show("헤드라이트 데이터 형식이 올바르지 않습니다.", "데이터 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/

            try
            {
                if (parts[4] == "상향")
                {
                    lbl_upward_lcd.Text = parts[5];
                    SetTextBoxColor(lbl_upward_lcd, parts[6]);

                    lbl_upward_lud.Text = parts[7];
                    SetTextBoxColor(lbl_upward_lud, parts[8]);

                    lbl_upward_llr.Text = parts[9];
                    SetTextBoxColor(lbl_upward_llr, parts[10]);

                    lbl_upward_rcd.Text = parts[11];
                    SetTextBoxColor(lbl_upward_rcd, parts[12]);

                    lbl_upward_rud.Text = parts[13];
                    SetTextBoxColor(lbl_upward_rud, parts[14]);

                    lbl_upward_rlr.Text = parts[15];
                    SetTextBoxColor(lbl_upward_rlr, parts[16]);

                    string upBeamResult = parts[17];
                    lbl_upward_totalPan.Text = upBeamResult;
                    SetTextBoxColor(lbl_upward_totalPan, upBeamResult);

                    dbManager.SaveHeadLightData(
                        parts[2],
                        currentChassisNumber,
                        "상향",
                        parts[5], parts[6],
                        parts[7], parts[8],
                        parts[9], parts[10],
                        parts[11], parts[12],
                        parts[13], parts[14],
                        parts[15], parts[16],
                        upBeamResult);
                }
                else
                {
                    lbl_downward_lcd.Text = parts[5];
                    SetTextBoxColor(lbl_downward_lcd, parts[6]);

                    lbl_downward_lud.Text = parts[7];
                    SetTextBoxColor(lbl_downward_lud, parts[8]);

                    lbl_downward_llr.Text = parts[9];
                    SetTextBoxColor(lbl_downward_llr, parts[10]);

                    lbl_downward_rcd.Text = parts[11];
                    SetTextBoxColor(lbl_downward_rcd, parts[12]);

                    lbl_downward_rud.Text = parts[13];
                    SetTextBoxColor(lbl_downward_rud, parts[14]);

                    lbl_downward_rlr.Text = parts[15];
                    SetTextBoxColor(lbl_downward_rlr, parts[16]);

                    string lowBeamResult = parts[17];
                    lbl_downward_totalPan.Text = lowBeamResult;
                    SetTextBoxColor(lbl_downward_totalPan, lowBeamResult);

                    dbManager.SaveHeadLightData(
                        parts[2],
                        currentChassisNumber,
                        "하향",
                        parts[5], parts[6],  // 좌측 상하 값, 결과
                        parts[7], parts[8],  // 좌측 상하 값, 결과
                        parts[9], parts[10], // 좌측 좌우 값, 결과
                        parts[11], parts[12], // 우측 상하 값, 결과
                        parts[13], parts[14], // 우측 상하 값, 결과
                        parts[15], parts[16], // 우측 좌우 값, 결과
                        lowBeamResult);   // 종합 결과
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"헤드라이트 데이터 처리 중 오류 : {ex.Message}", "처리 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessWgtData(string[] parts)
        {
            /*if (parts.Length < 9)
            {
                MessageBox.Show("중량계 데이터 형식이 올바르지 않습니다.", "데이터 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/

            try
            {
                lbl_flw.Text = parts[3];
                lbl_frw.Text = parts[4];
                lbl_ftw.Text = parts[5];
                lbl_rlw.Text = parts[6];
                lbl_rrw.Text = parts[7];
                lbl_rtw.Text = parts[8];

                dbManager.SaveWeightData(
                    parts[2],
                    currentChassisNumber, // 차량 번호
                    parts[3],  // 전륜 좌측
                    parts[4],  // 전륜 우측
                    parts[5],  // 전륜 합계
                    parts[6],  // 후륜 좌측
                    parts[7],  // 후륜 우측
                    parts[8]   // 후륜 합계
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show($"중량계 데이터 처리 중 오류 : {ex.Message}", "처리 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessAngData(string[] parts)
        {
            /*if (parts.Length < 5)
            {
                MessageBox.Show("경사각도 데이터 형식이 올바르지 않습니다.", "데이터 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/

            try
            {
                lbl_incAngle.Text = parts[3];

                dbManager.SaveAngleData(parts[2], currentChassisNumber, parts[3]);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"경사각도 데이터 처리 중 오류 : {ex.Message}", "처리 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }

        private void ProcessAbsData(string[] parts)
        {
            try
            {
                // 브레이크
                lbl_frontWeight.Text = (int.Parse(parts[15]) + int.Parse(parts[27]).ToString());
                lbl_frontSum.Text = (int.Parse(parts[25]) + int.Parse(parts[37]).ToString());
                lbl_frontDiff.Text = ((Math.Abs(int.Parse(parts[23]) - int.Parse(parts[35])) / (int.Parse(parts[23]) + int.Parse(parts[35])) / 2) * 100).ToString();
                lbl_frontOkNg.Text = (parts[26] == "OK" && parts[38] == "OK") ? "OK" : "NG";
                SetTextBoxColor(lbl_frontOkNg, lbl_frontOkNg.Text);

                lbl_rearWeight.Text = (int.Parse(parts[39]) + int.Parse(parts[51]).ToString());
                lbl_rearSum.Text = (int.Parse(parts[49]) + int.Parse(parts[61]).ToString());
                lbl_rearDiff.Text = ((Math.Abs(int.Parse(parts[47]) - int.Parse(parts[59])) / (int.Parse(parts[47]) + int.Parse(parts[59])) / 2) * 100).ToString();
                lbl_rearOkNg.Text = (parts[52] == "OK" && parts[62] == "OK") ? "OK" : "NG";
                SetTextBoxColor(lbl_rearOkNg, lbl_rearOkNg.Text);

                lbl_totalWeight.Text = (int.Parse(parts[75]) + int.Parse(parts[76]).ToString());
                lbl_totalSum.Text = parts[77];
                lbl_totalDiff.Text = ((Math.Abs(int.Parse(parts[75]) - int.Parse(parts[76])) / (int.Parse(parts[75]) + int.Parse(parts[76])) / 2) * 100).ToString();
                lbl_totalOkNg.Text = parts[78];
                SetTextBoxColor(lbl_totalOkNg, lbl_totalOkNg.Text);

                lbl_parkingWeight.Text = (int.Parse(parts[80]) + int.Parse(parts[81]).ToString());
                lbl_parkingSum.Text = parts[82];
                lbl_parkingDiff.Text = ((Math.Abs(int.Parse(parts[80]) - int.Parse(parts[81])) / (int.Parse(parts[80]) + int.Parse(parts[81])) / 2) * 100).ToString();
                lbl_parkingOkNg.Text = parts[83];
                SetTextBoxColor(lbl_parkingOkNg, lbl_parkingOkNg.Text);

                // 속도계 (40km/h 까지 4초 이하로 걸렸냐 + 속도계 최대값은 어떤 기준으로 해야 할지 모르겠다)
                lbl_speedFourtyValue.Text = ((int.Parse(parts[86]) - int.Parse(parts[85])) / 1000).ToString();
                lbl_speedFourtyOkNg.Text = int.Parse(lbl_speedFourtyValue.Text) < 4.0 ? "OK" : "NG";
                SetTextBoxColor(lbl_speedFourtyOkNg, lbl_speedFourtyOkNg.Text);

                //lbl_speedMaxValue.Text = parts[86];
                //lbl_speedMaxOkNg.Text = 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ABS 데이터 처리 중 오류 : {ex.Message}", "처리 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetTextBoxColor(Label label, string result)
        {
            if (result.Trim().ToUpper() == "OK")
            {
                label.BackColor = Color.LightGreen;
                label.ForeColor = Color.Black;
            }
            else if (result.Trim().ToUpper() == "NG")
            {
                label.BackColor = Color.Red;
                label.ForeColor = Color.White;
            }
            else
            {
                label.BackColor = SystemColors.Window;
                label.ForeColor = SystemColors.WindowText;
            }
        }

        private void LoadAllAcceptNumbers()
        {
            try
            {
                DataTable acceptData = dbManager.GetAllAcceptNumbers();

                DisplayAcceptNumbersInGrid(acceptData);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"접수번호 데이터 로드 중 오류 발생: {ex.Message}", "데이터 로드 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayAcceptNumbersInGrid(DataTable data)
        {
            dataGridView1.Rows.Clear();

            if (data == null || data.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in data.Rows)
            {
                int index = dataGridView1.Rows.Add();
                dataGridView1.Rows[index].Cells["Accept_No"].Value = row["Accept_No"];
                dataGridView1.Rows[index].Cells["Vin_No"].Value = row["Vin_No"];

                if (row["Mea_Date"] != DBNull.Value)
                {
                    DateTime meaDate = (DateTime)row["Mea_Date"];
                    dataGridView1.Rows[index].Cells["Mea_Date"].Value = meaDate.ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
        }

        private void btn_connectEquipment_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_vinNo.Text))
            {
                MessageBox.Show("접수번호를 입력해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_vinNo.Focus();

                return;
            }

            currentChassisNumber = txt_vinNo.Text.Trim();

            var equipmentAddresses = new Dictionary<EquipmentType, Tuple<string, int>>
            {
                { EquipmentType.ABS, Tuple.Create("192.168.10.98", 5001) },
                { EquipmentType.HLT, Tuple.Create("192.168.10.98", 5002) },
                { EquipmentType.ADAS, Tuple.Create("192.168.10.98", 5003) },
                { EquipmentType.ANG, Tuple.Create("192.168.10.98", 5004) },
                { EquipmentType.WGT, Tuple.Create("192.168.10.98", 5005) }
            };

            Task.Run(() =>
            {
                try
                {
                    tcpServer.ConnectToAllEquipment(equipmentAddresses, currentChassisNumber);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"설비 연결 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private void btn_saveCsv_Click(object sender, EventArgs e)
        {
            string acceptNo = string.Empty;
            string vinNo = string.Empty;

            if (dataGridView1.CurrentRow != null)
            {
                acceptNo = dataGridView1.CurrentRow.Cells["Accept_No"].Value.ToString();
                vinNo = dataGridView1.CurrentRow.Cells["Vin_No"].Value.ToString();
            }

            try
            {
                var data = dbManager.GetAllDataByAcceptNo(acceptNo);

                if (data.Count == 0)
                {
                    MessageBox.Show("저장할 데이터가 없습니다.", "정보", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "CSV 파일 (*.csv)|*.csv",
                    Title = "검사 데이터",
                    FileName = $"검사데이터_{acceptNo}_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                };

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8))
                    {
                        sw.WriteLine($"접수번호,=\"{acceptNo}\"");
                        sw.WriteLine($"차량번호,{vinNo}");
                        sw.WriteLine($"저장일시,{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
                        sw.WriteLine();

                        // 1. 경사각도 데이터
                        if (data.ContainsKey("Inc_Angle"))
                        {
                            sw.WriteLine("[경사각도 데이터]");
                            sw.WriteLine($"측정일시,{FormatValue(data, "Angle_Mea_Date")}");
                            sw.WriteLine($"경사각도,{FormatValue(data, "Inc_Angle")}");
                            sw.WriteLine();
                        }

                        // 2. 중량계 데이터
                        if (data.ContainsKey("F_LW"))
                        {
                            sw.WriteLine("[중량계 데이터]");
                            sw.WriteLine($"측정일시,{FormatValue(data, "Weight_Mea_Date")}");
                            sw.WriteLine($"전륜 좌측,{FormatValue(data, "F_LW")}");
                            sw.WriteLine($"전륜 우측,{FormatValue(data, "F_RW")}");
                            sw.WriteLine($"전륜 합계,{FormatValue(data, "F_TW")}");
                            sw.WriteLine($"후륜 좌측,{FormatValue(data, "R_LW")}");
                            sw.WriteLine($"후륜 우측,{FormatValue(data, "R_RW")}");
                            sw.WriteLine($"후륜 합계,{FormatValue(data, "R_TW")}");
                            sw.WriteLine($"총 중량,{FormatValue(data, "Total_Weight")}");
                            sw.WriteLine();
                        }

                        // 3. 헤드라이트 데이터
                        if (data.ContainsKey("Beam_Type"))
                        {
                            sw.WriteLine("[헤드라이트 데이터]");
                            sw.WriteLine($"측정일시,{FormatValue(data, "HeadLight_Mea_Date")}");
                            sw.WriteLine($"빔 타입,{FormatValue(data, "Beam_Type")}");
                            sw.WriteLine($"좌측 상하,{FormatValue(data, "L_CD")},{FormatValue(data, "L_CD_Result")}");
                            sw.WriteLine($"좌측 상하,{FormatValue(data, "L_UD")},{FormatValue(data, "L_UD_Result")}");
                            sw.WriteLine($"좌측 좌우,{FormatValue(data, "L_LR")},{FormatValue(data, "L_LR_Result")}");
                            sw.WriteLine($"우측 상하,{FormatValue(data, "R_CD")},{FormatValue(data, "R_CD_Result")}");
                            sw.WriteLine($"우측 상하,{FormatValue(data, "R_UD")},{FormatValue(data, "R_UD_Result")}");
                            sw.WriteLine($"우측 좌우,{FormatValue(data, "R_LR")},{FormatValue(data, "R_LR_Result")}");
                            sw.WriteLine($"종합 결과,{FormatValue(data, "HeadLight_Total_Result")}");
                            sw.WriteLine();
                        }

                        // 4. 브레이크 데이터
                        if (data.ContainsKey("Left_Brake"))
                        {
                            sw.WriteLine("[브레이크 데이터]");
                            sw.WriteLine($"측정일시,{FormatValue(data, "Brake_Mea_Date")}");
                            sw.WriteLine($"좌측 제동력,{FormatValue(data, "Left_Brake")}");
                            sw.WriteLine($"우측 제동력,{FormatValue(data, "Right_Brake")}");
                            sw.WriteLine($"종합 결과,{FormatValue(data, "Brake_Total_Result")}");
                            sw.WriteLine();
                        }

                        // 5. 속도계 데이터
                        if (data.ContainsKey("Measured_Speed"))
                        {
                            sw.WriteLine("[속도계 데이터]");
                            sw.WriteLine($"측정일시,{FormatValue(data, "Speed_Mea_Date")}");
                            sw.WriteLine($"측정 속도,{FormatValue(data, "Measured_Speed")}");
                            sw.WriteLine($"실제 속도,{FormatValue(data, "Actual_Speed")}");
                            sw.WriteLine($"측정 결과,{FormatValue(data, "Speed_Result")}");
                            sw.WriteLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CSV 저장 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FormatValue(Dictionary<string, object> data, string key)
        {
            if (data.TryGetValue(key, out object value))
            {
                if (value is DateTime dt)
                {
                    return dt.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (value is long || value is int || value is double || value is decimal)
                {
                    return $"=\"{value}\"";
                }
                return value?.ToString() ?? "";
            }

            return "";
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime? searchDate = null;
                string searchText = null;

                if (txt_searchDate.Checked)
                {
                    searchDate = txt_searchDate.Value.Date;
                }

                if (!string.IsNullOrWhiteSpace(txt_searchVin.Text))
                {
                    searchText = txt_searchVin.Text.Trim();
                }

                if (!searchDate.HasValue && string.IsNullOrEmpty(searchText))
                {
                    LoadAllAcceptNumbers();
                    return;
                }

                DataTable searchResults = dbManager.SearchAcceptNumbers(searchDate, searchText);

                DisplayAcceptNumbersInGrid(searchResults);

                if (searchResults.Rows.Count == 0)
                {
                    MessageBox.Show("검색 결과가 없습니다.", "검색 결과", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"검색 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex <0 || e.ColumnIndex < 0)
            {
                return;
            }

            try
            {
                string selectedAcceptNo = dataGridView1.Rows[e.RowIndex].Cells["Accept_No"].Value.ToString();

                var detailData = dbManager.GetAllDataByAcceptNo(selectedAcceptNo);

                lbl_numberResult.Text = selectedAcceptNo;

                // 1. 경사각도 데이터
                if (detailData.ContainsKey("Inc_Angle"))
                {
                    lbl_incAngle.Text = detailData["Inc_Angle"].ToString();
                }
                else
                {
                    lbl_incAngle.Text = "";
                }

                // 2. 중량계 데이터
                if (detailData.ContainsKey("F_LW"))
                {
                    lbl_flw.Text = detailData["F_LW"].ToString();
                    lbl_frw.Text = detailData["F_RW"].ToString();
                    lbl_ftw.Text = detailData["F_TW"].ToString();
                    lbl_rlw.Text = detailData["R_LW"].ToString();
                    lbl_rrw.Text = detailData["R_RW"].ToString();
                    lbl_rtw.Text = detailData["R_TW"].ToString();
                }
                else
                {
                    lbl_flw.Text = "";
                    lbl_frw.Text = "";
                    lbl_ftw.Text = "";
                    lbl_rlw.Text = "";
                    lbl_rrw.Text = "";
                    lbl_rtw.Text = "";
                }

                // 3. 헤드라이트 데이터 
                ClearHeadlightData();

                if (detailData.ContainsKey("Beam_Type"))
                {
                    string beamType = detailData["Beam_Type"].ToString();

                    if (beamType == "상향")
                    {
                        // 상향 빔 데이터 설정
                        lbl_upward_lcd.Text = detailData["L_CD"].ToString();
                        SetTextBoxColor(lbl_upward_lcd, detailData["L_CD_Result"].ToString());

                        lbl_upward_lud.Text = detailData["L_UD"].ToString();
                        SetTextBoxColor(lbl_upward_lud, detailData["L_UD_Result"].ToString());

                        lbl_upward_llr.Text = detailData["L_LR"].ToString();
                        SetTextBoxColor(lbl_upward_llr, detailData["L_LR_Result"].ToString());

                        lbl_upward_rcd.Text = detailData["R_CD"].ToString();
                        SetTextBoxColor(lbl_upward_rcd, detailData["R_CD_Result"].ToString());

                        lbl_upward_rud.Text = detailData["R_UD"].ToString();
                        SetTextBoxColor(lbl_upward_rud, detailData["R_UD_Result"].ToString());

                        lbl_upward_rlr.Text = detailData["R_LR"].ToString();
                        SetTextBoxColor(lbl_upward_rlr, detailData["R_LR_Result"].ToString());

                        lbl_upward_totalPan.Text = detailData["HeadLight_Total_Result"].ToString();
                        SetTextBoxColor(lbl_upward_totalPan, detailData["HeadLight_Total_Result"].ToString());
                    }
                    else if (beamType == "하향")
                    {
                        // 하향 빔 데이터 설정
                        lbl_downward_lcd.Text = detailData["L_CD"].ToString();
                        SetTextBoxColor(lbl_downward_lcd, detailData["L_CD_Result"].ToString());

                        lbl_downward_lud.Text = detailData["L_UD"].ToString();
                        SetTextBoxColor(lbl_downward_lud, detailData["L_UD_Result"].ToString());

                        lbl_downward_llr.Text = detailData["L_LR"].ToString();
                        SetTextBoxColor(lbl_downward_llr, detailData["L_LR_Result"].ToString());

                        lbl_downward_rcd.Text = detailData["R_CD"].ToString();
                        SetTextBoxColor(lbl_downward_rcd, detailData["R_CD_Result"].ToString());

                        lbl_downward_rud.Text = detailData["R_UD"].ToString();
                        SetTextBoxColor(lbl_downward_rud, detailData["R_UD_Result"].ToString());

                        lbl_downward_rlr.Text = detailData["R_LR"].ToString();
                        SetTextBoxColor(lbl_downward_rlr, detailData["R_LR_Result"].ToString());

                        lbl_downward_totalPan.Text = detailData["HeadLight_Total_Result"].ToString();
                        SetTextBoxColor(lbl_downward_totalPan, detailData["HeadLight_Total_Result"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터 표시 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearHeadlightData()
        {
            // 상향등 데이터 초기화
            lbl_upward_lcd.Text = "";
            lbl_upward_lud.Text = "";
            lbl_upward_llr.Text = "";
            lbl_upward_rcd.Text = "";
            lbl_upward_rud.Text = "";
            lbl_upward_rlr.Text = "";
            lbl_upward_totalPan.Text = "";

            lbl_upward_lcd.BackColor = SystemColors.Window;
            lbl_upward_lud.BackColor = SystemColors.Window;
            lbl_upward_llr.BackColor = SystemColors.Window;
            lbl_upward_rcd.BackColor = SystemColors.Window;
            lbl_upward_rud.BackColor = SystemColors.Window;
            lbl_upward_rlr.BackColor = SystemColors.Window;
            lbl_upward_totalPan.BackColor = SystemColors.Window;

            // 하향등 데이터 초기화
            lbl_downward_lcd.Text = "";
            lbl_downward_lud.Text = "";
            lbl_downward_llr.Text = "";
            lbl_downward_rcd.Text = "";
            lbl_downward_rud.Text = "";
            lbl_downward_rlr.Text = "";
            lbl_downward_totalPan.Text = "";

            lbl_downward_lcd.BackColor = SystemColors.Window;
            lbl_downward_lud.BackColor = SystemColors.Window;
            lbl_downward_llr.BackColor = SystemColors.Window;
            lbl_downward_rcd.BackColor = SystemColors.Window;
            lbl_downward_rud.BackColor = SystemColors.Window;
            lbl_downward_rlr.BackColor = SystemColors.Window;
            lbl_downward_totalPan.BackColor = SystemColors.Window;
        }
    }
}
