using DoosanDataServer.Comm;
using DoosanDataServer.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public Form1()
        {
            InitializeComponent();

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            //barcodeReader = new SerialBarcodeReader("COM7");
            tcpServer = new TcpServer("192.168.10.98", 5004);

            tcpServer.InspectionResultReceived += TcpServer_InspectionResultReceived;
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                //barcodeReader.Open();
                tcpServer.Start();

                for (int i = 0; i < 30; i++)
                {
                    dataGridView1.Rows.Add();
                }
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

            if (e.EquipmentType == EquipmentType.HLT)
            {
                if (e.Result.Length >= 20)
                {
                    currentChassisNumber = e.Result[2];
                    lbl_numberResult.Text = e.Result[2];
                }

                ProcessHeadlightData(e.Result);
            }
            else if (e.EquipmentType == EquipmentType.WGT)
            {
                if (e.Result.Length >= 9)
                {
                    currentChassisNumber = e.Result[2];
                    lbl_numberResult.Text = e.Result[2];
                }

                ProcessWgtData(e.Result);
            }
            else if (e.EquipmentType == EquipmentType.ANG)
            {
                if (e.Result.Length >= 5)
                {
                    currentChassisNumber = e.Result[2];
                    lbl_numberResult.Text = e.Result[2];
                }

                ProcessAngData(e.Result);
            }
        }

        private void ProcessHeadlightData(string[] parts)
        {
            if (parts.Length < 20)
            {
                MessageBox.Show("헤드라이트 데이터 형식이 올바르지 않습니다.", "데이터 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (parts[4] == "상향")
                {
                    txt_upward_lcd.Text = parts[5];
                    SetTextBoxColor(txt_upward_lcd, parts[6]);

                    txt_upward_lud.Text = parts[7];
                    SetTextBoxColor(txt_upward_lud, parts[8]);

                    txt_upward_llr.Text = parts[9];
                    SetTextBoxColor(txt_upward_llr, parts[10]);

                    txt_upward_rcd.Text = parts[11];
                    SetTextBoxColor(txt_upward_rcd, parts[12]);

                    txt_upward_rud.Text = parts[13];
                    SetTextBoxColor(txt_upward_rud, parts[14]);

                    txt_upward_rlr.Text = parts[15];
                    SetTextBoxColor(txt_upward_rlr, parts[16]);

                    string upBeamResult = parts[17];
                    txt_upward_totalPan.Text = upBeamResult;
                    SetTextBoxColor(txt_upward_totalPan, upBeamResult);
                }
                else
                {
                    txt_downward_lcd.Text = parts[5];
                    SetTextBoxColor(txt_downward_lcd, parts[6]);

                    txt_downward_lud.Text = parts[7];
                    SetTextBoxColor(txt_downward_lud, parts[8]);

                    txt_downward_llr.Text = parts[9];
                    SetTextBoxColor(txt_downward_llr, parts[10]);

                    txt_downward_rcd.Text = parts[11];
                    SetTextBoxColor(txt_downward_rcd, parts[12]);

                    txt_downward_rud.Text = parts[13];
                    SetTextBoxColor(txt_downward_rud, parts[14]);

                    txt_downward_rlr.Text = parts[15];
                    SetTextBoxColor(txt_downward_rlr, parts[16]);

                    string lowBeamResult = parts[17];
                    txt_downward_totalPan.Text = lowBeamResult;
                    SetTextBoxColor(txt_downward_totalPan, lowBeamResult);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"헤드라이트 데이터 처리 중 오류 : {ex.Message}", "처리 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessWgtData(string[] parts)
        {
            if (parts.Length < 9)
            {
                MessageBox.Show("중량계 데이터 형식이 올바르지 않습니다.", "데이터 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                txt_flw.Text = parts[3];
                txt_frw.Text = parts[4];
                txt_ftw.Text = parts[5];
                txt_rlw.Text = parts[6];
                txt_rrw.Text = parts[7];
                txt_rtw.Text = parts[8];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"중량계 데이터 처리 중 오류 : {ex.Message}", "처리 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ProcessAngData(string[] parts)
        {
            if (parts.Length < 5)
            {
                MessageBox.Show("경사각도 데이터 형식이 올바르지 않습니다.", "데이터 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                txt_incAngle.Text = parts[3];
            }
            catch (Exception ex)
            {
                MessageBox.Show($"경사각도 데이터 형식이 올바르지 않습니다. 오류: {ex.Message}", "데이터 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }

        private void SetTextBoxColor(TextBox textBox, string result)
        {
            if (result.Trim().ToUpper() == "OK")
            {
                textBox.BackColor = Color.LightGreen;
                textBox.ForeColor = Color.Black;
            }
            else if (result.Trim().ToUpper() == "NG")
            {
                textBox.BackColor = Color.Red;
                textBox.ForeColor = Color.White;
            }
            else
            {
                textBox.BackColor = SystemColors.Window;
                textBox.ForeColor = SystemColors.WindowText;
            }
        }

        private void btn_connectEquipment_Click(object sender, EventArgs e)
        {
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
                    tcpServer.ConnectToAllEquipment(equipmentAddresses);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"설비 연결 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }
    }
}
