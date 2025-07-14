namespace Incline
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
            if (disposing)
            {
                if (backgroundImage != null)
                {
                    backgroundImage.Dispose();
                    backgroundImage = null;
                }
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbl_message = new System.Windows.Forms.Label();
            this.lbl_arcGaugeValue = new System.Windows.Forms.Label();
            this.lbl_incAngle = new System.Windows.Forms.Label();
            this.panel_arcGauge = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_okNg = new System.Windows.Forms.Label();
            this.btn_liftUp = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_config = new System.Windows.Forms.Button();
            this.btn_selectVehicle = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_currentVehicle = new System.Windows.Forms.Label();
            this.btn_inspectionStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_liftDown = new System.Windows.Forms.Button();
            this.btn_io = new System.Windows.Forms.Button();
            this.lbl_ioBoardComm = new System.Windows.Forms.Label();
            this.lbl_sensorComm = new System.Windows.Forms.Label();
            this.btn_allPause = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_message
            // 
            this.lbl_message.BackColor = System.Drawing.Color.Black;
            this.lbl_message.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_message.ForeColor = System.Drawing.Color.Lime;
            this.lbl_message.Location = new System.Drawing.Point(1186, 109);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(701, 123);
            this.lbl_message.TabIndex = 0;
            this.lbl_message.Text = "label1";
            this.lbl_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_arcGaugeValue
            // 
            this.lbl_arcGaugeValue.BackColor = System.Drawing.Color.White;
            this.lbl_arcGaugeValue.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_arcGaugeValue.ForeColor = System.Drawing.Color.Black;
            this.lbl_arcGaugeValue.Location = new System.Drawing.Point(1692, 242);
            this.lbl_arcGaugeValue.Name = "lbl_arcGaugeValue";
            this.lbl_arcGaugeValue.Size = new System.Drawing.Size(111, 51);
            this.lbl_arcGaugeValue.TabIndex = 1;
            this.lbl_arcGaugeValue.Text = "label2";
            this.lbl_arcGaugeValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_incAngle
            // 
            this.lbl_incAngle.BackColor = System.Drawing.Color.Black;
            this.lbl_incAngle.Font = new System.Drawing.Font("굴림", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_incAngle.ForeColor = System.Drawing.Color.Red;
            this.lbl_incAngle.Location = new System.Drawing.Point(1185, 306);
            this.lbl_incAngle.Name = "lbl_incAngle";
            this.lbl_incAngle.Size = new System.Drawing.Size(699, 368);
            this.lbl_incAngle.TabIndex = 2;
            this.lbl_incAngle.Text = "label3";
            this.lbl_incAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_arcGauge
            // 
            this.panel_arcGauge.Location = new System.Drawing.Point(40, 199);
            this.panel_arcGauge.Name = "panel_arcGauge";
            this.panel_arcGauge.Size = new System.Drawing.Size(1139, 684);
            this.panel_arcGauge.TabIndex = 3;
            this.panel_arcGauge.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_arcGauge_Paint);
            this.panel_arcGauge.Resize += new System.EventHandler(this.panel_arcGauge_Resize);
            // 
            // lbl_title
            // 
            this.lbl_title.BackColor = System.Drawing.Color.Silver;
            this.lbl_title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_title.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_title.Location = new System.Drawing.Point(1187, 80);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(700, 29);
            this.lbl_title.TabIndex = 4;
            this.lbl_title.Text = "Message";
            this.lbl_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(1186, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(491, 51);
            this.label2.TabIndex = 5;
            this.label2.Text = "degree";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_okNg
            // 
            this.lbl_okNg.BackColor = System.Drawing.Color.Black;
            this.lbl_okNg.Location = new System.Drawing.Point(1819, 242);
            this.lbl_okNg.Name = "lbl_okNg";
            this.lbl_okNg.Size = new System.Drawing.Size(65, 51);
            this.lbl_okNg.TabIndex = 6;
            // 
            // btn_liftUp
            // 
            this.btn_liftUp.BackColor = System.Drawing.Color.SkyBlue;
            this.btn_liftUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_liftUp.Font = new System.Drawing.Font("굴림", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_liftUp.Location = new System.Drawing.Point(40, 903);
            this.btn_liftUp.Name = "btn_liftUp";
            this.btn_liftUp.Size = new System.Drawing.Size(240, 93);
            this.btn_liftUp.TabIndex = 8;
            this.btn_liftUp.Text = "리프트 상승";
            this.btn_liftUp.UseVisualStyleBackColor = false;
            this.btn_liftUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_liftUp_MouseDown);
            this.btn_liftUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_liftUp_MouseUp);
            // 
            // btn_save
            // 
            this.btn_save.BackColor = System.Drawing.Color.SkyBlue;
            this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_save.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_save.Location = new System.Drawing.Point(1185, 904);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(343, 93);
            this.btn_save.TabIndex = 10;
            this.btn_save.Text = "저   장";
            this.btn_save.UseVisualStyleBackColor = false;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.Color.SkyBlue;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_close.Location = new System.Drawing.Point(1546, 903);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(343, 93);
            this.btn_close.TabIndex = 12;
            this.btn_close.Text = "닫  기";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_config
            // 
            this.btn_config.BackColor = System.Drawing.Color.Silver;
            this.btn_config.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_config.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_config.Location = new System.Drawing.Point(1546, 691);
            this.btn_config.Name = "btn_config";
            this.btn_config.Size = new System.Drawing.Size(341, 93);
            this.btn_config.TabIndex = 21;
            this.btn_config.Text = "설  정";
            this.btn_config.UseVisualStyleBackColor = false;
            this.btn_config.Click += new System.EventHandler(this.btn_config_Click);
            // 
            // btn_selectVehicle
            // 
            this.btn_selectVehicle.BackColor = System.Drawing.Color.Silver;
            this.btn_selectVehicle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_selectVehicle.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_selectVehicle.Location = new System.Drawing.Point(1546, 790);
            this.btn_selectVehicle.Name = "btn_selectVehicle";
            this.btn_selectVehicle.Size = new System.Drawing.Size(341, 93);
            this.btn_selectVehicle.TabIndex = 22;
            this.btn_selectVehicle.Text = "차량 선택";
            this.btn_selectVehicle.UseVisualStyleBackColor = false;
            this.btn_selectVehicle.Click += new System.EventHandler(this.btn_selectVehicle_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(30, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(386, 90);
            this.label4.TabIndex = 23;
            this.label4.Text = "현재 검사 차량 : ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_currentVehicle
            // 
            this.lbl_currentVehicle.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_currentVehicle.ForeColor = System.Drawing.Color.Red;
            this.lbl_currentVehicle.Location = new System.Drawing.Point(421, 87);
            this.lbl_currentVehicle.Name = "lbl_currentVehicle";
            this.lbl_currentVehicle.Size = new System.Drawing.Size(758, 90);
            this.lbl_currentVehicle.TabIndex = 24;
            this.lbl_currentVehicle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_inspectionStart
            // 
            this.btn_inspectionStart.BackColor = System.Drawing.Color.Silver;
            this.btn_inspectionStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_inspectionStart.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_inspectionStart.Location = new System.Drawing.Point(940, 903);
            this.btn_inspectionStart.Name = "btn_inspectionStart";
            this.btn_inspectionStart.Size = new System.Drawing.Size(239, 93);
            this.btn_inspectionStart.TabIndex = 27;
            this.btn_inspectionStart.Text = "검사 시작";
            this.btn_inspectionStart.UseVisualStyleBackColor = false;
            this.btn_inspectionStart.Click += new System.EventHandler(this.btn_inspectionStart_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.LightGray;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(30, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1857, 68);
            this.label1.TabIndex = 28;
            this.label1.Text = "경 사 각 도  시 험 기";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_liftDown
            // 
            this.btn_liftDown.BackColor = System.Drawing.Color.Red;
            this.btn_liftDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_liftDown.Font = new System.Drawing.Font("굴림", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_liftDown.Location = new System.Drawing.Point(296, 902);
            this.btn_liftDown.Name = "btn_liftDown";
            this.btn_liftDown.Size = new System.Drawing.Size(240, 93);
            this.btn_liftDown.TabIndex = 30;
            this.btn_liftDown.Text = "리프트 하강";
            this.btn_liftDown.UseVisualStyleBackColor = false;
            this.btn_liftDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btn_liftDown_MouseDown);
            this.btn_liftDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btn_liftDown_MouseUp);
            // 
            // btn_io
            // 
            this.btn_io.BackColor = System.Drawing.Color.Silver;
            this.btn_io.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_io.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_io.Location = new System.Drawing.Point(1186, 691);
            this.btn_io.Name = "btn_io";
            this.btn_io.Size = new System.Drawing.Size(342, 93);
            this.btn_io.TabIndex = 33;
            this.btn_io.Text = "I/O";
            this.btn_io.UseVisualStyleBackColor = false;
            this.btn_io.Click += new System.EventHandler(this.btn_io_Click);
            // 
            // lbl_ioBoardComm
            // 
            this.lbl_ioBoardComm.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_ioBoardComm.Location = new System.Drawing.Point(61, 9);
            this.lbl_ioBoardComm.Name = "lbl_ioBoardComm";
            this.lbl_ioBoardComm.Size = new System.Drawing.Size(170, 68);
            this.lbl_ioBoardComm.TabIndex = 34;
            this.lbl_ioBoardComm.Text = "IoBoard";
            this.lbl_ioBoardComm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_sensorComm
            // 
            this.lbl_sensorComm.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_sensorComm.Location = new System.Drawing.Point(264, 9);
            this.lbl_sensorComm.Name = "lbl_sensorComm";
            this.lbl_sensorComm.Size = new System.Drawing.Size(170, 68);
            this.lbl_sensorComm.TabIndex = 35;
            this.lbl_sensorComm.Text = "Sensor";
            this.lbl_sensorComm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_allPause
            // 
            this.btn_allPause.BackColor = System.Drawing.Color.Silver;
            this.btn_allPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_allPause.Font = new System.Drawing.Font("굴림", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_allPause.Location = new System.Drawing.Point(554, 902);
            this.btn_allPause.Name = "btn_allPause";
            this.btn_allPause.Size = new System.Drawing.Size(240, 93);
            this.btn_allPause.TabIndex = 36;
            this.btn_allPause.Text = "정  지";
            this.btn_allPause.UseVisualStyleBackColor = false;
            this.btn_allPause.Visible = false;
            this.btn_allPause.Click += new System.EventHandler(this.btn_allPause_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1924, 1055);
            this.Controls.Add(this.btn_allPause);
            this.Controls.Add(this.lbl_sensorComm);
            this.Controls.Add(this.lbl_ioBoardComm);
            this.Controls.Add(this.btn_io);
            this.Controls.Add(this.btn_liftDown);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_inspectionStart);
            this.Controls.Add(this.lbl_currentVehicle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_selectVehicle);
            this.Controls.Add(this.btn_config);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_liftUp);
            this.Controls.Add(this.lbl_okNg);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.panel_arcGauge);
            this.Controls.Add(this.lbl_incAngle);
            this.Controls.Add(this.lbl_arcGaugeValue);
            this.Controls.Add(this.lbl_message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "KI&T Incliner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_message;
        private System.Windows.Forms.Label lbl_arcGaugeValue;
        private System.Windows.Forms.Label lbl_incAngle;
        private System.Windows.Forms.Panel panel_arcGauge;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbl_okNg;
        private System.Windows.Forms.Button btn_liftUp;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_config;
        private System.Windows.Forms.Button btn_selectVehicle;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label lbl_currentVehicle;
        private System.Windows.Forms.Button btn_inspectionStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_liftDown;
        private System.Windows.Forms.Button btn_io;
        private System.Windows.Forms.Label lbl_ioBoardComm;
        private System.Windows.Forms.Label lbl_sensorComm;
        private System.Windows.Forms.Button btn_allPause;
    }
}

