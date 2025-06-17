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
    public partial class ManualInputForm : Form
    {
        public double FrontLeft { get; private set; }
        public double FrontRight { get; private set; }
        public double RearLeft { get; private set; }
        public double RearRight { get; private set; }
        public bool IsConfirmed { get; private set; }

        public ManualInputForm()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        private void btn_confirm_Click(object sender, EventArgs e)
        {
            try
            {
                FrontLeft = double.Parse(txt_frontLeft.Text);
                FrontRight = double.Parse(txt_frontRight.Text);
                RearLeft = double.Parse(txt_rearLeft.Text);
                RearRight = double.Parse(txt_rearRight.Text);
                IsConfirmed = true;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("유효한 숫자를 입력하세요.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
