using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using WGT.Forms;

namespace WGT
{
    public class SettingDb
    {
        public string connectionString { get; private set; }
        private Form1 form;
        private ListForm listForm;

        public SettingDb(Form1 form) : this(form, null)
        {
        }

        public SettingDb(Form1 form, ListForm listForm)
        {
            this.form = form;
            this.listForm = listForm;
        }

        public void SetListForm(ListForm listForm)
        {
            this.listForm = listForm;
        }

        // DB 연결 설정
        public void SetupDatabaseConnection()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["AccessConnection"].ConnectionString;

                AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

                if (string.IsNullOrEmpty(connectionString))
                {
                    string dbPath = Path.Combine(Application.StartupPath, "WGT_Data.mdb");

                    if (File.Exists(dbPath))
                    {
                        connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath};";
                    }
                    else
                    {
                        dbPath = Path.Combine(Application.StartupPath, "WGT_Data.accdb");

                        if (File.Exists(dbPath))
                        {
                            connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";
                        }
                        else
                        {
                            throw new FileNotFoundException("데이터베이스 파일(WGT_Data.mdb 또는 WGT_Data.accdb)을 찾을 수 없습니다.");
                        }
                    }
                }

                Console.WriteLine($"데이터베이스 연결 문자열: {connectionString}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터베이스 연결 설정에 실패했습니다: " + ex.Message, "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // DB에 측정 데이터 저장 or 수정
        public void SaveMeasurementDataToMDB(string acceptNo = null)
        {
            string vehicleAcceptNo = acceptNo ?? (listForm?.SelectedAccpetNo ?? string.Empty);

            if (string.IsNullOrEmpty(vehicleAcceptNo))
            {
                MessageBox.Show("차량이 선택되지 않았습니다. 차량을 선택하거나 바코드를 입력해주세요.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (OleDbConnection con = new OleDbConnection(connectionString))
                {
                    con.Open();

                    // AcceptNo가 이미 존재하는지 확인
                    string checkSql = "SELECT COUNT(*) FROM MeasurementData WHERE Accept_No = ?";

                    using (OleDbCommand checkCmd = new OleDbCommand(checkSql, con))
                    {
                        checkCmd.Parameters.Add("@AcceptNo", OleDbType.VarChar).Value = acceptNo;

                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            // 기존 데이터가 존재하면 UPDATE 수행
                            string updateSql = @"UPDATE MeasurementData 
                                         SET Mea_Date = ?, 
                                             F_LW = ?, 
                                             F_RW = ?, 
                                             F_TW = ?, 
                                             R_LW = ?, 
                                             R_RW = ?, 
                                             R_TW = ?, 
                                             Total_Weight = ?
                                         WHERE Accept_No = ?";

                            using (OleDbCommand updateCmd = new OleDbCommand(updateSql, con))
                            {
                                updateCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                                updateCmd.Parameters.Add("@F_LW", OleDbType.Double).Value = form.frontLeftWeight;
                                updateCmd.Parameters.Add("@F_RW", OleDbType.Double).Value = form.frontRightWeight;
                                updateCmd.Parameters.Add("@F_TW", OleDbType.Double).Value = form.frontTotalWeight;
                                updateCmd.Parameters.Add("@R_LW", OleDbType.Double).Value = form.rearLeftWeight;
                                updateCmd.Parameters.Add("@R_RW", OleDbType.Double).Value = form.rearRightWeight;
                                updateCmd.Parameters.Add("@R_TW", OleDbType.Double).Value = form.rearTotalWeight;
                                updateCmd.Parameters.Add("@Total_Weight", OleDbType.Double).Value = form.totalWeight;
                                updateCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            // 기존 데이터가 없으면 INSERT 수행
                            string insertSql = @"INSERT INTO MeasurementData
                                         (Accept_No, Mea_Date, F_LW, F_RW, F_TW,
                                          R_LW, R_RW, R_TW, Total_Weight)
                                         VALUES
                                         (?, ?, ?, ?, ?, ?, ?, ?, ?)";

                            using (OleDbCommand insertCmd = new OleDbCommand(insertSql, con))
                            {
                                insertCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                                insertCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                                insertCmd.Parameters.Add("@F_LW", OleDbType.Double).Value = form.frontLeftWeight;
                                insertCmd.Parameters.Add("@F_RW", OleDbType.Double).Value = form.frontRightWeight;
                                insertCmd.Parameters.Add("@F_TW", OleDbType.Double).Value = form.frontTotalWeight;
                                insertCmd.Parameters.Add("@R_LW", OleDbType.Double).Value = form.rearLeftWeight;
                                insertCmd.Parameters.Add("@R_RW", OleDbType.Double).Value = form.rearRightWeight;
                                insertCmd.Parameters.Add("@R_TW", OleDbType.Double).Value = form.rearTotalWeight;
                                insertCmd.Parameters.Add("@Total_Weight", OleDbType.Double).Value = form.totalWeight;
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }

                MessageBox.Show("측정 데이터가 성공적으로 저장되었습니다.", "저장 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("측정 데이터를 저장하는 데 실패했습니다: " + ex.Message, "저장 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
