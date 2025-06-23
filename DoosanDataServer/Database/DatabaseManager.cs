using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Data;
using System.Drawing;

namespace DoosanDataServer.Database
{
    public class DatabaseManager
    {
        public string ConnectionString { get; private set; }

        public DatabaseManager()
        {
            SetupDatabaseConnection();
        }

        // DB 연결 설정
        private void SetupDatabaseConnection()
        {
            try
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["AccessConnection"].ConnectionString;

                AppDomain.CurrentDomain.SetData("DataDirectory", Application.StartupPath);

                if (string.IsNullOrEmpty(ConnectionString))
                {
                    string dbPath = Path.Combine(Application.StartupPath, "Server_Data.accdb");

                    if (File.Exists(dbPath))
                    {
                        ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";
                    }
                    else
                    {
                        throw new FileNotFoundException("데이터베이스 파일을 찾을 수 없습니다.", dbPath);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터베이스 연결 설정 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 접수번호로 데이터 존재 여부 확인
        private bool DoesRecordExist(string tableName, string acceptNo)
        {
            try
            {
                using (OleDbConnection con = new OleDbConnection(ConnectionString))
                {
                    con.Open();

                    string checkSql = $"SELECT COUNT(*) FROM {tableName} WHERE Accept_No = ?";

                    using (OleDbCommand checkCmd = new OleDbCommand(checkSql, con))
                    {
                        checkCmd.Parameters.Add("@AcceptNo", OleDbType.VarChar).Value = acceptNo;

                        return (int)checkCmd.ExecuteScalar() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"레코드 존재 여부 확인 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
        }

        // 경사각도 데이터 저장
        public void SaveAngleData(string acceptNo, string vinNo,  string incAngle)
        {
            if (string.IsNullOrEmpty(acceptNo))
            {
                MessageBox.Show("접수번호가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            try
            {
                double angle;

                if (!double.TryParse(incAngle, out angle))
                {
                    MessageBox.Show("유효한 경사각도 값이 아닙니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }

                using (OleDbConnection con = new OleDbConnection(ConnectionString))
                {
                    con.Open();

                    if (DoesRecordExist("Angle", acceptNo))
                    {
                        string updateSql = @"UPDATE Angle
                                            SET Inc_Angle = ?,
                                                Vin_No = ?,
                                                Mea_Date = ?
                                            WHERE Accept_No = ?";

                        using (OleDbCommand updateCmd = new OleDbCommand(updateSql, con))
                        {
                            updateCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                            updateCmd.Parameters.Add("@Inc_Angle", OleDbType.Double).Value = angle;
                            updateCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = vinNo;
                            updateCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertSql = @"INSERT INTO Angle
                                            (Accept_No, Vin_No, Inc_Angle, Mea_Date)
                                            VALUES
                                            (?, ?, ?, ?)";
                        using (OleDbCommand insertCmd = new OleDbCommand(insertSql, con))
                        {
                            insertCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                            insertCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = vinNo;
                            insertCmd.Parameters.Add("@Inc_Angle", OleDbType.Double).Value = angle;
                            insertCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"경사각도 데이터 저장 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 중량계 데이터 저장
        public void SaveWeightData(string acceptNo, string vinNo, string flw, string frw, string ftw, string rlw, string rrw, string rtw)
        {
            if (string.IsNullOrEmpty(acceptNo))
            {
                MessageBox.Show("접수번호가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            try
            {
                double frontLeftWeight = ParseDoubleOrDefault(flw);
                double frontRightWeight = ParseDoubleOrDefault(frw);
                double frontTotalWeight = ParseDoubleOrDefault(ftw);
                double rearLeftWeight = ParseDoubleOrDefault(rlw);
                double rearRightWeight = ParseDoubleOrDefault(rrw);
                double rearTotalWeight = ParseDoubleOrDefault(rtw);
                double totalWeight = frontTotalWeight + rearTotalWeight;

                using (OleDbConnection con = new OleDbConnection(ConnectionString))
                {
                    con.Open();

                    if (DoesRecordExist("Weight", acceptNo))
                    {
                        string updateSql = @"UPDATE Weight
                                            SET F_LW = ?,
                                                F_RW = ?,
                                                F_TW = ?,
                                                R_LW = ?,
                                                R_RW = ?,
                                                R_TW = ?,
                                                Total_Weight = ?,
                                                Mea_Date = ?,
                                                Vin_No = ?
                                            WHERE Accept_No = ?";

                        using (OleDbCommand updateCmd = new OleDbCommand(updateSql, con))
                        {
                            updateCmd.Parameters.Add("@F_LW", OleDbType.Double).Value = frontLeftWeight;
                            updateCmd.Parameters.Add("@F_RW", OleDbType.Double).Value = frontRightWeight;
                            updateCmd.Parameters.Add("@F_TW", OleDbType.Double).Value = frontTotalWeight;
                            updateCmd.Parameters.Add("@R_LW", OleDbType.Double).Value = rearLeftWeight;
                            updateCmd.Parameters.Add("@R_RW", OleDbType.Double).Value = rearRightWeight;
                            updateCmd.Parameters.Add("@R_TW", OleDbType.Double).Value = rearTotalWeight;
                            updateCmd.Parameters.Add("@Total_Weight", OleDbType.Double).Value = totalWeight;
                            updateCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                            updateCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = vinNo;
                            updateCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertSql = @"INSERT INTO Weight 
                                            (Accept_No, Vin_No, F_LW, F_RW, F_TW, R_LW, R_RW, R_TW, Total_Weight, Mea_Date)
                                            VALUES
                                            (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                        using (OleDbCommand insertCmd = new OleDbCommand(insertSql, con))
                        {
                            insertCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                            insertCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = vinNo;
                            insertCmd.Parameters.Add("@F_LW", OleDbType.Double).Value = frontLeftWeight;
                            insertCmd.Parameters.Add("@F_RW", OleDbType.Double).Value = frontRightWeight;
                            insertCmd.Parameters.Add("@F_TW", OleDbType.Double).Value = frontTotalWeight;
                            insertCmd.Parameters.Add("@R_LW", OleDbType.Double).Value = rearLeftWeight;
                            insertCmd.Parameters.Add("@R_RW", OleDbType.Double).Value = rearRightWeight;
                            insertCmd.Parameters.Add("@R_TW", OleDbType.Double).Value = rearTotalWeight;
                            insertCmd.Parameters.Add("@Total_Weight", OleDbType.Double).Value = totalWeight;
                            insertCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"중량계 데이터 저장 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 헤드라이트 제이터 저장
        public void SaveHeadLightData(string acceptNo, string vinNo, string beamType,
            string lcd, string lcdResult,
            string lud, string ludResult,
            string llr, string llrResult,
            string rcd, string rcdResult,
            string rud, string rudResult,
            string rlr, string rlrResult,
            string totalResult)
        {
            if (string.IsNullOrEmpty(acceptNo))
            {
                MessageBox.Show("접수번호가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            try
            {
                using (OleDbConnection con = new OleDbConnection(ConnectionString))
                {
                    con.Open();

                    if (DoesRecordExist("HeadLight", acceptNo))
                    {
                        string updateSql = @"UPDATE HeadLight
                                            SET Beam_Type = ?,
                                                L_CD = ?,
                                                L_CD_Result = ?,
                                                L_UD = ?,
                                                L_UD_Result = ?,
                                                L_LR = ?,
                                                L_LR_Result = ?,
                                                R_CD = ?,
                                                R_CD_Result = ?,
                                                R_UD = ?,
                                                R_UD_Result = ?,
                                                R_LR = ?,
                                                R_LR_Result = ?,
                                                Total_Result = ?,
                                                Mea_Date = ?,
                                                Vin_No = ?
                                            WHERE Accept_No = ?";

                        using (OleDbCommand updateCmd = new OleDbCommand(updateSql, con))
                        {
                            updateCmd.Parameters.Add("@Beam_Type", OleDbType.VarChar).Value = beamType;
                            updateCmd.Parameters.Add("@L_CD", OleDbType.Double).Value = lcd;
                            updateCmd.Parameters.Add("@L_CD_Result", OleDbType.VarChar).Value = lcdResult;
                            updateCmd.Parameters.Add("@L_UD", OleDbType.Double).Value = lud;
                            updateCmd.Parameters.Add("@L_UD_Result", OleDbType.VarChar).Value = ludResult;
                            updateCmd.Parameters.Add("@L_LR", OleDbType.Double).Value = llr;
                            updateCmd.Parameters.Add("@L_LR_Result", OleDbType.VarChar).Value = llrResult;
                            updateCmd.Parameters.Add("@R_CD", OleDbType.Double).Value = rcd;
                            updateCmd.Parameters.Add("@R_CD_Result", OleDbType.VarChar).Value = rcdResult;
                            updateCmd.Parameters.Add("@R_UD", OleDbType.Double).Value = rud;
                            updateCmd.Parameters.Add("@R_UD_Result", OleDbType.VarChar).Value = rudResult;
                            updateCmd.Parameters.Add("@R_LR", OleDbType.Double).Value = rlr;
                            updateCmd.Parameters.Add("@R_LR_Result", OleDbType.VarChar).Value = rlrResult;
                            updateCmd.Parameters.Add("@Total_Result", OleDbType.VarChar).Value = totalResult;
                            updateCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                            updateCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = vinNo;
                            updateCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertSql = @"INSERT INTO HeadLight
                                     (Accept_No, Vin_No, Mea_Date, Beam_Type, 
                                      L_CD, L_CD_Result, L_UD, L_UD_Result, L_LR, L_LR_Result,
                                      R_CD, R_CD_Result, R_UD, R_UD_Result, R_LR, R_LR_Result,
                                      Total_Result)
                                     VALUES
                                     (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";

                        using (OleDbCommand insertCmd = new OleDbCommand(insertSql, con))
                        {
                            insertCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                            insertCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = vinNo;
                            insertCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                            insertCmd.Parameters.Add("@Beam_Type", OleDbType.VarChar).Value = beamType;
                            insertCmd.Parameters.Add("@L_CD", OleDbType.Double).Value = lcd;
                            insertCmd.Parameters.Add("@L_CD_Result", OleDbType.VarChar).Value = lcdResult;
                            insertCmd.Parameters.Add("@L_UD", OleDbType.Double).Value = lud;
                            insertCmd.Parameters.Add("@L_UD_Result", OleDbType.VarChar).Value = ludResult;
                            insertCmd.Parameters.Add("@L_LR", OleDbType.Double).Value = llr;
                            insertCmd.Parameters.Add("@L_LR_Result", OleDbType.VarChar).Value = llrResult;
                            insertCmd.Parameters.Add("@R_CD", OleDbType.Double).Value = rcd;
                            insertCmd.Parameters.Add("@R_CD_Result", OleDbType.VarChar).Value = rcdResult;
                            insertCmd.Parameters.Add("@R_UD", OleDbType.Double).Value = rud;
                            insertCmd.Parameters.Add("@R_UD_Result", OleDbType.VarChar).Value = rudResult;
                            insertCmd.Parameters.Add("@R_LR", OleDbType.Double).Value = rlr;
                            insertCmd.Parameters.Add("@R_LR_Result", OleDbType.VarChar).Value = rlrResult;
                            insertCmd.Parameters.Add("@Total_Result", OleDbType.VarChar).Value = totalResult;
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"헤드라이트 데이터 저장 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 브레이크 데이터 저장
        public void SaveBrakeData(string acceptNo, string vinNo, string leftBrake, string rightBrake, string totalResult)
        {
            if (string.IsNullOrEmpty(acceptNo))
            {
                MessageBox.Show("접수번호가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                double left = ParseDoubleOrDefault(leftBrake);
                double right = ParseDoubleOrDefault(rightBrake);

                using (OleDbConnection con = new OleDbConnection(ConnectionString))
                {
                    con.Open();

                    if (DoesRecordExist("Brake", acceptNo))
                    {
                        string updateSql = @"UPDATE Brake
                                     SET Mea_Date = ?, 
                                         Left_Brake = ?,
                                         Right_Brake = ?,
                                         Total_Result = ?,
                                         Vin_No = ?
                                     WHERE Accept_No = ?";

                        using (OleDbCommand updateCmd = new OleDbCommand(updateSql, con))
                        {
                            updateCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                            updateCmd.Parameters.Add("@Left_Brake", OleDbType.Double).Value = left;
                            updateCmd.Parameters.Add("@Right_Brake", OleDbType.Double).Value = right;
                            updateCmd.Parameters.Add("@Total_Result", OleDbType.VarChar).Value = totalResult;
                            updateCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = vinNo;
                            updateCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertSql = @"INSERT INTO Brake
                                     (Accept_No, Vin_No, Mea_Date, Left_Brake, Right_Brake, Total_Result)
                                     VALUES
                                     (?, ?, ?, ?, ?, ?)";

                        using (OleDbCommand insertCmd = new OleDbCommand(insertSql, con))
                        {
                            insertCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                            insertCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                            insertCmd.Parameters.Add("@Left_Brake", OleDbType.Double).Value = left;
                            insertCmd.Parameters.Add("@Right_Brake", OleDbType.Double).Value = right;
                            insertCmd.Parameters.Add("@Total_Result", OleDbType.VarChar).Value = totalResult;
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }

                Console.WriteLine($"브레이크 데이터 저장 완료 - 접수번호: {acceptNo}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"브레이크 데이터 저장 중 오류: {ex.Message}", "저장 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 속도계 데이터 저장
        public void SaveSpeedData(string acceptNo, string vinNo, string measuredSpeed, string actualSpeed, string result)
        {
            if (string.IsNullOrEmpty(acceptNo))
            {
                MessageBox.Show("접수번호가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                double measured = ParseDoubleOrDefault(measuredSpeed);
                double actual = ParseDoubleOrDefault(actualSpeed);

                using (OleDbConnection con = new OleDbConnection(ConnectionString))
                {
                    con.Open();

                    if (DoesRecordExist("Speed", acceptNo))
                    {
                        string updateSql = @"UPDATE Speed
                                     SET Mea_Date = ?, 
                                         Measured_Speed = ?,
                                         Actual_Speed = ?,
                                         Result = ?,
                                         Vin_No = ?
                                     WHERE Accept_No = ?";

                        using (OleDbCommand updateCmd = new OleDbCommand(updateSql, con))
                        {
                            updateCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                            updateCmd.Parameters.Add("@Measured_Speed", OleDbType.Double).Value = measured;
                            updateCmd.Parameters.Add("@Actual_Speed", OleDbType.Double).Value = actual;
                            updateCmd.Parameters.Add("@Result", OleDbType.VarChar).Value = result;
                            updateCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = vinNo;
                            updateCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                            updateCmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        string insertSql = @"INSERT INTO Speed
                                     (Accept_No, Vin_No Mea_Date, Measured_Speed, Actual_Speed, Result)
                                     VALUES
                                     (?, ?, ?, ?, ?, ?)";

                        using (OleDbCommand insertCmd = new OleDbCommand(insertSql, con))
                        {
                            insertCmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;
                            insertCmd.Parameters.Add("@Vin_No", OleDbType.VarChar).Value = vinNo;
                            insertCmd.Parameters.Add("@Mea_Date", OleDbType.Date).Value = DateTime.Now;
                            insertCmd.Parameters.Add("@Measured_Speed", OleDbType.Double).Value = measured;
                            insertCmd.Parameters.Add("@Actual_Speed", OleDbType.Double).Value = actual;
                            insertCmd.Parameters.Add("@Result", OleDbType.VarChar).Value = result;
                            insertCmd.ExecuteNonQuery();
                        }
                    }
                }

                Console.WriteLine($"속도계 데이터 저장 완료 - 접수번호: {acceptNo}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"속도계 데이터 저장 중 오류: {ex.Message}", "저장 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private double ParseDoubleOrDefault(string value, double defaultValue = 0.0)
        {
            double result;

            return double.TryParse(value, out result) ? result : defaultValue;
        }

        public Dictionary<string, object> GetAllDataByAcceptNo(string acceptNo)
        {
            if (string.IsNullOrEmpty(acceptNo))
            {
                throw new ArgumentException("접수번호가 비어 있습니다.");
            }

            var result = new Dictionary<string, object>();

            try
            {
                using (OleDbConnection con = new OleDbConnection(ConnectionString))
                {
                    con.Open();

                    // 경사각도 데이터 조회
                    GetAngleData(con, acceptNo, result);

                    // 중량계 데이터 조회
                    GetWeightData(con, acceptNo, result);

                    // 헤드라이트 데이터 조회
                    GetHeadLightData(con, acceptNo, result);

                    // 브레이크 데이터 조회
                    GetBrakeData(con, acceptNo, result);

                    // 속도계 데이터 조회
                    GetSpeedData(con, acceptNo, result);
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터 조회 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return new Dictionary<string, object>();
            }
        }

        private void GetAngleData(OleDbConnection con, string acceptNo, Dictionary<string, object> result)
        {
            try
            {
                string sql = "SELECT * FROM Angle WHERE Accept_No = ?";

                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result["Angle_Mea_Date"] = reader["Mea_Date"];
                            result["Inc_Angle"] = reader["Inc_Angle"];
                        }
                    }
                }
            }
            catch (OleDbException)
            {
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"경사각도 데이터 조회 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }   
        }

        private void GetWeightData(OleDbConnection con, string acceptNo, Dictionary<string, object> result)
        {
            try
            {
                string sql = "SELECT * FROM Weight WHERE Accept_No = ?";

                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result["Weight_Mea_Date"] = reader["Mea_Date"];
                            result["F_LW"] = reader["F_LW"];
                            result["F_RW"] = reader["F_RW"];
                            result["F_TW"] = reader["F_TW"];
                            result["R_LW"] = reader["R_LW"];
                            result["R_RW"] = reader["R_RW"];
                            result["R_TW"] = reader["R_TW"];
                            result["Total_Weight"] = reader["Total_Weight"];
                        }
                    }
                }
            }
            catch (OleDbException)
            {
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("중량계 데이터 조회 중 오류 발생.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetHeadLightData(OleDbConnection con, string acceptNo, Dictionary<string, object> result)
        {
            try
            {
                string sql = "SELECT * FROM HeadLight WHERE Accept_No = ?";

                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result["HeadLight_Mea_Date"] = reader["Mea_Date"];
                            result["Beam_Type"] = reader["Beam_Type"];
                            result["L_CD"] = reader["L_CD"];
                            result["L_CD_Result"] = reader["L_CD_Result"];
                            result["L_UD"] = reader["L_UD"];
                            result["L_UD_Result"] = reader["L_UD_Result"];
                            result["L_LR"] = reader["L_LR"];
                            result["L_LR_Result"] = reader["L_LR_Result"];
                            result["R_CD"] = reader["R_CD"];
                            result["R_CD_Result"] = reader["R_CD_Result"];
                            result["R_UD"] = reader["R_UD"];
                            result["R_UD_Result"] = reader["R_UD_Result"];
                            result["R_LR"] = reader["R_LR"];
                            result["R_LR_Result"] = reader["R_LR_Result"];
                            result["HeadLight_Total_Result"] = reader["Total_Result"];
                        }
                    }
                }
            }
            catch (OleDbException)
            {
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("헤드라이트 데이터 조회 중 오류 발생.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetBrakeData(OleDbConnection con, string acceptNo, Dictionary<string, object> result)
        {
            try
            {
                string sql = "SELECT * FROM Brake WHERE Accept_No = ?";

                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result["Brake_Mea_Date"] = reader["Mea_Date"];
                            result["Left_Brake"] = reader["Left_Brake"];
                            result["Right_Brake"] = reader["Right_Brake"];
                            result["Brake_Total_Result"] = reader["Total_Result"];
                        }
                    }
                }
            }
            catch (OleDbException)
            {
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("브레이크 데이터 조회 중 오류 발생.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GetSpeedData(OleDbConnection con, string acceptNo, Dictionary<string, object> result)
        {
            try
            {
                string sql = "SELECT * FROM Speed WHERE Accept_No = ?";

                using (OleDbCommand cmd = new OleDbCommand(sql, con))
                {
                    cmd.Parameters.Add("@Accept_No", OleDbType.VarChar).Value = acceptNo;

                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            result["Speed_Mea_Date"] = reader["Mea_Date"];
                            result["Measured_Speed"] = reader["Measured_Speed"];
                            result["Actual_Speed"] = reader["Actual_Speed"];
                            result["Speed_Result"] = reader["Result"];
                        }
                    }
                }
            }
            catch (OleDbException)
            {
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("속도계 데이터 조회 중 오류 발생.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 모든 데이터 가져오기
        public DataTable GetAllAcceptNumbers()
        {
            DataTable result = new DataTable();
            result.Columns.Add("Accept_No", typeof(string));
            result.Columns.Add("Vin_No", typeof(string));
            result.Columns.Add("Mea_Date", typeof(DateTime));

            try
            {
                using (OleDbConnection con = new OleDbConnection(ConnectionString))
                {
                    con.Open();

                    // 중복 제거
                    HashSet<string> acceptNumbers = new HashSet<string>();
                    Dictionary<string, DateTime> dateDict = new Dictionary<string, DateTime>();
                    Dictionary<string, string> vehicleDict = new Dictionary<string, string>();

                    string[] tables = { "Angle", "Weight", "HeadLight", "Brake", "Speed" };

                    foreach (var table in tables)
                    {
                        try
                        {
                            string sql = $"SELECT Accept_No, Vin_No, Mea_Date FROM {table}";

                            using (OleDbCommand cmd = new OleDbCommand(sql, con))
                            {
                                using (OleDbDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string acceptNo = reader["Accept_No"].ToString();
                                        acceptNumbers.Add(acceptNo);

                                        if (!reader.IsDBNull(reader.GetOrdinal("Vin_No"))){
                                            string vinNo = reader["Vin_No"].ToString();

                                            if (!string.IsNullOrEmpty(vinNo))
                                            {
                                                vehicleDict[acceptNo] = vinNo;
                                            }
                                        }

                                        DateTime meaDate = Convert.ToDateTime(reader["Mea_Date"]);

                                        if (!dateDict.ContainsKey(acceptNo) || dateDict[acceptNo] < meaDate)
                                        {
                                            dateDict[acceptNo] = meaDate;
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    foreach (var acceptNo in acceptNumbers)
                    {
                        DataRow row = result.NewRow();
                        row["Accept_No"] = acceptNo;

                        // 차량번호 설정
                        if (vehicleDict.ContainsKey(acceptNo))
                        {
                            row["Vin_No"] = vehicleDict[acceptNo];
                        }
                        else
                        {
                            row["Vin_No"] = "";
                        }

                        // 측정일자 설정
                        if (dateDict.ContainsKey(acceptNo))
                        {
                            row["Mea_Date"] = dateDict[acceptNo];
                        }

                        result.Rows.Add(row);
                    }

                    result.DefaultView.Sort = "Mea_Date DESC"; // 날짜 기준으로 내림차순 정렬
                    result = result.DefaultView.ToTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"접수번호 조회 중 오류 발생: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }

        // 날짜 또는 접수번호로 필터링하여 데이터 검색
        public DataTable SearchAcceptNumbers(DateTime? date = null, string searchText = null)
        {
            DataTable result = new DataTable();
            result.Columns.Add("Accept_No", typeof(string));
            result.Columns.Add("Vin_No", typeof(string));
            result.Columns.Add("Mea_Date", typeof(DateTime));

            try
            {
                using (OleDbConnection con = new OleDbConnection(ConnectionString))
                {
                    con.Open();

                    HashSet<string> acceptNumbers = new HashSet<string>();
                    Dictionary<string, DateTime> dateDict = new Dictionary<string, DateTime>();
                    Dictionary<string, string> vehicleDict = new Dictionary<string, string>();

                    string[] tables = { "Angle", "Weight", "HeadLight", "Brake", "Speed" };

                    foreach (var table in tables)
                    {
                        try
                        {
                            List<string> conditions = new List<string>();

                            if (!string.IsNullOrEmpty(searchText))
                            {
                                conditions.Add("Vin_No like ?");
                            }

                            if (date.HasValue)
                            {
                                conditions.Add("Mea_Date >= ? AND Mea_Date < ?");
                            }

                            string whereClause = conditions.Count > 0 ? " WHERE " + string.Join(" AND ", conditions) : "";
                            string sql = $"SELECT Accept_No, Vin_No, Mea_Date FROM {table}{whereClause}";

                            using (OleDbCommand cmd = new OleDbCommand(sql, con))
                            {
                                int paramIndex = 0;

                                if (!string.IsNullOrEmpty(searchText))
                                {
                                    cmd.Parameters.AddWithValue($"p{paramIndex++}", $"%{searchText}%");
                                }

                                if (date.HasValue)
                                {
                                    DateTime startDate = date.Value.Date;
                                    DateTime endDate = startDate.AddDays(1);
                                    cmd.Parameters.AddWithValue($"p{paramIndex++}", startDate);
                                    cmd.Parameters.AddWithValue($"p{paramIndex++}", endDate);
                                }

                                using (OleDbDataReader reader = cmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        string acceptNo = reader["Accept_No"].ToString();
                                        acceptNumbers.Add(acceptNo);

                                        if (!reader.IsDBNull(reader.GetOrdinal("Vin_No")))
                                        {
                                            string vinNo = reader["Vin_No"].ToString();

                                            if (!string.IsNullOrEmpty(vinNo))
                                            {
                                                vehicleDict[acceptNo] = vinNo;
                                            }
                                        }

                                        DateTime meaDate = Convert.ToDateTime(reader["Mea_Date"]);

                                        if (!dateDict.ContainsKey(acceptNo) || dateDict[acceptNo] < meaDate)
                                        {
                                            dateDict[acceptNo] = meaDate;
                                        }
                                    }
                                }
                            }
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    foreach (var acceptNo in acceptNumbers)
                    {
                        DataRow row = result.NewRow();
                        row["Accept_No"] = acceptNo;

                        // 차량번호 설정
                        if (vehicleDict.ContainsKey(acceptNo))
                        {
                            row["Vin_No"] = vehicleDict[acceptNo];
                        }
                        else
                        {
                            row["Vin_No"] = "";
                        }

                        // 측정일자 설정
                        if (dateDict.ContainsKey(acceptNo))
                        {
                            row["Mea_Date"] = dateDict[acceptNo];
                        }
                        else
                        {
                            // 측정일자가 없는 경우 현재 시간으로 설정
                            row["Mea_Date"] = DateTime.Now;
                        }

                        result.Rows.Add(row);
                    }

                    result.DefaultView.Sort = "Mea_Date DESC";
                    result = result.DefaultView.ToTable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"접수번호 검색 중 오류 발생: {ex.Message}", "검색 오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return result;
        }
    }
}
