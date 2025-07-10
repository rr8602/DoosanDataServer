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
            this.btn_connect = new System.Windows.Forms.Button();
            this.btn_refreshPorts = new System.Windows.Forms.Button();
            this.cmb_ports = new System.Windows.Forms.ComboBox();
            this.txt_maxInclineAngle = new System.Windows.Forms.TextBox();
            this.txt_minInclineAngle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
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
            this.btn_close.Location = new System.Drawing.Point(437, 428);
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
            this.btn_apply.Location = new System.Drawing.Point(287, 428);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(144, 54);
            this.btn_apply.TabIndex = 27;
            this.btn_apply.Text = "적용";
            this.btn_apply.UseVisualStyleBackColor = false;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // btn_connect
            // 
            this.btn_connect.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_connect.Location = new System.Drawing.Point(233, 368);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(168, 34);
            this.btn_connect.TabIndex = 31;
            this.btn_connect.Text = "연결";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // btn_refreshPorts
            // 
            this.btn_refreshPorts.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_refreshPorts.Location = new System.Drawing.Point(416, 368);
            this.btn_refreshPorts.Name = "btn_refreshPorts";
            this.btn_refreshPorts.Size = new System.Drawing.Size(159, 34);
            this.btn_refreshPorts.TabIndex = 30;
            this.btn_refreshPorts.Text = "Refresh";
            this.btn_refreshPorts.UseVisualStyleBackColor = true;
            this.btn_refreshPorts.Click += new System.EventHandler(this.btn_refreshPorts_Click);
            // 
            // cmb_ports
            // 
            this.cmb_ports.FormattingEnabled = true;
            this.cmb_ports.Location = new System.Drawing.Point(92, 375);
            this.cmb_ports.Name = "cmb_ports";
            this.cmb_ports.Size = new System.Drawing.Size(125, 23);
            this.cmb_ports.TabIndex = 29;
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
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 494);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_minInclineAngle);
            this.Controls.Add(this.txt_maxInclineAngle);
            this.Controls.Add(this.btn_connect);
            this.Controls.Add(this.btn_refreshPorts);
            this.Controls.Add(this.cmb_ports);
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
            this.Text = "ConfigForm";
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
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.Button btn_refreshPorts;
        private System.Windows.Forms.ComboBox cmb_ports;
        private System.Windows.Forms.TextBox txt_maxInclineAngle;
        private System.Windows.Forms.TextBox txt_minInclineAngle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}