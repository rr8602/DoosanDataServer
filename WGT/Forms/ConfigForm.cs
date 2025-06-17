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

namespace WGT.Forms
{
    public partial class ConfigForm : Form
    {
        public ConfigForm()
        {
            InitializeComponent();
            CreateAndLoadIniSettings();
        }

        private void CreateAndLoadIniSettings()
        {
            if (!System.IO.File.Exists("Config\\WGT.ini"))
            {
                CreateDefaultIni.CreateWgtIniFile(txt_ip.Text, txt_port.Text, txt_minWeight.Text, txt_runout.Text);
            }
            else
            {
                IniFile iniFile = new IniFile("Config\\WGT.ini");

                txt_ip.Text = iniFile.ReadString("Network", "IP", txt_ip.Text);
                txt_port.Text = iniFile.ReadString("Network", "Port", txt_port.Text);
                txt_minWeight.Text = iniFile.ReadString("Weight", "MinimumWeight", txt_minWeight.Text);
                txt_runout.Text = iniFile.ReadString("Weight", "Runout", txt_runout.Text);
            }
        }

        private void SaveIniSettings()
        {
            CreateDefaultIni.CreateWgtIniFile(txt_ip.Text, txt_port.Text, txt_minWeight.Text, txt_runout.Text);
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
    }
}
