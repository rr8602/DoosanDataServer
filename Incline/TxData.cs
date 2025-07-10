using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incline
{
    public class TxData
    {
        private const byte PREFIX = 0x23; // #
        private const byte CR = 0x0D;
        private const byte LF = 0x0A;

        private readonly byte[] commandBytes;

        public string Command { get; }
        public byte[] Packet { get; }

        public TxData(string command)
        {
            this.Command = command;
            this.commandBytes = Encoding.ASCII.GetBytes(command);

            byte[] packet = new byte[commandBytes.Length + 3];
            packet[0] = PREFIX;
            Array.Copy(commandBytes, 0, packet, 1, commandBytes.Length);
            packet[packet.Length - 2] = CR;
            packet[packet.Length - 1] = LF;

            this.Packet = packet;
        }

        public string GetHexString()
        {
            return BitConverter.ToString(Packet).Replace("-", " ");
        }
    }
}
