using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonIniFile;

namespace Incline
{
    public class CreateDefaultIni
    {
        public static void CreateInclineIniFile(string ip, string port, string filePath = "Config\\Incline.ini")
        {
            try
            {
                IniFile iniFile = new IniFile(filePath);

                // 네트워크 설정
                iniFile.WriteValue("Network", "IP", ip);
                iniFile.WriteValue("Network", "Port", port);

                Console.WriteLine($"INI 파일이 성공적으로 생성되었습니다: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"INI 파일 생성 중 오류: {ex.Message}");
            }
        }
    }
}
