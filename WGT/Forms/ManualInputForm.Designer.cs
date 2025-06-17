namespace WGT.Forms
{
    partial class ManualInputForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_frontLeft = new System.Windows.Forms.TextBox();
            this.txt_frontRight = new System.Windows.Forms.TextBox();
            this.txt_rearLeft = new System.Windows.Forms.TextBox();
            this.txt_rearRight = new System.Windows.Forms.TextBox();
            this.btn_confirm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 28);
            this.label1.TabIndex = 0;
            this.label1.Text = "전좌";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("굴림", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label2.Location = new System.Drawing.Point(12, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 28);
            this.label2.TabIndex = 1;
            this.label2.Text = "전우";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("굴림", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(12, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 28);
            this.label3.TabIndex = 2;
            this.label3.Text = "후좌";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("굴림", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label4.Location = new System.Drawing.Point(12, 185);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(68, 28);
            this.label4.TabIndex = 3;
            this.label4.Text = "후우";
            // 
            // txt_frontLeft
            // 
            this.txt_frontLeft.Location = new System.Drawing.Point(86, 19);
            this.txt_frontLeft.Name = "txt_frontLeft";
            this.txt_frontLeft.Size = new System.Drawing.Size(100, 25);
            this.txt_frontLeft.TabIndex = 4;
            this.txt_frontLeft.Text = "0";
            // 
            // txt_frontRight
            // 
            this.txt_frontRight.Location = new System.Drawing.Point(86, 74);
            this.txt_frontRight.Name = "txt_frontRight";
            this.txt_frontRight.Size = new System.Drawing.Size(100, 25);
            this.txt_frontRight.TabIndex = 5;
            this.txt_frontRight.Text = "0";
            // 
            // txt_rearLeft
            // 
            this.txt_rearLeft.Location = new System.Drawing.Point(86, 136);
            this.txt_rearLeft.Name = "txt_rearLeft";
            this.txt_rearLeft.Size = new System.Drawing.Size(100, 25);
            this.txt_rearLeft.TabIndex = 6;
            this.txt_rearLeft.Text = "0";
            // 
            // txt_rearRight
            // 
            this.txt_rearRight.Location = new System.Drawing.Point(86, 188);
            this.txt_rearRight.Name = "txt_rearRight";
            this.txt_rearRight.Size = new System.Drawing.Size(100, 25);
            this.txt_rearRight.TabIndex = 7;
            this.txt_rearRight.Text = "0";
            // 
            // btn_confirm
            // 
            this.btn_confirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_confirm.Location = new System.Drawing.Point(144, 220);
            this.btn_confirm.Name = "btn_confirm";
            this.btn_confirm.Size = new System.Drawing.Size(75, 35);
            this.btn_confirm.TabIndex = 8;
            this.btn_confirm.Text = "입력";
            this.btn_confirm.UseVisualStyleBackColor = true;
            this.btn_confirm.Click += new System.EventHandler(this.btn_confirm_Click);
            // 
            // ManualInputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(222, 267);
            this.Controls.Add(this.btn_confirm);
            this.Controls.Add(this.txt_rearRight);
            this.Controls.Add(this.txt_rearLeft);
            this.Controls.Add(this.txt_frontRight);
            this.Controls.Add(this.txt_frontLeft);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ManualInputForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "입력";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_frontLeft;
        private System.Windows.Forms.TextBox txt_frontRight;
        private System.Windows.Forms.TextBox txt_rearLeft;
        private System.Windows.Forms.TextBox txt_rearRight;
        private System.Windows.Forms.Button btn_confirm;
    }
}