namespace Incline.Forms
{
    partial class ConfigForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigForm));
            this.txt_serverPort = new System.Windows.Forms.TextBox();
            this.lbl_port = new System.Windows.Forms.Label();
            this.txt_serverIp = new System.Windows.Forms.TextBox();
            this.lbl_ip = new System.Windows.Forms.Label();
            this.btn_close = new System.Windows.Forms.Button();
            this.btn_apply = new System.Windows.Forms.Button();
            this.btn_ioConnect = new System.Windows.Forms.Button();
            this.btn_ioRefreshPorts = new System.Windows.Forms.Button();
            this.cmb_ioPorts = new System.Windows.Forms.ComboBox();
            this.txt_maxInclineAngle = new System.Windows.Forms.TextBox();
            this.txt_minInclineAngle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btn_sensorConnect = new System.Windows.Forms.Button();
            this.btn_sensorRefreshPorts = new System.Windows.Forms.Button();
            this.cmb_sensorPorts = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // txt_serverPort
            // 
            this.txt_serverPort.Font = new System.Drawing.Font("굴림", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_serverPort.Location = new System.Drawing.Point(236, 91);
            this.txt_serverPort.Multiline = true;
            this.txt_serverPort.Name = "txt_serverPort";
            this.txt_serverPort.Size = new System.Drawing.Size(335, 50);
            this.txt_serverPort.TabIndex = 24;
            // 
            // lbl_port
            // 
            this.lbl_port.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_port.Location = new System.Drawing.Point(25, 91);
            this.lbl_port.Name = "lbl_port";
            this.lbl_port.Size = new System.Drawing.Size(167, 50);
            this.lbl_port.TabIndex = 23;
            this.lbl_port.Text = "PORT";
            this.lbl_port.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_serverIp
            // 
            this.txt_serverIp.Font = new System.Drawing.Font("굴림", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_serverIp.Location = new System.Drawing.Point(236, 22);
            this.txt_serverIp.Multiline = true;
            this.txt_serverIp.Name = "txt_serverIp";
            this.txt_serverIp.Size = new System.Drawing.Size(335, 50);
            this.txt_serverIp.TabIndex = 22;
            // 
            // lbl_ip
            // 
            this.lbl_ip.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_ip.Location = new System.Drawing.Point(25, 22);
            this.lbl_ip.Name = "lbl_ip";
            this.lbl_ip.Size = new System.Drawing.Size(167, 50);
            this.lbl_ip.TabIndex = 21;
            this.lbl_ip.Text = "Server IP";
            this.lbl_ip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.Color.Silver;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_close.Location = new System.Drawing.Point(431, 499);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(140, 54);
            this.btn_close.TabIndex = 28;
            this.btn_close.Text = "닫기";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btn_apply
            // 
            this.btn_apply.BackColor = System.Drawing.Color.Silver;
            this.btn_apply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_apply.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_apply.Location = new System.Drawing.Point(281, 499);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(144, 54);
            this.btn_apply.TabIndex = 27;
            this.btn_apply.Text = "적용";
            this.btn_apply.UseVisualStyleBackColor = false;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // btn_ioConnect
            // 
            this.btn_ioConnect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ioConnect.Location = new System.Drawing.Point(403, 317);
            this.btn_ioConnect.Name = "btn_ioConnect";
            this.btn_ioConnect.Size = new System.Drawing.Size(168, 23);
            this.btn_ioConnect.TabIndex = 31;
            this.btn_ioConnect.Text = "Connect";
            this.btn_ioConnect.UseVisualStyleBackColor = true;
            this.btn_ioConnect.Click += new System.EventHandler(this.btn_ioConnect_Click);
            // 
            // btn_ioRefreshPorts
            // 
            this.btn_ioRefreshPorts.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_ioRefreshPorts.Location = new System.Drawing.Point(403, 346);
            this.btn_ioRefreshPorts.Name = "btn_ioRefreshPorts";
            this.btn_ioRefreshPorts.Size = new System.Drawing.Size(168, 23);
            this.btn_ioRefreshPorts.TabIndex = 30;
            this.btn_ioRefreshPorts.Text = "Refresh";
            this.btn_ioRefreshPorts.UseVisualStyleBackColor = true;
            this.btn_ioRefreshPorts.Click += new System.EventHandler(this.btn_refreshPorts_Click);
            // 
            // cmb_ioPorts
            // 
            this.cmb_ioPorts.FormattingEnabled = true;
            this.cmb_ioPorts.Location = new System.Drawing.Point(236, 317);
            this.cmb_ioPorts.Name = "cmb_ioPorts";
            this.cmb_ioPorts.Size = new System.Drawing.Size(161, 23);
            this.cmb_ioPorts.TabIndex = 29;
            // 
            // txt_maxInclineAngle
            // 
            this.txt_maxInclineAngle.Font = new System.Drawing.Font("굴림", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_maxInclineAngle.Location = new System.Drawing.Point(236, 161);
            this.txt_maxInclineAngle.Multiline = true;
            this.txt_maxInclineAngle.Name = "txt_maxInclineAngle";
            this.txt_maxInclineAngle.Size = new System.Drawing.Size(335, 50);
            this.txt_maxInclineAngle.TabIndex = 35;
            // 
            // txt_minInclineAngle
            // 
            this.txt_minInclineAngle.Font = new System.Drawing.Font("굴림", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_minInclineAngle.Location = new System.Drawing.Point(236, 229);
            this.txt_minInclineAngle.Multiline = true;
            this.txt_minInclineAngle.Name = "txt_minInclineAngle";
            this.txt_minInclineAngle.Size = new System.Drawing.Size(335, 50);
            this.txt_minInclineAngle.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(25, 229);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 50);
            this.label1.TabIndex = 37;
            this.label1.Text = "최소경사각도";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(25, 161);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(167, 50);
            this.label2.TabIndex = 38;
            this.label2.Text = "최대경사각도";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(25, 300);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 50);
            this.label3.TabIndex = 39;
            this.label3.Text = "IoBoard";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(25, 380);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 50);
            this.label4.TabIndex = 43;
            this.label4.Text = "Sensor";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_sensorConnect
            // 
            this.btn_sensorConnect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_sensorConnect.Location = new System.Drawing.Point(403, 397);
            this.btn_sensorConnect.Name = "btn_sensorConnect";
            this.btn_sensorConnect.Size = new System.Drawing.Size(168, 23);
            this.btn_sensorConnect.TabIndex = 42;
            this.btn_sensorConnect.Text = "Connect";
            this.btn_sensorConnect.UseVisualStyleBackColor = true;
            this.btn_sensorConnect.Click += new System.EventHandler(this.btn_sensorConnect_Click);
            // 
            // btn_sensorRefreshPorts
            // 
            this.btn_sensorRefreshPorts.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_sensorRefreshPorts.Location = new System.Drawing.Point(403, 426);
            this.btn_sensorRefreshPorts.Name = "btn_sensorRefreshPorts";
            this.btn_sensorRefreshPorts.Size = new System.Drawing.Size(168, 23);
            this.btn_sensorRefreshPorts.TabIndex = 41;
            this.btn_sensorRefreshPorts.Text = "Refresh";
            this.btn_sensorRefreshPorts.UseVisualStyleBackColor = true;
            this.btn_sensorRefreshPorts.Click += new System.EventHandler(this.btn_sensorRefreshPorts_Click);
            // 
            // cmb_sensorPorts
            // 
            this.cmb_sensorPorts.FormattingEnabled = true;
            this.cmb_sensorPorts.Location = new System.Drawing.Point(236, 397);
            this.cmb_sensorPorts.Name = "cmb_sensorPorts";
            this.cmb_sensorPorts.Size = new System.Drawing.Size(161, 23);
            this.cmb_sensorPorts.TabIndex = 40;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 565);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btn_sensorConnect);
            this.Controls.Add(this.btn_sensorRefreshPorts);
            this.Controls.Add(this.cmb_sensorPorts);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_minInclineAngle);
            this.Controls.Add(this.txt_maxInclineAngle);
            this.Controls.Add(this.btn_ioConnect);
            this.Controls.Add(this.btn_ioRefreshPorts);
            this.Controls.Add(this.cmb_ioPorts);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_apply);
            this.Controls.Add(this.txt_serverPort);
            this.Controls.Add(this.lbl_port);
            this.Controls.Add(this.txt_serverIp);
            this.Controls.Add(this.lbl_ip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "설정";
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_serverPort;
        private System.Windows.Forms.Label lbl_port;
        private System.Windows.Forms.TextBox txt_serverIp;
        private System.Windows.Forms.Label lbl_ip;
        private System.Windows.Forms.Button btn_close;
        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Button btn_ioConnect;
        private System.Windows.Forms.Button btn_ioRefreshPorts;
        private System.Windows.Forms.ComboBox cmb_ioPorts;
        private System.Windows.Forms.TextBox txt_maxInclineAngle;
        private System.Windows.Forms.TextBox txt_minInclineAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btn_sensorConnect;
        private System.Windows.Forms.Button btn_sensorRefreshPorts;
        private System.Windows.Forms.ComboBox cmb_sensorPorts;
    }
}