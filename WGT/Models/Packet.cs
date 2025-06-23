using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WGT.Models
{
    public class Packet
    {
        public const char STX = '<';
        public const char ETX = '>';
        public const char DELIMETER = '/';

        public const string CMD_WHO = "WHO";
        public const string CMD_CON = "CON";
        public const string CMD_REG = "REG";

        public const string SRC_SVR = "SVR";
        public const string SRC_WGT = "WGT";

        public const string RESULT_OK = "OK";

        public string CreatePacket(string command, string source, string data, int dataCount)
        {
            return $"{STX}{command}{DELIMETER}{source}{DELIMETER}{data}{DELIMETER}{dataCount}{ETX}";
        }

        public bool ParsePacket(string packetStr, out string command, out string source, out string data, out string dataCount)
        {
            command = string.Empty;
            source = string.Empty;
            data = string.Empty;
            dataCount = string.Empty;

            if (string.IsNullOrEmpty(packetStr) || packetStr[0] != STX || packetStr[packetStr.Length - 1] != ETX)
            {
                return false;
            }

            string content = packetStr.Substring(1, packetStr.Length - 2);
            string[] parts = content.Split(DELIMETER);

            if (parts.Length < 3)
            {
                return false;
            }

            command = parts[0];
            source = parts[1];
            data = parts[2];

            if (!int.TryParse(parts[parts.Length - 1], out int count))
            {
                return false;
            }

            return true;
        }
    }
}
