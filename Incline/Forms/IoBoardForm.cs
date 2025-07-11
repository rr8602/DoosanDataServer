using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Incline.Forms
{
    public partial class IoBoardForm : Form
    {
        private Form1 form;
        private bool isOnOff = true;

        private const string LampNonePath = "..\\..\\Lamp-None.bmp";
        private const string ButtonNonePath = "..\\..\\Resources\\Button-None.png";
        private const string ButtonGreenPath = "..\\..\\Resources\\Button-Green1.png";

        private static Image NoneLampImg = Image.FromFile(LampNonePath);
        private static Image GreenButtonImg = Image.FromFile(ButtonGreenPath);
        private static Image NoneButtonImg = Image.FromFile(ButtonNonePath);

        private static Image ProcessedGreenImg = ChangeMagentaToLightYellow(GreenButtonImg);
        private static Image ProcessedNoneImg = ChangeMagentaToLightYellow(NoneButtonImg);

        public IoBoardForm(Form1 parentForm)
        {
            InitializeComponent();
            this.form = parentForm;
        }

        private static Image ChangeMagentaToLightYellow(Image srcImg)
        {
            Bitmap bmp = new Bitmap(srcImg);
            Color target = Color.FromArgb(255, 0, 255);
            Color replace = Color.LightYellow;

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    if (bmp.GetPixel(x, y).ToArgb() == target.ToArgb())
                        bmp.SetPixel(x, y, replace);
                }
            }
            return bmp;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i <= 7; i++)
                {
                    PictureBox pictureLampBox = (PictureBox)panel1.Controls.Find($"pic_input{i}", true)[0];
                    pictureLampBox.Image = ChangeMagentaToLightYellow(NoneLampImg);

                    PictureBox pictureButtonBox = (PictureBox)panel2.Controls.Find($"pic_output{i}", true)[0];
                    pictureButtonBox.Image = ChangeMagentaToLightYellow(NoneButtonImg);
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"해당 파일 없음: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"에러 발생: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pic_input0_Click(object sender, EventArgs e)
        {
            try
            {
                if (isOnOff)
                {
                    pic_output0.Image = ProcessedGreenImg;
                    pic_output1.Image = ProcessedGreenImg;
                    pic_output3.Image = ProcessedGreenImg;

                    form.LiftUpOnSignal(true);
                    isOnOff = false;
                }
                else
                {
                    pic_output0.Image = ProcessedNoneImg;
                    pic_output1.Image = ProcessedNoneImg;
                    pic_output3.Image = ProcessedNoneImg;

                    form.LiftOffSignal();
                    isOnOff = true;
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"해당 파일 없음: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"에러 발생: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
            
        private void pic_input1_Click(object sender, EventArgs e)
        {
            try
            {
                if (isOnOff)
                {
                    pic_output0.Image = ProcessedGreenImg;
                    pic_output2.Image = ProcessedGreenImg;
                    pic_output3.Image = ProcessedGreenImg;

                    form.LiftDownOnSignal(true);
                    isOnOff = false;
                }
                else
                {
                    pic_output0.Image = ProcessedNoneImg;
                    pic_output2.Image = ProcessedNoneImg;
                    pic_output3.Image = ProcessedNoneImg;

                    form.LiftOffSignal();
                    isOnOff = true;
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show($"해당 파일 없음: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"에러 발생: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
