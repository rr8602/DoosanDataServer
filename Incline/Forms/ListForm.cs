using Incline.Models;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;

namespace Incline.Forms
{
    public partial class ListForm : Form
    {
        public InclineMeasurement SelectedMeasurement { get; private set; }
        private SettingDb db;
        private Incline parent;

        private bool isFirstLoad = true;

        public ListForm(Incline parent, SettingDb db)
        {
            InitializeComponent();
            this.db = db;
            this.parent = parent;
            this.Load += ListForm_Load;
        }

        private void ListForm_Load(object sender, EventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            LoadDataFromDatabase();
        }

        private void LoadDataFromDatabase()
        {
            try
            {
                dataGridView1.Rows.Clear();
                using (OleDbConnection con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    string query = @"SELECT Accept_No, Vin_No, Model, Inc_Angle, Inspection_Status, Ok_Ng, Mea_Date 
                                    FROM Incline
                                    ORDER BY Mea_Date DESC";
                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            DisplayData(reader);
                        }
                    }
                    FormatDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터를 불러오는 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.ReadOnly = true;
        }

        private void DisplayData(OleDbDataReader reader)
        {
            if (reader.HasRows)
            {
                string todayDate = DateTime.Today.ToString("yyyyMMdd");
                while (reader.Read())
                {
                    var measurement = new InclineMeasurement
                    {
                        AcceptNo = reader["Accept_No"].ToString(),
                        VinNo = reader["Vin_No"].ToString(),
                        Model = reader["Model"].ToString(),
                        InclineAngle = Convert.ToDouble(reader["Inc_Angle"]),
                        InspectionStatus = Convert.ToBoolean(reader["Inspection_Status"]),
                        OkNg = Convert.ToBoolean(reader["Ok_Ng"]),
                        MeaDate = Convert.ToDateTime(reader["Mea_Date"])
                    };

                    if (isFirstLoad)
                    {
                        if (measurement.AcceptNo.Length >= 8 && measurement.AcceptNo.Substring(0, 8) == todayDate)
                        {
                            AddRowToGrid(measurement);
                        }
                    }
                    else
                    {
                        AddRowToGrid(measurement);
                    }
                }
                if (isFirstLoad) isFirstLoad = false;
            }
        }

        private void AddRowToGrid(InclineMeasurement m)
        {
            dataGridView1.Rows.Add(
                m.AcceptNo,
                m.VinNo,
                m.Model,
                m.InclineAngle.ToString("F1"),
                m.InspectionStatus ? "검사완료" : "검사대기",
                m.OkNg ? "양호" : "불량",
                m.MeaDate.ToString("yyyy-MM-dd HH:mm:ss")
            );
        }

        private void SearchData(string vinNo = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                dataGridView1.Rows.Clear();
                using (OleDbConnection con = new OleDbConnection(db.connectionString))
                {
                    con.Open();
                    string query = @"SELECT Accept_No, Vin_No, Model, Inc_Angle, Inspection_Status, Ok_Ng, Mea_Date
                                    FROM Incline WHERE 1=1";
                    List<OleDbParameter> parameters = new List<OleDbParameter>();

                    if (!string.IsNullOrWhiteSpace(vinNo))
                    {
                        query += " AND Vin_No LIKE ?";
                        parameters.Add(new OleDbParameter("@Vin_No", OleDbType.VarChar) { Value = "%" + vinNo + "%" });
                    }

                    if (date_start.Checked || date_end.Checked)
                    {
                        query += " AND Left(Accept_No, 8) BETWEEN ? AND ?";
                        string startDateStr = startDate.Value.ToString("yyyyMMdd");
                        string endDateStr = endDate.Value.ToString("yyyyMMdd");
                        parameters.Add(new OleDbParameter("@StartDate", OleDbType.VarChar) { Value = startDateStr });
                        parameters.Add(new OleDbParameter("@EndDate", OleDbType.VarChar) { Value = endDateStr });
                    }

                    query += " ORDER BY Mea_Date DESC";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            DisplayData(reader);
                            if (!reader.HasRows)
                            {
                                MessageBox.Show("검색 조건에 맞는 데이터가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    FormatDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터를 검색하는 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectAndClose(int rowIndex)
        {
            if (rowIndex < 0) return;

            SelectedMeasurement = new InclineMeasurement
            {
                AcceptNo = dataGridView1.Rows[rowIndex].Cells["Column1"].Value?.ToString() ?? "",
                VinNo = dataGridView1.Rows[rowIndex].Cells["Column2"].Value?.ToString() ?? "",
                Model = dataGridView1.Rows[rowIndex].Cells["Column3"].Value?.ToString() ?? ""
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_selectVehicle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                SelectAndClose(dataGridView1.SelectedRows[0].Index);
            }
            else
            {
                MessageBox.Show("선택된 차량이 없습니다. 행을 선택해주세요.", "선택 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void dataGridView1_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            SelectAndClose(e.RowIndex);
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string vinNo = txt_vinNo.Text.Trim();
            DateTime? startDate = date_start.Checked ? date_start.Value.Date : (DateTime?)null;
            DateTime? endDate = date_end.Checked ? date_end.Value.Date : (DateTime?)null;

            if (!string.IsNullOrEmpty(vinNo) || (startDate.HasValue && endDate.HasValue))
            {
                SearchData(vinNo, startDate, endDate);
            }
            else
            {
                LoadDataFromDatabase();
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.Value == null) return;

            if (e.ColumnIndex == 5)
            {
                string value = e.Value.ToString();
                if (value == "양호") e.CellStyle.ForeColor = Color.Green;
                else if (value == "불량") e.CellStyle.ForeColor = Color.Red;
                e.CellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
            }
            else if (e.ColumnIndex == 4)
            {
                if (e.Value.ToString() == "검사대기")
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                }
            }
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;
            dtp.CustomFormat = dtp.Checked ? "yyyy-MM-dd" : " ";
        }

        private void btn_resend_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count > 0)
                {
                    int rowIndex = dataGridView1.SelectedRows[0].Index;

                    string acceptNo = dataGridView1.Rows[rowIndex].Cells["Column1"].Value?.ToString() ?? "";
                    double IncAngle = double.Parse(dataGridView1.Rows[rowIndex].Cells["Column4"].Value?.ToString() ?? "0.0");

                    parent.SendInclineDataToServer(acceptNo, IncAngle);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터 재전송 중 오류가 발생했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}