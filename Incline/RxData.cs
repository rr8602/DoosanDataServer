using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incline
{
    public class RxData
    {
        private const byte PREFIX = 0x23;
        private const byte ETB = 0x17;
        private const byte SUFFIX = 0x24;

        public byte[] RawPacket { get; }
        public byte[] Data { get; }
        public string DataAsString { get; }
        public bool IsValid { get; }

        public RxData(byte[] packet)
        {
            this.RawPacket = packet;
            this.IsValid = ValidatePacket(packet);

            if (this.IsValid && packet.Length > 3)
            {
                // 데이터 부분 추출
                int dataLength = packet.Length - 3;
                this.Data = new byte[dataLength];
                Array.Copy(packet, 1, this.Data, 0, dataLength);

                // 데이터 -> 문자열 변환
                this.DataAsString = Encoding.ASCII.GetString(this.Data);
            }
            else
            {
                this.Data = new byte[0];
                this.DataAsString = string.Empty;
            }
        }

        public bool ValidatePacket(byte[] packet)
        {
            if (packet == null || packet.Length < 4)
                return false;

            return packet[0] == PREFIX && 
                   packet[packet.Length - 2] == ETB && 
                   packet[packet.Length - 1] == SUFFIX;
        }

        public string GetHexString()
        {
            return BitConverter.ToString(RawPacket).Replace("-", " ");
        }
    }
}
