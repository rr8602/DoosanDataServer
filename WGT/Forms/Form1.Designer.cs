namespace WGT
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_processTitle = new System.Windows.Forms.Label();
            this.lbl_processMessage = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_frontLeft = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbl_rearLeft = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lbl_frontCenter = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.lbl_rearCenter = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.lbl_frontRight = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.lbl_rearRight = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel11 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.panel12 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.panel13 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.btn_setAllZero = new System.Windows.Forms.Button();
            this.btn_exit = new System.Windows.Forms.Button();
            this.btn_manualInput = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_acceptNo = new System.Windows.Forms.TextBox();
            this.btn_config = new System.Windows.Forms.Button();
            this.btn_selectVehicle = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.lbl_currentVehicle = new System.Windows.Forms.Label();
            this.btn_inspectionStart = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel9.SuspendLayout();
            this.panel11.SuspendLayout();
            this.panel12.SuspendLayout();
            this.panel13.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbl_processTitle);
            this.panel1.Controls.Add(this.lbl_processMessage);
            this.panel1.Location = new System.Drawing.Point(38, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1814, 243);
            this.panel1.TabIndex = 0;
            // 
            // lbl_processTitle
            // 
            this.lbl_processTitle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_processTitle.Location = new System.Drawing.Point(3, 0);
            this.lbl_processTitle.Name = "lbl_processTitle";
            this.lbl_processTitle.Size = new System.Drawing.Size(1808, 22);
            this.lbl_processTitle.TabIndex = 12;
            this.lbl_processTitle.Text = "Total Sum Load";
            this.lbl_processTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_processMessage
            // 
            this.lbl_processMessage.BackColor = System.Drawing.Color.Black;
            this.lbl_processMessage.Font = new System.Drawing.Font("굴림", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_processMessage.ForeColor = System.Drawing.Color.Lime;
            this.lbl_processMessage.Location = new System.Drawing.Point(0, 22);
            this.lbl_processMessage.Name = "lbl_processMessage";
            this.lbl_processMessage.Size = new System.Drawing.Size(1814, 221);
            this.lbl_processMessage.TabIndex = 0;
            this.lbl_processMessage.Text = "초기화중";
            this.lbl_processMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbl_frontLeft);
            this.panel2.Location = new System.Drawing.Point(227, 554);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(512, 164);
            this.panel2.TabIndex = 1;
            // 
            // lbl_frontLeft
            // 
            this.lbl_frontLeft.BackColor = System.Drawing.Color.Black;
            this.lbl_frontLeft.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_frontLeft.ForeColor = System.Drawing.Color.Yellow;
            this.lbl_frontLeft.Location = new System.Drawing.Point(0, 20);
            this.lbl_frontLeft.Name = "lbl_frontLeft";
            this.lbl_frontLeft.Size = new System.Drawing.Size(512, 144);
            this.lbl_frontLeft.TabIndex = 0;
            this.lbl_frontLeft.Text = "0.0";
            this.lbl_frontLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.lbl_rearLeft);
            this.panel3.Location = new System.Drawing.Point(228, 761);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(511, 170);
            this.panel3.TabIndex = 2;
            // 
            // lbl_rearLeft
            // 
            this.lbl_rearLeft.BackColor = System.Drawing.Color.Black;
            this.lbl_rearLeft.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_rearLeft.ForeColor = System.Drawing.Color.Yellow;
            this.lbl_rearLeft.Location = new System.Drawing.Point(-1, 20);
            this.lbl_rearLeft.Name = "lbl_rearLeft";
            this.lbl_rearLeft.Size = new System.Drawing.Size(512, 150);
            this.lbl_rearLeft.TabIndex = 3;
            this.lbl_rearLeft.Text = "0.0";
            this.lbl_rearLeft.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.lbl_frontCenter);
            this.panel4.Location = new System.Drawing.Point(771, 554);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(524, 164);
            this.panel4.TabIndex = 2;
            // 
            // lbl_frontCenter
            // 
            this.lbl_frontCenter.BackColor = System.Drawing.Color.Black;
            this.lbl_frontCenter.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_frontCenter.ForeColor = System.Drawing.Color.Red;
            this.lbl_frontCenter.Location = new System.Drawing.Point(-1, 20);
            this.lbl_frontCenter.Name = "lbl_frontCenter";
            this.lbl_frontCenter.Size = new System.Drawing.Size(522, 144);
            this.lbl_frontCenter.TabIndex = 1;
            this.lbl_frontCenter.Text = "0.0";
            this.lbl_frontCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.lbl_rearCenter);
            this.panel5.Location = new System.Drawing.Point(770, 761);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(522, 170);
            this.panel5.TabIndex = 2;
            // 
            // lbl_rearCenter
            // 
            this.lbl_rearCenter.BackColor = System.Drawing.Color.Black;
            this.lbl_rearCenter.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_rearCenter.ForeColor = System.Drawing.Color.Red;
            this.lbl_rearCenter.Location = new System.Drawing.Point(-1, 16);
            this.lbl_rearCenter.Name = "lbl_rearCenter";
            this.lbl_rearCenter.Size = new System.Drawing.Size(520, 154);
            this.lbl_rearCenter.TabIndex = 4;
            this.lbl_rearCenter.Text = "0.0";
            this.lbl_rearCenter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.panel10);
            this.panel6.Controls.Add(this.lbl_frontRight);
            this.panel6.Location = new System.Drawing.Point(1329, 554);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(523, 164);
            this.panel6.TabIndex = 2;
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Silver;
            this.panel10.Controls.Add(this.label13);
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(523, 42);
            this.panel10.TabIndex = 9;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.White;
            this.label13.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label13.Location = new System.Drawing.Point(-2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(196, 42);
            this.label13.TabIndex = 2;
            this.label13.Text = "Right Front Load";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_frontRight
            // 
            this.lbl_frontRight.BackColor = System.Drawing.Color.Black;
            this.lbl_frontRight.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_frontRight.ForeColor = System.Drawing.Color.Yellow;
            this.lbl_frontRight.Location = new System.Drawing.Point(-3, 23);
            this.lbl_frontRight.Name = "lbl_frontRight";
            this.lbl_frontRight.Size = new System.Drawing.Size(526, 141);
            this.lbl_frontRight.TabIndex = 2;
            this.lbl_frontRight.Text = "0.0";
            this.lbl_frontRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.lbl_rearRight);
            this.panel7.Location = new System.Drawing.Point(1329, 761);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(523, 170);
            this.panel7.TabIndex = 2;
            // 
            // lbl_rearRight
            // 
            this.lbl_rearRight.BackColor = System.Drawing.Color.Black;
            this.lbl_rearRight.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_rearRight.ForeColor = System.Drawing.Color.Yellow;
            this.lbl_rearRight.Location = new System.Drawing.Point(0, 42);
            this.lbl_rearRight.Name = "lbl_rearRight";
            this.lbl_rearRight.Size = new System.Drawing.Size(523, 128);
            this.lbl_rearRight.TabIndex = 5;
            this.lbl_rearRight.Text = "0.0";
            this.lbl_rearRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Gray;
            this.label7.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label7.Location = new System.Drawing.Point(227, 386);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(517, 135);
            this.label7.TabIndex = 3;
            this.label7.Text = "좌";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Gray;
            this.label8.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label8.Location = new System.Drawing.Point(771, 386);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(521, 135);
            this.label8.TabIndex = 4;
            this.label8.Text = "합계";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.Gray;
            this.label9.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label9.Location = new System.Drawing.Point(1327, 386);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(525, 135);
            this.label9.TabIndex = 5;
            this.label9.Text = "우";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.Silver;
            this.panel8.Controls.Add(this.label1);
            this.panel8.Location = new System.Drawing.Point(227, 554);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(512, 42);
            this.panel8.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(174, 42);
            this.label1.TabIndex = 0;
            this.label1.Text = "Left Fromt Load";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.Silver;
            this.panel9.Controls.Add(this.label4);
            this.panel9.Location = new System.Drawing.Point(770, 554);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(522, 42);
            this.panel9.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.White;
            this.label4.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(-3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(528, 42);
            this.label4.TabIndex = 1;
            this.label4.Text = "Front Sum Load";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.Color.Silver;
            this.panel11.Controls.Add(this.label16);
            this.panel11.Location = new System.Drawing.Point(1329, 761);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(523, 42);
            this.panel11.TabIndex = 9;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.White;
            this.label16.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label16.Location = new System.Drawing.Point(-1, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(195, 42);
            this.label16.TabIndex = 5;
            this.label16.Text = "Right Rear Load";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel12
            // 
            this.panel12.BackColor = System.Drawing.Color.Silver;
            this.panel12.Controls.Add(this.label15);
            this.panel12.Location = new System.Drawing.Point(770, 761);
            this.panel12.Name = "panel12";
            this.panel12.Size = new System.Drawing.Size(519, 42);
            this.panel12.TabIndex = 9;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.White;
            this.label15.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label15.Location = new System.Drawing.Point(-3, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(542, 42);
            this.label15.TabIndex = 4;
            this.label15.Text = "Rear Sum Load";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel13
            // 
            this.panel13.BackColor = System.Drawing.Color.Silver;
            this.panel13.Controls.Add(this.label14);
            this.panel13.Location = new System.Drawing.Point(227, 761);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(512, 42);
            this.panel13.TabIndex = 9;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.White;
            this.label14.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label14.Location = new System.Drawing.Point(-2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(176, 42);
            this.label14.TabIndex = 3;
            this.label14.Text = "Left Rear Load";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_setAllZero
            // 
            this.btn_setAllZero.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btn_setAllZero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_setAllZero.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_setAllZero.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_setAllZero.Location = new System.Drawing.Point(227, 945);
            this.btn_setAllZero.Name = "btn_setAllZero";
            this.btn_setAllZero.Size = new System.Drawing.Size(512, 79);
            this.btn_setAllZero.TabIndex = 10;
            this.btn_setAllZero.Text = "Zero";
            this.btn_setAllZero.UseVisualStyleBackColor = false;
            this.btn_setAllZero.Click += new System.EventHandler(this.btn_setAllZero_Click);
            // 
            // btn_exit
            // 
            this.btn_exit.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btn_exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_exit.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_exit.Location = new System.Drawing.Point(1633, 937);
            this.btn_exit.Name = "btn_exit";
            this.btn_exit.Size = new System.Drawing.Size(219, 87);
            this.btn_exit.TabIndex = 6;
            this.btn_exit.Text = "닫기";
            this.btn_exit.UseVisualStyleBackColor = false;
            this.btn_exit.Click += new System.EventHandler(this.btn_exit_Click);
            // 
            // btn_manualInput
            // 
            this.btn_manualInput.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btn_manualInput.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_manualInput.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_manualInput.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_manualInput.Location = new System.Drawing.Point(38, 386);
            this.btn_manualInput.Name = "btn_manualInput";
            this.btn_manualInput.Size = new System.Drawing.Size(157, 135);
            this.btn_manualInput.TabIndex = 11;
            this.btn_manualInput.Text = "수기입력";
            this.btn_manualInput.UseVisualStyleBackColor = false;
            this.btn_manualInput.Click += new System.EventHandler(this.btn_manualInput_Click);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Black;
            this.label10.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label10.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label10.Location = new System.Drawing.Point(37, 554);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(158, 164);
            this.label10.TabIndex = 6;
            this.label10.Text = "전륜";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Black;
            this.label11.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label11.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.label11.Location = new System.Drawing.Point(38, 761);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(157, 170);
            this.label11.TabIndex = 7;
            this.label11.Text = "후륜";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_acceptNo
            // 
            this.txt_acceptNo.Location = new System.Drawing.Point(1611, 336);
            this.txt_acceptNo.Name = "txt_acceptNo";
            this.txt_acceptNo.Size = new System.Drawing.Size(238, 25);
            this.txt_acceptNo.TabIndex = 13;
            // 
            // btn_config
            // 
            this.btn_config.BackColor = System.Drawing.Color.Silver;
            this.btn_config.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_config.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_config.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_config.Location = new System.Drawing.Point(38, 945);
            this.btn_config.Name = "btn_config";
            this.btn_config.Size = new System.Drawing.Size(157, 79);
            this.btn_config.TabIndex = 21;
            this.btn_config.Text = "설정";
            this.btn_config.UseVisualStyleBackColor = false;
            this.btn_config.Click += new System.EventHandler(this.btn_config_Click);
            // 
            // btn_selectVehicle
            // 
            this.btn_selectVehicle.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btn_selectVehicle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_selectVehicle.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_selectVehicle.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btn_selectVehicle.Location = new System.Drawing.Point(770, 945);
            this.btn_selectVehicle.Name = "btn_selectVehicle";
            this.btn_selectVehicle.Size = new System.Drawing.Size(522, 79);
            this.btn_selectVehicle.TabIndex = 22;
            this.btn_selectVehicle.Text = "차량 선택";
            this.btn_selectVehicle.UseVisualStyleBackColor = false;
            this.btn_selectVehicle.Click += new System.EventHandler(this.btn_selectVehicle_Click);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("굴림", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(31, 320);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(312, 41);
            this.label3.TabIndex = 24;
            this.label3.Text = "현재 검사 차량 : ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_currentVehicle
            // 
            this.lbl_currentVehicle.Font = new System.Drawing.Font("굴림", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_currentVehicle.ForeColor = System.Drawing.Color.Red;
            this.lbl_currentVehicle.Location = new System.Drawing.Point(349, 320);
            this.lbl_currentVehicle.Name = "lbl_currentVehicle";
            this.lbl_currentVehicle.Size = new System.Drawing.Size(298, 41);
            this.lbl_currentVehicle.TabIndex = 25;
            this.lbl_currentVehicle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_inspectionStart
            // 
            this.btn_inspectionStart.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_inspectionStart.Location = new System.Drawing.Point(664, 320);
            this.btn_inspectionStart.Name = "btn_inspectionStart";
            this.btn_inspectionStart.Size = new System.Drawing.Size(177, 41);
            this.btn_inspectionStart.TabIndex = 26;
            this.btn_inspectionStart.Text = "검사 시작";
            this.btn_inspectionStart.UseVisualStyleBackColor = true;
            this.btn_inspectionStart.Click += new System.EventHandler(this.btn_inspectionStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1905, 1053);
            this.Controls.Add(this.btn_inspectionStart);
            this.Controls.Add(this.lbl_currentVehicle);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_selectVehicle);
            this.Controls.Add(this.btn_config);
            this.Controls.Add(this.txt_acceptNo);
            this.Controls.Add(this.btn_manualInput);
            this.Controls.Add(this.btn_exit);
            this.Controls.Add(this.btn_setAllZero);
            this.Controls.Add(this.panel13);
            this.Controls.Add(this.panel12);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel8);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel7);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel11.ResumeLayout(false);
            this.panel12.ResumeLayout(false);
            this.panel13.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label lbl_frontLeft;
        private System.Windows.Forms.Label lbl_frontCenter;
        private System.Windows.Forms.Label lbl_frontRight;
        private System.Windows.Forms.Label lbl_rearLeft;
        private System.Windows.Forms.Label lbl_rearCenter;
        private System.Windows.Forms.Label lbl_rearRight;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbl_processMessage;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.Panel panel12;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btn_setAllZero;
        private System.Windows.Forms.Button btn_exit;
        private System.Windows.Forms.Button btn_manualInput;
        private System.Windows.Forms.Label lbl_processTitle;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txt_acceptNo;
        private System.Windows.Forms.Button btn_config;
        private System.Windows.Forms.Button btn_selectVehicle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbl_currentVehicle;
        private System.Windows.Forms.Button btn_inspectionStart;
    }
}

