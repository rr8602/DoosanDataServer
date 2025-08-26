using Incline.Models;
using System;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace Incline
{
    public class SettingDb
    {
        public string connectionString { get; private set; }

        public SettingDb()
        {
        }

        public void SetupDatabaseConnection()
        {
            try
            {
                connectionString = ConfigurationManager.ConnectionStrings["AccessConnection"].ConnectionString;

                AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

                if (string.IsNullOrEmpty(connectionString))
                {
                    string dbPath = Path.Combine(Application.StartupPath, "Incline_Data.mdb");

                    if (File.Exists(dbPath))
                    {
                        connectionString = $"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={dbPath};";
                    }
                    else
                    {
                        dbPath = Path.Combine(Application.StartupPath, "Incline_Data.accdb");

                        if (File.Exists(dbPath))
                        {
                            connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";
                        }
                        else
                        {
                            throw new FileNotFoundException("데이터베이스 파일(Incline_Data.mdb 또는 Incline_Data.accdb)을 찾을 수 없습니다.");
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

        public void SaveMeasurementDataToMDB(InclineMeasurement measurement)
        {
            if (measurement == null || string.IsNullOrEmpty(measurement.VinNo))
            {
                MessageBox.Show("차량 정보가 유효하지 않습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (OleDbConnection con = new OleDbConnection(connectionString))
                {
                    con.Open();

                    string checkSql = "SELECT COUNT(*) FROM Incline WHERE Vin_No = ?";
                    using (OleDbCommand checkCmd = new OleDbCommand(checkSql, con))
                    {
                        checkCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = measurement.VinNo;
                        int count = (int)checkCmd.ExecuteScalar();

                        if (count > 0)
                        {
                            string updateSql = @"UPDATE Incline
                                         SET Mea_Date = ?, OK_Ng = ?, Inspection_Status = ?, Inc_Angle = ?, Accept_No = ?, Model = ?
                                         WHERE Vin_No = ?";
                            using (OleDbCommand updateCmd = new OleDbCommand(updateSql, con))
                            {
                                updateCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = measurement.MeaDate;
                                updateCmd.Parameters.Add("@OK_Ng", OleDbType.Boolean).Value = measurement.OkNg;
                                updateCmd.Parameters.Add("@Inspection_Status", OleDbType.Boolean).Value = measurement.InspectionStatus;
                                updateCmd.Parameters.Add("@Inc_Angle", OleDbType.Double).Value = measurement.InclineAngle;
                                updateCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = measurement.AcceptNo;
                                updateCmd.Parameters.Add("@Model", OleDbType.VarChar).Value = measurement.Model;
                                updateCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = measurement.VinNo;
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string insertSql = @"INSERT INTO Incline
                                         (Accept_No, Vin_No, Model, Inc_Angle, Inspection_Status, Ok_Ng, Mea_Date)
                                         VALUES (?, ?, ?, ?, ?, ?, ?)";
                            using (OleDbCommand insertCmd = new OleDbCommand(insertSql, con))
                            {
                                insertCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = measurement.AcceptNo;
                                insertCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = measurement.VinNo;
                                insertCmd.Parameters.Add("@Model", OleDbType.VarChar).Value = measurement.Model;
                                insertCmd.Parameters.Add("@Inc_Angle", OleDbType.Double).Value = measurement.InclineAngle;
                                insertCmd.Parameters.Add("@Inspection_Status", OleDbType.Boolean).Value = measurement.InspectionStatus;
                                insertCmd.Parameters.Add("@Ok_Ng", OleDbType.Boolean).Value = measurement.OkNg;
                                insertCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = measurement.MeaDate;
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