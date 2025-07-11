using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace Incline
{
    public class TxData
    {
        private const byte STX = 0x02; // 시작 문자
        private const byte DELIMITER = 0x17; // 구분자
        private const byte END_MARKER = 0x24; // $ 종료 문자

        private readonly byte[] commandBytes;

        public string Command { get; }
        public byte[] Packet { get; }

        public TxData(string command)
        {
            this.Command = command;
            byte[] commandBytes = Encoding.ASCII.GetBytes(command);

            byte[] packet = new byte[commandBytes.Length + 2]; // STX + 명령어 + $ 종료 마커
            packet[0] = STX;
            Array.Copy(commandBytes, 0, packet, 1, commandBytes.Length);
            packet[packet.Length - 1] = END_MARKER;

            this.Packet = packet;
        }

        // 각도 데이터를 포함한 패킷을 생성하는 생성자 추가
        public TxData(params double[] angles)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < angles.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append((char)DELIMITER);
                }
                sb.Append(angles[i].ToString());
            }

            this.Command = sb.ToString();
            byte[] commandBytes = Encoding.ASCII.GetBytes(Command);

            byte[] packet = new byte[commandBytes.Length + 2]; // STX + 데이터 + $ 종료 마커
            packet[0] = STX;
            Array.Copy(commandBytes, 0, packet, 1, commandBytes.Length);
            packet[packet.Length - 1] = END_MARKER;

            this.Packet = packet;
        }

        public string GetHexString()
        {
            return BitConverter.ToString(Packet).Replace("-", " ");
        }
    }
}
