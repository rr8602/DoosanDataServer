using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoosanDataServer.Comm
{
    // 바코드 스캔
    public class SerialBarcodeReader
    {
        public event Action<string> BarcodeScanned;
        private SerialPort _port;

        public SerialBarcodeReader(string portName, int baudRate = 9600)
        {
            _port = new SerialPort(portName, baudRate);

            _port.DataReceived += (s, e) =>
            {
                string data = _port.ReadLine().Trim();
                BarcodeScanned?.Invoke(data);
            };
        }

        public void Open() => _port.Open();
        public void Close() => _port.Close();
    }
}
