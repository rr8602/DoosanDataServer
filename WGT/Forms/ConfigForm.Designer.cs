namespace WGT.Forms
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
            this.txt_ip = new System.Windows.Forms.TextBox();
            this.lbl_ip = new System.Windows.Forms.Label();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.lbl_port = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_minWeight = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_runout = new System.Windows.Forms.TextBox();
            this.btn_apply = new System.Windows.Forms.Button();
            this.btn_close = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_ip
            // 
            this.txt_ip.Font = new System.Drawing.Font("굴림", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_ip.Location = new System.Drawing.Point(362, 20);
            this.txt_ip.Multiline = true;
            this.txt_ip.Name = "txt_ip";
            this.txt_ip.Size = new System.Drawing.Size(335, 50);
            this.txt_ip.TabIndex = 18;
            // 
            // lbl_ip
            // 
            this.lbl_ip.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_ip.Location = new System.Drawing.Point(12, 20);
            this.lbl_ip.Name = "lbl_ip";
            this.lbl_ip.Size = new System.Drawing.Size(289, 50);
            this.lbl_ip.TabIndex = 17;
            this.lbl_ip.Text = "IP";
            this.lbl_ip.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_port
            // 
            this.txt_port.Font = new System.Drawing.Font("굴림", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_port.Location = new System.Drawing.Point(362, 89);
            this.txt_port.Multiline = true;
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(335, 50);
            this.txt_port.TabIndex = 20;
            // 
            // lbl_port
            // 
            this.lbl_port.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_port.Location = new System.Drawing.Point(12, 89);
            this.lbl_port.Name = "lbl_port";
            this.lbl_port.Size = new System.Drawing.Size(289, 50);
            this.lbl_port.TabIndex = 19;
            this.lbl_port.Text = "PORT";
            this.lbl_port.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 168);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(289, 50);
            this.label1.TabIndex = 21;
            this.label1.Text = "기준 중량 (kg)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_minWeight
            // 
            this.txt_minWeight.Font = new System.Drawing.Font("굴림", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_minWeight.Location = new System.Drawing.Point(362, 168);
            this.txt_minWeight.Multiline = true;
            this.txt_minWeight.Name = "txt_minWeight";
            this.txt_minWeight.Size = new System.Drawing.Size(335, 50);
            this.txt_minWeight.TabIndex = 22;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 246);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(289, 50);
            this.label2.TabIndex = 23;
            this.label2.Text = "흔들림 (kg)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txt_runout
            // 
            this.txt_runout.Font = new System.Drawing.Font("굴림", 19.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_runout.Location = new System.Drawing.Point(362, 246);
            this.txt_runout.Multiline = true;
            this.txt_runout.Name = "txt_runout";
            this.txt_runout.Size = new System.Drawing.Size(335, 50);
            this.txt_runout.TabIndex = 24;
            // 
            // btn_apply
            // 
            this.btn_apply.BackColor = System.Drawing.Color.Silver;
            this.btn_apply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_apply.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_apply.Location = new System.Drawing.Point(407, 344);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(144, 54);
            this.btn_apply.TabIndex = 25;
            this.btn_apply.Text = "적용";
            this.btn_apply.UseVisualStyleBackColor = false;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // btn_close
            // 
            this.btn_close.BackColor = System.Drawing.Color.Silver;
            this.btn_close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_close.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_close.Location = new System.Drawing.Point(557, 344);
            this.btn_close.Name = "btn_close";
            this.btn_close.Size = new System.Drawing.Size(140, 54);
            this.btn_close.TabIndex = 26;
            this.btn_close.Text = "닫기";
            this.btn_close.UseVisualStyleBackColor = false;
            this.btn_close.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 424);
            this.Controls.Add(this.btn_close);
            this.Controls.Add(this.btn_apply);
            this.Controls.Add(this.txt_runout);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_minWeight);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_port);
            this.Controls.Add(this.lbl_port);
            this.Controls.Add(this.txt_ip);
            this.Controls.Add(this.lbl_ip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConfigForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_ip;
        private System.Windows.Forms.Label lbl_ip;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label lbl_port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_minWeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_runout;
        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Button btn_close;
    }
}