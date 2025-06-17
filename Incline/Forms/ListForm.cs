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

namespace Incline.Forms
{
    public partial class ListForm : Form
    {
        public string SelectedAccpetNo { get; private set; }
        private SettingDb db;

        public ListForm(Form1 parent, SettingDb db)
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
            this.db = db;
            this.Load += ListForm_Load;
        }

        private void ListForm_Load(object sender, EventArgs e)
        {
            dataGridView1.RowHeadersVisible = false;
            date_start.Value = DateTime.Now.AddDays(-7);
            date_end.Value = DateTime.Now;
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

                    string query = @"SELECT Accept_No, Mea_Date, Inc_Angle 
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
                    double IncAngle = Convert.ToDouble(reader["Inc_Angle"]);
                    DateTime measurementDate = Convert.ToDateTime(reader["Mea_Date"]);

                    dataGridView1.Rows.Add(
                        acceptNo,
                        IncAngle.ToString("F1"),
                        measurementDate.ToString("yyyy-MM-dd HH:mm:ss")
                    );
                }
            }
            else
            {
                MessageBox.Show("검색 조건에 맞는 데이터가 없습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void SearchData(string accpetNo = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            try
            {
                dataGridView1.Rows.Clear();

                using (OleDbConnection con = new OleDbConnection(db.connectionString))
                {
                    con.Open();

                    string query = @"SELECT Accept_No, Mea_Date, Inc_Angle 
                                    FROM Incline WHERE 1=1";

                    List<OleDbParameter> parameters = new List<OleDbParameter>();

                    // 바코드 검색 조건 추가
                    if (!string.IsNullOrWhiteSpace(accpetNo))
                    {
                        query += " AND Accept_No LIKE ?";
                        parameters.Add(new OleDbParameter("@Accept_No", OleDbType.VarChar) { Value = "%" + accpetNo + "%" });
                    }

                    // 날짜 검색 조건 추가
                    if (startDate.HasValue && endDate.HasValue)
                    {
                        query += " AND Mea_Date BETWEEN ? AND ?";
                        parameters.Add(new OleDbParameter("@StartDate", OleDbType.Date) { Value = startDate.Value });

                        parameters.Add(new OleDbParameter("@EndDate", OleDbType.Date) { Value = endDate.Value });
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
                SelectedAccpetNo = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
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
                SelectedAccpetNo = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            string accpetNo = txt_receptionNumber.Text.Trim();

            DateTime startDate = date_start.Value.Date;
            DateTime endDate = date_end.Value.Date;

            // 접수번호만 입력
            if (!string.IsNullOrEmpty(accpetNo) && startDate == endDate)
            {
                SearchData(accpetNo: accpetNo);
            }
            // 날짜만 선택
            else if (string.IsNullOrEmpty(accpetNo) && startDate <= endDate)
            {
                SearchData(startDate: startDate, endDate: endDate);
            }
            // 접수번호와 날짜 모두 입력
            else if (!string.IsNullOrEmpty(accpetNo) && startDate <= endDate)
            {
                SearchData(accpetNo: accpetNo, startDate: startDate, endDate: endDate);
            }
            // 아무것도 입력되지 않거나 날짜 범위가 유효하지 않은 경우
            else
            {
                if (startDate > endDate)
                {
                    MessageBox.Show("검색 조건이 올바르지 않습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // 모든 데이터 로드
                    LoadDataFromDatabase();
                }
            }
        }
    }
}
