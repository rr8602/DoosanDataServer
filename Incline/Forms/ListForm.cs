using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Incline.Forms
{
    public partial class ListForm : Form
    {
        public string SelectedVinNo { get; private set; }
        private SettingDb db;

        public ListForm(Form1 parent, SettingDb db)
        {
            InitializeComponent();
            this.db = db;
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
                while (reader.Read())
                {
                    string acceptNo = reader["Accept_No"].ToString();
                    string vinNo = reader["Vin_No"].ToString();
                    string model = reader["Model"].ToString();
                    double IncAngle = Convert.ToDouble(reader["Inc_Angle"]);
                    bool inspectionStatus = Convert.ToBoolean(reader["Inspection_Status"]);
                    bool okNg = Convert.ToBoolean(reader["Ok_Ng"]);
                    DateTime measurementDate = Convert.ToDateTime(reader["Mea_Date"]);

                    string todayDate = DateTime.Today.ToString("yyyyMMdd");

                    if (acceptNo.StartsWith(todayDate))
                    {
                        dataGridView1.Rows.Add
                        (
                            acceptNo,
                            vinNo,
                            model,
                            IncAngle.ToString("F1"),
                            inspectionStatus ? "검사완료" : "검사대기",
                            okNg ? "양호" : "불량",
                            measurementDate.ToString("yyyy-MM-dd HH:mm:ss")
                        );
                    }
                }
            }
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

                    // 차대번호 검색 조건 추가
                    if (!string.IsNullOrWhiteSpace(vinNo))
                    {
                        query += " AND Vin_No LIKE ?";
                        parameters.Add(new OleDbParameter("@Vin_No", OleDbType.VarChar) { Value = "%" + vinNo + "%" });
                    }

                    // 날짜 검색 조건 추가
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        query += " AND Mea_Date BETWEEN ? AND ?";
                        parameters.Add(new OleDbParameter("@StartDate", OleDbType.Date) { Value = startDate.Value });

                        DateTime endDateWithTime = endDate.Value.Date.AddDays(1).AddSeconds(-1);
                        parameters.Add(new OleDbParameter("@EndDate", OleDbType.Date) { Value = endDateWithTime });
                    }

                    // 결과 정렬
                    query += " ORDER BY Mea_Date DESC";

                    using (OleDbCommand cmd = new OleDbCommand(query, con))
                    {
                        // 파라미터 추가
                        foreach (OleDbParameter parameter in parameters)
                        {
                            cmd.Parameters.Add(parameter);
                        }

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

        private void btn_selectVehicle_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                SelectedVinNo = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("선택된 차량이 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dataGridView1_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectedVinNo = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string vinNo = txt_vinNo.Text.Trim();

            DateTime? startDate = date_start.Checked ? date_start.Value.Date : (DateTime?)null;
            DateTime? endDate = date_end.Checked ? date_end.Value.Date : (DateTime?)null;

            // 차대번호만 입력한 경우
            if (!string.IsNullOrEmpty(vinNo) && !startDate.HasValue && !endDate.HasValue)
            {
                SearchData(vinNo: vinNo);
            }
            // 날짜만 선택한 경우
            else if (string.IsNullOrEmpty(vinNo) && startDate.HasValue && endDate.HasValue && startDate <= endDate)
            {
                SearchData(startDate: startDate, endDate: endDate);
            }
            // 차대번호와 날짜 모두 입력한 경우
            else if (!string.IsNullOrEmpty(vinNo) && startDate.HasValue && endDate.HasValue && startDate <= endDate)
            {
                SearchData(vinNo: vinNo, startDate: startDate, endDate: endDate);
            }
            // 시작 날짜만 선택한 경우
            else if (startDate.HasValue && !endDate.HasValue)
            {
                SearchData(vinNo: vinNo, startDate: startDate, endDate: startDate); // 같은 날짜로 검색
            }
            // 종료 날짜만 선택한 경우
            else if (!startDate.HasValue && endDate.HasValue)
            {
                SearchData(vinNo: vinNo, startDate: endDate, endDate: endDate); // 같은 날짜로 검색
            }
            // 날짜 범위가 유효하지 않은 경우
            else if (startDate.HasValue && endDate.HasValue && startDate > endDate)
            {
                MessageBox.Show("검색 조건이 올바르지 않습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // 아무것도 입력되지 않은 경우
            else
            {
                LoadDataFromDatabase();
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 4 && e.RowIndex >= 0 && e.Value != null)
            {
                string value = e.Value.ToString();

                if (value == "양호")
                {
                    e.CellStyle.ForeColor = Color.Green;
                    e.CellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                }
                else if (value == "불량")
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                }
            }
            else if (e.ColumnIndex == 3 && e.RowIndex >= 0 && e.Value != null)
            {
                string value = e.Value.ToString();

                if (value == "검사대기")
                {
                    e.CellStyle.ForeColor = Color.Red;
                    e.CellStyle.Font = new Font(dataGridView1.Font, FontStyle.Bold);
                }
            }
        }

        private void DateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            DateTimePicker dtp = (DateTimePicker)sender;

            if (dtp.Checked)
            {
                dtp.CustomFormat = "yyyy-MM-dd";
            }
            else
            {
                dtp.CustomFormat = " ";
            }
        }
    }
}
