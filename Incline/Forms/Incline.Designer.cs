namespace Incline
{
    partial class Incline
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Incline));
            this.lbl_message = new System.Windows.Forms.Label();
            this.lbl_incAngle = new System.Windows.Forms.Label();
            this.panel_arcGauge = new System.Windows.Forms.Panel();
            this.lbl_currentVehicle = new System.Windows.Forms.Label();
            this.lbl_title = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_config = new System.Windows.Forms.Button();
            this.btn_selectVehicle = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_inspectionStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_io = new System.Windows.Forms.Button();
            this.lbl_ioBoardComm = new System.Windows.Forms.Label();
            this.lbl_sensorComm = new System.Windows.Forms.Label();
            this.btn_inspectionCompelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_message
            // 
            this.lbl_message.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_message.BackColor = System.Drawing.Color.Black;
            this.lbl_message.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_message.ForeColor = System.Drawing.Color.Lime;
            this.lbl_message.Location = new System.Drawing.Point(870, 100);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(642, 298);
            this.lbl_message.TabIndex = 0;
            this.lbl_message.Text = "label1";
            this.lbl_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_incAngle
            // 
            this.lbl_incAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_incAngle.BackColor = System.Drawing.Color.Black;
            this.lbl_incAngle.Font = new System.Drawing.Font("굴림", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_incAngle.ForeColor = System.Drawing.Color.Lime;
            this.lbl_incAngle.Location = new System.Drawing.Point(870, 412);
            this.lbl_incAngle.Name = "lbl_incAngle";
            this.lbl_incAngle.Size = new System.Drawing.Size(642, 347);
            this.lbl_incAngle.TabIndex = 2;
            this.lbl_incAngle.Text = "label3";
            this.lbl_incAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_arcGauge
            // 
            this.panel_arcGauge.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel_arcGauge.Location = new System.Drawing.Point(40, 141);
            this.panel_arcGauge.Name = "panel_arcGauge";
            this.panel_arcGauge.Size = new System.Drawing.Size(816, 543);
            this.panel_arcGauge.TabIndex = 3;
            this.panel_arcGauge.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_arcGauge_Paint);
            this.panel_arcGauge.Resize += new System.EventHandler(this.panel_arcGauge_Resize);
            // 
            // lbl_currentVehicle
            // 
            this.lbl_currentVehicle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_currentVehicle.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_currentVehicle.ForeColor = System.Drawing.Color.Red;
            this.lbl_currentVehicle.Location = new System.Drawing.Point(455, 70);
            this.lbl_currentVehicle.Name = "lbl_currentVehicle";
            this.lbl_currentVehicle.Size = new System.Drawing.Size(401, 62);
            this.lbl_currentVehicle.TabIndex = 24;
            this.lbl_currentVehicle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lbl_title
            // 
            this.lbl_title.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_title.BackColor = System.Drawing.Color.Silver;
            this.lbl_title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_title.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_title.Location = new System.Drawing.Point(870, 70);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(642, 29);
            this.lbl_title.TabIndex = 4;
            this.lbl_title.Text = "Message";
            this.lbl_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_close
            // 
            this.btn_close.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_close.BackColor = System.Drawing.Color.Yellow;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_close.Location = new System.Drawing.Point(1213, 765);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(303, 93);
            this.btn_close.TabIndex = 12;
            this.btn_close.Text = "닫  기";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_config
            // 
            this.btn_config.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_config.BackColor = System.Drawing.Color.Silver;
            this.btn_config.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_config.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_config.Location = new System.Drawing.Point(1213, 666);
            this.btn_config.Name = "btn_config";
            this.btn_config.Size = new System.Drawing.Size(303, 93);
            this.btn_config.TabIndex = 21;
            this.btn_config.Text = "설  정";
            this.btn_config.UseVisualStyleBackColor = false;
            this.btn_config.Click += new System.EventHandler(this.btn_config_Click);
            // 
            // btn_selectVehicle
            // 
            this.btn_selectVehicle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_selectVehicle.BackColor = System.Drawing.Color.Silver;
            this.btn_selectVehicle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_selectVehicle.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_selectVehicle.Location = new System.Drawing.Point(874, 765);
            this.btn_selectVehicle.Name = "btn_selectVehicle";
            this.btn_selectVehicle.Size = new System.Drawing.Size(333, 93);
            this.btn_selectVehicle.TabIndex = 22;
            this.btn_selectVehicle.Text = "차량 선택";
            this.btn_selectVehicle.UseVisualStyleBackColor = false;
            this.btn_selectVehicle.Click += new System.EventHandler(this.btn_selectVehicle_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(32, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(396, 57);
            this.label4.TabIndex = 23;
            this.label4.Text = "현재 검사 차량 : ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_inspectionStart
            // 
            this.btn_inspectionStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_inspectionStart.BackColor = System.Drawing.Color.Silver;
            this.btn_inspectionStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_inspectionStart.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_inspectionStart.Location = new System.Drawing.Point(40, 759);
            this.btn_inspectionStart.Name = "btn_inspectionStart";
            this.btn_inspectionStart.Size = new System.Drawing.Size(239, 93);
            this.btn_inspectionStart.TabIndex = 27;
            this.btn_inspectionStart.Text = "검사 시작";
            this.btn_inspectionStart.UseVisualStyleBackColor = false;
            this.btn_inspectionStart.Click += new System.EventHandler(this.btn_inspectionStart_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BackColor = System.Drawing.Color.LightGray;
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("굴림", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(1536, 68);
            this.label1.TabIndex = 28;
            this.label1.Text = "경 사 각 도  시 험 기";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_io
            // 
            this.btn_io.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_io.BackColor = System.Drawing.Color.Silver;
            this.btn_io.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_io.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_io.Location = new System.Drawing.Point(874, 666);
            this.btn_io.Name = "btn_io";
            this.btn_io.Size = new System.Drawing.Size(333, 93);
            this.btn_io.TabIndex = 33;
            this.btn_io.Text = "I/O";
            this.btn_io.UseVisualStyleBackColor = false;
            this.btn_io.Click += new System.EventHandler(this.btn_io_Click);
            // 
            // lbl_ioBoardComm
            // 
            this.lbl_ioBoardComm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_ioBoardComm.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_ioBoardComm.Location = new System.Drawing.Point(-381, 0);
            this.lbl_ioBoardComm.Name = "lbl_ioBoardComm";
            this.lbl_ioBoardComm.Size = new System.Drawing.Size(170, 68);
            this.lbl_ioBoardComm.TabIndex = 34;
            this.lbl_ioBoardComm.Text = "IoBoard";
            this.lbl_ioBoardComm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_sensorComm
            // 
            this.lbl_sensorComm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_sensorComm.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_sensorComm.Location = new System.Drawing.Point(-205, 0);
            this.lbl_sensorComm.Name = "lbl_sensorComm";
            this.lbl_sensorComm.Size = new System.Drawing.Size(170, 68);
            this.lbl_sensorComm.TabIndex = 35;
            this.lbl_sensorComm.Text = "Sensor";
            this.lbl_sensorComm.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_inspectionCompelete
            // 
            this.btn_inspectionCompelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btn_inspectionCompelete.BackColor = System.Drawing.Color.Silver;
            this.btn_inspectionCompelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_inspectionCompelete.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_inspectionCompelete.Location = new System.Drawing.Point(285, 759);
            this.btn_inspectionCompelete.Name = "btn_inspectionCompelete";
            this.btn_inspectionCompelete.Size = new System.Drawing.Size(239, 93);
            this.btn_inspectionCompelete.TabIndex = 37;
            this.btn_inspectionCompelete.Text = "검사 완료";
            this.btn_inspectionCompelete.UseVisualStyleBackColor = false;
            this.btn_inspectionCompelete.Click += new System.EventHandler(this.btn_inspectionCompelete_Click);
            // 
            // Incline
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1536, 864);
            this.Controls.Add(this.lbl_currentVehicle);
            this.Controls.Add(this.btn_inspectionCompelete);
            this.Controls.Add(this.lbl_sensorComm);
            this.Controls.Add(this.lbl_ioBoardComm);
            this.Controls.Add(this.btn_io);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_inspectionStart);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_selectVehicle);
            this.Controls.Add(this.btn_config);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.panel_arcGauge);
            this.Controls.Add(this.lbl_incAngle);
            this.Controls.Add(this.lbl_message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Incline";
            this.Text = "KI&T 경사각도";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Incline_FormClosed);
            this.Load += new System.EventHandler(this.Incline_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_message;
        private System.Windows.Forms.Label lbl_incAngle;
        private System.Windows.Forms.Panel panel_arcGauge;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_config;
        private System.Windows.Forms.Button btn_selectVehicle;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label lbl_currentVehicle;
        private System.Windows.Forms.Button btn_inspectionStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_io;
        private System.Windows.Forms.Label lbl_ioBoardComm;
        private System.Windows.Forms.Label lbl_sensorComm;
        private System.Windows.Forms.Button btn_inspectionCompelete;
    }
}
