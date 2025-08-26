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
        private Incline mainForm;

        public ConfigForm()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
        }

        public ConfigForm(Incline mainForm) : this()
        {
            this.mainForm = mainForm;
        }

        private void CreateAndLoadIniSettings()
        {
            if (!System.IO.File.Exists("Config\\Incline.ini"))
            {
                CreateDefaultIni.CreateInclineIniFile(txt_serverIp.Text, txt_serverPort.Text, txt_maxInclineAngle.Text, txt_minInclineAngle.Text,
                cmb_ioPorts.Text, cmb_sensorPorts.Text);
            }
            else
            {
                IniFile iniFile = new IniFile("Config\\Incline.ini");

                txt_serverIp.Text = iniFile.ReadString("Network", "IP", txt_serverIp.Text);
                txt_serverPort.Text = iniFile.ReadString("Network", "Port", txt_serverPort.Text);
                txt_maxInclineAngle.Text = iniFile.ReadString("Angle", "MaxAngle", txt_maxInclineAngle.Text);
                txt_minInclineAngle.Text = iniFile.ReadString("Angle", "MinAngle", txt_minInclineAngle.Text);

                string savedIoPort = iniFile.ReadString("Network", "IoPort");
                string savedSensorPort = iniFile.ReadString("Network", "SensorPort");

                if (!string.IsNullOrEmpty(savedIoPort) && cmb_ioPorts.Items.Contains(savedIoPort))
                {
                    cmb_ioPorts.SelectedItem = savedIoPort;
                }

                if (!string.IsNullOrEmpty(savedSensorPort) && cmb_sensorPorts.Items.Contains(savedSensorPort))
                {
                    cmb_sensorPorts.SelectedItem = savedSensorPort;
                }
            }
        }

        private void LoadIoComPorts()
        {
            cmb_ioPorts.Items.Clear();

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            cmb_ioPorts.Items.AddRange(ports);

            if (ports.Length > 0)
            {
                cmb_ioPorts.SelectedIndex = 0;
            }
        }

        private void LoadSensorComPorts()
        {
            cmb_sensorPorts.Items.Clear();

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            cmb_sensorPorts.Items.AddRange(ports);

            if (ports.Length > 0)
            {
                cmb_sensorPorts.SelectedIndex = 0;
            }
        }

        private void SaveIniSettings()
        {
            CreateDefaultIni.CreateInclineIniFile(txt_serverIp.Text, txt_serverPort.Text, txt_maxInclineAngle.Text, txt_minInclineAngle.Text,
                cmb_ioPorts.Text, cmb_sensorPorts.Text);
            MessageBox.Show("설정이 저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            SaveIniSettings();

            if (mainForm != null)
            {
                mainForm.ReloadSettingsAndRedrawGauge();
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_refreshPorts_Click(object sender, EventArgs e)
        {
            LoadIoComPorts();

            if (cmb_ioPorts.Items.Count == 0)
                MessageBox.Show("사용 가능한 COM 포트가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_ioConnect_Click(object sender, EventArgs e)
        {
            if (cmb_ioPorts.SelectedItem == null)
            {
                MessageBox.Show("COM 포트를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string portName = cmb_ioPorts.SelectedItem.ToString();

            if (mainForm != null)
            {
                bool connected = mainForm.ConnectIOBoard(portName);
            }
            else
            {
                MessageBox.Show("선택한 COM 포트에 연결된 장비가 없습니다. 확인 후 다시 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_sensorRefreshPorts_Click(object sender, EventArgs e)
        {
            LoadSensorComPorts();

            if (cmb_sensorPorts.Items.Count == 0)
                MessageBox.Show("사용 가능한 COM 포트가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btn_sensorConnect_Click(object sender, EventArgs e)
        {
            if (cmb_sensorPorts.SelectedItem == null)
            {
                MessageBox.Show("COM 포트를 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string portName = cmb_sensorPorts.SelectedItem.ToString();

            if (mainForm != null)
            {
                bool connected = mainForm.ConnectSensor(portName);
            }
            else
            {
                MessageBox.Show("선택한 COM 포트에 연결된 장비가 없습니다. 확인 후 다시 선택하세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            LoadIoComPorts();
            LoadSensorComPorts();
            CreateAndLoadIniSettings();
        }
    }
}
