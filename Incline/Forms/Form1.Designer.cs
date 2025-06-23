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
            this.lbl_message = new System.Windows.Forms.Label();
            this.lbl_arcGaugeValue = new System.Windows.Forms.Label();
            this.lbl_incAngle = new System.Windows.Forms.Label();
            this.panel_arcGauge = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_motorOn = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_motorOff = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.lbl_maxIncline = new System.Windows.Forms.Label();
            this.lbl_minIncline = new System.Windows.Forms.Label();
            this.cmb_ports = new System.Windows.Forms.ComboBox();
            this.btn_refreshPorts = new System.Windows.Forms.Button();
            this.lbl_maxInclineResult = new System.Windows.Forms.Label();
            this.lbl_minInclineResult = new System.Windows.Forms.Label();
            this.btn_connect = new System.Windows.Forms.Button();
            this.btn_config = new System.Windows.Forms.Button();
            this.btn_selectVehicle = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.lbl_currentVehicle = new System.Windows.Forms.Label();
            this.btn_inspectionStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_message
            // 
            this.lbl_message.BackColor = System.Drawing.Color.Black;
            this.lbl_message.Font = new System.Drawing.Font("굴림", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_message.ForeColor = System.Drawing.Color.Lime;
            this.lbl_message.Location = new System.Drawing.Point(1182, 75);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(701, 162);
            this.lbl_message.TabIndex = 0;
            this.lbl_message.Text = "label1";
            this.lbl_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_arcGaugeValue
            // 
            this.lbl_arcGaugeValue.BackColor = System.Drawing.Color.White;
            this.lbl_arcGaugeValue.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_arcGaugeValue.ForeColor = System.Drawing.Color.Black;
            this.lbl_arcGaugeValue.Location = new System.Drawing.Point(1682, 253);
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
            this.lbl_incAngle.Location = new System.Drawing.Point(1184, 317);
            this.lbl_incAngle.Name = "lbl_incAngle";
            this.lbl_incAngle.Size = new System.Drawing.Size(699, 368);
            this.lbl_incAngle.TabIndex = 2;
            this.lbl_incAngle.Text = "label3";
            this.lbl_incAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel_arcGauge
            // 
            this.panel_arcGauge.Location = new System.Drawing.Point(26, 46);
            this.panel_arcGauge.Name = "panel_arcGauge";
            this.panel_arcGauge.Size = new System.Drawing.Size(1150, 859);
            this.panel_arcGauge.TabIndex = 3;
            this.panel_arcGauge.Paint += new System.Windows.Forms.PaintEventHandler(this.panel_arcGauge_Paint);
            // 
            // lbl_title
            // 
            this.lbl_title.BackColor = System.Drawing.Color.Silver;
            this.lbl_title.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_title.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_title.Location = new System.Drawing.Point(1183, 46);
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
            this.label2.Location = new System.Drawing.Point(1185, 253);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(491, 51);
            this.label2.TabIndex = 5;
            this.label2.Text = "degree";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(1809, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 51);
            this.label3.TabIndex = 6;
            // 
            // btn_motorOn
            // 
            this.btn_motorOn.BackColor = System.Drawing.Color.SkyBlue;
            this.btn_motorOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_motorOn.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_motorOn.Location = new System.Drawing.Point(26, 937);
            this.btn_motorOn.Name = "btn_motorOn";
            this.btn_motorOn.Size = new System.Drawing.Size(304, 93);
            this.btn_motorOn.TabIndex = 8;
            this.btn_motorOn.Text = "모터 ON";
            this.btn_motorOn.UseVisualStyleBackColor = false;
            this.btn_motorOn.Click += new System.EventHandler(this.btn_motorOn_Click);
            // 
            // btn_save
            // 
            this.btn_save.BackColor = System.Drawing.Color.SkyBlue;
            this.btn_save.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_save.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_save.Location = new System.Drawing.Point(1187, 937);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(343, 93);
            this.btn_save.TabIndex = 10;
            this.btn_save.Text = "저   장";
            this.btn_save.UseVisualStyleBackColor = false;
            // 
            // btn_motorOff
            // 
            this.btn_motorOff.BackColor = System.Drawing.Color.SkyBlue;
            this.btn_motorOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_motorOff.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_motorOff.Location = new System.Drawing.Point(336, 937);
            this.btn_motorOff.Name = "btn_motorOff";
            this.btn_motorOff.Size = new System.Drawing.Size(303, 93);
            this.btn_motorOff.TabIndex = 11;
            this.btn_motorOff.Text = "모터 OFF";
            this.btn_motorOff.UseVisualStyleBackColor = false;
            this.btn_motorOff.Click += new System.EventHandler(this.btn_motorOff_Click);
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.Color.SkyBlue;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_close.Location = new System.Drawing.Point(1540, 937);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(343, 93);
            this.btn_close.TabIndex = 12;
            this.btn_close.Text = "닫  기";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // lbl_maxIncline
            // 
            this.lbl_maxIncline.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_maxIncline.Location = new System.Drawing.Point(1182, 699);
            this.lbl_maxIncline.Name = "lbl_maxIncline";
            this.lbl_maxIncline.Size = new System.Drawing.Size(153, 38);
            this.lbl_maxIncline.TabIndex = 13;
            this.lbl_maxIncline.Text = "최대경사각도";
            this.lbl_maxIncline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_minIncline
            // 
            this.lbl_minIncline.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_minIncline.Location = new System.Drawing.Point(1182, 748);
            this.lbl_minIncline.Name = "lbl_minIncline";
            this.lbl_minIncline.Size = new System.Drawing.Size(153, 38);
            this.lbl_minIncline.TabIndex = 14;
            this.lbl_minIncline.Text = "하한경사각도";
            this.lbl_minIncline.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmb_ports
            // 
            this.cmb_ports.FormattingEnabled = true;
            this.cmb_ports.Location = new System.Drawing.Point(1190, 811);
            this.cmb_ports.Name = "cmb_ports";
            this.cmb_ports.Size = new System.Drawing.Size(125, 23);
            this.cmb_ports.TabIndex = 17;
            // 
            // btn_refreshPorts
            // 
            this.btn_refreshPorts.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_refreshPorts.Location = new System.Drawing.Point(1414, 811);
            this.btn_refreshPorts.Name = "btn_refreshPorts";
            this.btn_refreshPorts.Size = new System.Drawing.Size(87, 23);
            this.btn_refreshPorts.TabIndex = 18;
            this.btn_refreshPorts.Text = "Refresh";
            this.btn_refreshPorts.UseVisualStyleBackColor = true;
            this.btn_refreshPorts.Click += new System.EventHandler(this.btn_refreshPorts_Click);
            // 
            // lbl_maxInclineResult
            // 
            this.lbl_maxInclineResult.BackColor = System.Drawing.Color.White;
            this.lbl_maxInclineResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_maxInclineResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_maxInclineResult.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_maxInclineResult.Location = new System.Drawing.Point(1337, 708);
            this.lbl_maxInclineResult.Name = "lbl_maxInclineResult";
            this.lbl_maxInclineResult.Size = new System.Drawing.Size(125, 29);
            this.lbl_maxInclineResult.TabIndex = 0;
            this.lbl_maxInclineResult.Text = "label4";
            this.lbl_maxInclineResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_minInclineResult
            // 
            this.lbl_minInclineResult.BackColor = System.Drawing.Color.White;
            this.lbl_minInclineResult.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lbl_minInclineResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lbl_minInclineResult.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_minInclineResult.Location = new System.Drawing.Point(1337, 753);
            this.lbl_minInclineResult.Name = "lbl_minInclineResult";
            this.lbl_minInclineResult.Size = new System.Drawing.Size(125, 29);
            this.lbl_minInclineResult.TabIndex = 19;
            this.lbl_minInclineResult.Text = "label5";
            this.lbl_minInclineResult.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_connect
            // 
            this.btn_connect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_connect.Location = new System.Drawing.Point(1321, 811);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(87, 23);
            this.btn_connect.TabIndex = 20;
            this.btn_connect.Text = "연결";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // btn_config
            // 
            this.btn_config.BackColor = System.Drawing.Color.Silver;
            this.btn_config.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_config.Font = new System.Drawing.Font("굴림", 28.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_config.Location = new System.Drawing.Point(1580, 708);
            this.btn_config.Name = "btn_config";
            this.btn_config.Size = new System.Drawing.Size(303, 93);
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
            this.btn_selectVehicle.Location = new System.Drawing.Point(1580, 812);
            this.btn_selectVehicle.Name = "btn_selectVehicle";
            this.btn_selectVehicle.Size = new System.Drawing.Size(303, 93);
            this.btn_selectVehicle.TabIndex = 22;
            this.btn_selectVehicle.Text = "차량 선택";
            this.btn_selectVehicle.UseVisualStyleBackColor = false;
            this.btn_selectVehicle.Click += new System.EventHandler(this.btn_selectVehicle_Click);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(1186, 867);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(177, 38);
            this.label4.TabIndex = 23;
            this.label4.Text = "현재 검사 차량 : ";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lbl_currentVehicle
            // 
            this.lbl_currentVehicle.Font = new System.Drawing.Font("굴림", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_currentVehicle.ForeColor = System.Drawing.Color.Red;
            this.lbl_currentVehicle.Location = new System.Drawing.Point(1369, 867);
            this.lbl_currentVehicle.Name = "lbl_currentVehicle";
            this.lbl_currentVehicle.Size = new System.Drawing.Size(185, 38);
            this.lbl_currentVehicle.TabIndex = 24;
            this.lbl_currentVehicle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_inspectionStart
            // 
            this.btn_inspectionStart.BackColor = System.Drawing.Color.Silver;
            this.btn_inspectionStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_inspectionStart.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_inspectionStart.Location = new System.Drawing.Point(940, 937);
            this.btn_inspectionStart.Name = "btn_inspectionStart";
            this.btn_inspectionStart.Size = new System.Drawing.Size(225, 93);
            this.btn_inspectionStart.TabIndex = 27;
            this.btn_inspectionStart.Text = "검사 시작";
            this.btn_inspectionStart.UseVisualStyleBackColor = false;
            this.btn_inspectionStart.Click += new System.EventHandler(this.btn_inspectionStart_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2382, 1325);
            this.Controls.Add(this.btn_inspectionStart);
            this.Controls.Add(this.lbl_currentVehicle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_selectVehicle);
            this.Controls.Add(this.btn_config);
            this.Controls.Add(this.btn_connect);
            this.Controls.Add(this.lbl_minInclineResult);
            this.Controls.Add(this.lbl_maxInclineResult);
            this.Controls.Add(this.btn_refreshPorts);
            this.Controls.Add(this.cmb_ports);
            this.Controls.Add(this.lbl_minIncline);
            this.Controls.Add(this.lbl_maxIncline);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_motorOff);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.btn_motorOn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_title);
            this.Controls.Add(this.panel_arcGauge);
            this.Controls.Add(this.lbl_incAngle);
            this.Controls.Add(this.lbl_arcGaugeValue);
            this.Controls.Add(this.lbl_message);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_motorOn;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.Button btn_motorOff;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Label lbl_maxIncline;
        private System.Windows.Forms.Label lbl_minIncline;
        private System.Windows.Forms.ComboBox cmb_ports;
        private System.Windows.Forms.Button btn_refreshPorts;
        private System.Windows.Forms.Label lbl_maxInclineResult;
        private System.Windows.Forms.Label lbl_minInclineResult;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Button btn_config;
        private System.Windows.Forms.Button btn_selectVehicle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbl_currentVehicle;
        private System.Windows.Forms.Button btn_inspectionStart;
    }
}

