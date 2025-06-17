using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonIniFile;

namespace WGT
{
    public static class CreateDefaultIni
    {
        public static void CreateWgtIniFile(string ip, string port, string minWeight, string runout, string filePath = "Config\\WGT.ini")
        {
            try
            {
                IniFile iniFile = new IniFile(filePath);

                // 네트워크 설정
                iniFile.WriteValue("Network", "IP", ip);
                iniFile.WriteValue("Network", "Port", port);


                // 중량 설정 (기본값)
                iniFile.WriteValue("Weight", "MinimumWeight", minWeight);
                iniFile.WriteValue("Weight", "Runout", runout);

                Console.WriteLine($"INI 파일이 성공적으로 생성되었습니다: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"INI 파일 생성 중 오류: {ex.Message}");
            }
        }
    }
}
