namespace Incline.Forms
{
    partial class ListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListForm));
            this.btn_selectVehicle = new System.Windows.Forms.Button();
            this.date_end = new System.Windows.Forms.DateTimePicker();
            this.date_start = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btn_search = new System.Windows.Forms.Button();
            this.txt_vinNo = new System.Windows.Forms.TextBox();
            this.lbl_receptionNumber = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_selectVehicle
            // 
            this.btn_selectVehicle.BackColor = System.Drawing.Color.Silver;
            this.btn_selectVehicle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_selectVehicle.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_selectVehicle.Location = new System.Drawing.Point(812, 549);
            this.btn_selectVehicle.Name = "btn_selectVehicle";
            this.btn_selectVehicle.Size = new System.Drawing.Size(183, 42);
            this.btn_selectVehicle.TabIndex = 18;
            this.btn_selectVehicle.Text = "차량 선택";
            this.btn_selectVehicle.UseVisualStyleBackColor = false;
            this.btn_selectVehicle.Click += new System.EventHandler(this.btn_selectVehicle_Click);
            // 
            // date_end
            // 
            this.date_end.Checked = false;
            this.date_end.CustomFormat = "";
            this.date_end.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date_end.Location = new System.Drawing.Point(670, 37);
            this.date_end.Name = "date_end";
            this.date_end.ShowCheckBox = true;
            this.date_end.Size = new System.Drawing.Size(130, 25);
            this.date_end.TabIndex = 17;
            // 
            // date_start
            // 
            this.date_start.Checked = false;
            this.date_start.CustomFormat = "";
            this.date_start.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.date_start.Location = new System.Drawing.Point(476, 37);
            this.date_start.Name = "date_start";
            this.date_start.ShowCheckBox = true;
            this.date_start.Size = new System.Drawing.Size(131, 25);
            this.date_start.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label3.Location = new System.Drawing.Point(627, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 39);
            this.label3.TabIndex = 15;
            this.label3.Text = "~";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btn_search
            // 
            this.btn_search.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btn_search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_search.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btn_search.Location = new System.Drawing.Point(870, 27);
            this.btn_search.Name = "btn_search";
            this.btn_search.Size = new System.Drawing.Size(125, 42);
            this.btn_search.TabIndex = 14;
            this.btn_search.Text = "검색";
            this.btn_search.UseVisualStyleBackColor = false;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // txt_vinNo
            // 
            this.txt_vinNo.Font = new System.Drawing.Font("굴림", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.txt_vinNo.Location = new System.Drawing.Point(172, 20);
            this.txt_vinNo.Multiline = true;
            this.txt_vinNo.Name = "txt_vinNo";
            this.txt_vinNo.Size = new System.Drawing.Size(235, 53);
            this.txt_vinNo.TabIndex = 13;
            // 
            // lbl_receptionNumber
            // 
            this.lbl_receptionNumber.Font = new System.Drawing.Font("굴림", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lbl_receptionNumber.Location = new System.Drawing.Point(-177, 207);
            this.lbl_receptionNumber.Name = "lbl_receptionNumber";
            this.lbl_receptionNumber.Size = new System.Drawing.Size(172, 39);
            this.lbl_receptionNumber.TabIndex = 12;
            this.lbl_receptionNumber.Text = "접수번호 : ";
            this.lbl_receptionNumber.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column7,
            this.Column2,
            this.Column5,
            this.Column4,
            this.Column6});
            this.dataGridView1.Location = new System.Drawing.Point(-1, 183);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1008, 341);
            this.dataGridView1.TabIndex = 19;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick_1);
            this.dataGridView1.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dataGridView1_CellFormatting);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("굴림", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(23, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 40);
            this.label1.TabIndex = 20;
            this.label1.Text = "차대번호";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "접수번호";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.Width = 155;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "차대번호";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.Width = 155;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "모델";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.Width = 155;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "경사각도";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.Width = 130;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "검사여부";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.Width = 95;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "OK / NG";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.Width = 95;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "검사시간";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.Width = 170;
            // 
            // ListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 621);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.btn_selectVehicle);
            this.Controls.Add(this.date_end);
            this.Controls.Add(this.date_start);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_search);
            this.Controls.Add(this.txt_vinNo);
            this.Controls.Add(this.lbl_receptionNumber);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ListForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_selectVehicle;
        private System.Windows.Forms.DateTimePicker date_end;
        private System.Windows.Forms.DateTimePicker date_start;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.TextBox txt_vinNo;
        private System.Windows.Forms.Label lbl_receptionNumber;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
    }
}