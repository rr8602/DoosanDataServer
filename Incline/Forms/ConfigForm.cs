using CommonIniFile;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Incline.Forms
{
    public partial class ConfigForm : Form
    {
        private Form1 mainForm;

        public ConfigForm()
        {
            InitializeComponent();
            CreateAndLoadIniSettings();

            this.AutoScaleMode = AutoScaleMode.None;

            LoadComPorts();
        }

        public ConfigForm(Form1 mainForm) : this()
        {
            this.mainForm = mainForm;
        }

        private void CreateAndLoadIniSettings()
        {
            if (!System.IO.File.Exists("Config\\Incline.ini"))
            {
                CreateDefaultIni.CreateInclineIniFile(txt_serverIp.Text, txt_serverPort.Text);
            }
            else
            {
                IniFile iniFile = new IniFile("Config\\Incline.ini");

                txt_serverIp.Text = iniFile.ReadString("Network", "IP", txt_serverIp.Text);
                txt_serverPort.Text = iniFile.ReadString("Network", "Port", txt_serverPort.Text);
                txt_maxInclineAngle.Text = iniFile.ReadString("Network", "MaxAngle", txt_maxInclineAngle.Text);
                txt_minInclineAngle.Text = iniFile.ReadString("Network", "MinAngle", txt_minInclineAngle.Text);
            }
        }

        private void LoadComPorts()
        {
            cmb_ports.Items.Clear();

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            cmb_ports.Items.AddRange(ports);

            if (ports.Length > 0)
            {
                cmb_ports.SelectedIndex = 0;
            }
        }

        private void SaveIniSettings()
        {
            CreateDefaultIni.CreateInclineIniFile(txt_serverIp.Text, txt_serverPort.Text);
            MessageBox.Show("설정이 저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            SaveIniSettings();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_refreshPorts_Click(object sender, EventArgs e)
        {
            LoadComPorts();

            if (cmb_ports.Items.Count == 0)
                MessageBox.Show("사용 가능한 COM 포트가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (cmb_ports.SelectedItem == null)
            {
                MessageBox.Show("COM 포트를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string portName = cmb_ports.SelectedItem.ToString();

            if (mainForm != null)
            {
                bool connected = mainForm.ConnectIOBoard(portName);
            }
            else
            {
                MessageBox.Show("선택한 COM 포트에 연결된 장비가 없습니다. 확인 후 다시 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            LoadComPorts();
        }
    }
}
