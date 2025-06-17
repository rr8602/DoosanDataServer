using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoosanDataServer.Models
{
    public class Packet
    {
        public const char STX = '<';
        public const char ETX = '>';
        public const char DELIMETER = '/';

        // Command
        public const string CMD_WHO = "WHO";
        public const string CMD_CON = "CON";
        public const string CMD_REG = "REG";
        public const string CMD_RST = "RST";

        // source
        public const string SRC_SVR = "SVR";
        public const string SRC_WGT = "WGT";
        public const string SRC_ABS = "ABS";
        public const string SRC_HLT = "HLT";
        public const string SRC_ADAS = "ADAS";
        public const string SRC_ANG = "ANG";

        public const string RESULT_OK = "OK";

        public string CreatePacket(string command, string source, string data, int dataCoutnt)
        {
            return $"{STX}{command}{DELIMETER}{source}{DELIMETER}{data}{DELIMETER}{dataCoutnt}{ETX}";
        }

        public bool ParsePacket(string packetStr, out string command, out string source, out string data, out int dataCount)
        {
            command = string.Empty;
            source = string.Empty;
            data = string.Empty;
            dataCount = 0;

            if (string.IsNullOrEmpty(packetStr) || (packetStr[0] != STX || packetStr[packetStr.Length - 1] != ETX))
            {
                return false;
            }

            // 양 끝의 STX, ETX 제거
            string content = packetStr.Substring(1, packetStr.Length - 2);
            string[] parts = content.Split(DELIMETER);

            if (parts.Length < 4)
            {
                return false;
            }

            command = parts[0];
            source = parts[1];
            data = parts[2];

            if (!int.TryParse(parts[parts.Length - 1], out dataCount))
            {
                return false;
            }

            return true;
        }
    }
}
