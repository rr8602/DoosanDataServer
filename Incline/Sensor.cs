using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Incline
{
    public class Sensor
    {
        private SerialPort serialPort;
        private byte[] inputBuffer = new byte[1024];
        private int bufferPosition = 0;
        public bool isConnected { get; private set; }

        private const byte STX = 0x02;
        private const byte ETX = 0x04;
        private const byte CMD_INPUT = 0x33;
        private const byte CMD_OUTPUT = 0x30;
        private const byte CS = 0x33;

        private const byte PREFIX = 0x23;
        private const byte ETB = 0x17;
        private const byte SUFFIX = 0x24;
        private const byte CR = 0x0D;
        private const byte LF = 0x0A;

        // 각도값 저장 변수
        private double gatAngle = 0.0;
        public double GatAngle => gatAngle;

        private int getC = 0;
        private double getT = 0;

        public delegate void DataReceivedHandler(byte data);
        public event DataReceivedHandler OnInputDataReceived;

        public delegate void RxDataReceivedHandler(RxData rxData);
        public event RxDataReceivedHandler OnRxDataReceived;

        // 각도값 변경 이벤트
        public delegate void AngleChangedHandler(double angle);
        public event AngleChangedHandler OnAngleChanged;

        public bool Connect(string portName, int baudRate = 9600)
        {
            try
            {
                serialPort = new SerialPort(portName, baudRate)
                {
                    DataBits = 8,
                    Parity = Parity.None,
                    StopBits = StopBits.One,
                    ReadTimeout = 1000,
                    WriteTimeout = 1000
                };

                serialPort.DataReceived += SerialPort_DataReceived;
                serialPort.Open();
                isConnected = true;

                MessageBox.Show("센서에 연결되었습니다.", "연결 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"연결 오류 : {ex.Message}");

                return false;
            }
        }

        public void Disconnect()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort.Dispose();
                serialPort.DataReceived -= SerialPort_DataReceived;
                isConnected = false;
            }
        }

        public bool IsConnected => isConnected;

        // Sensor 데이터 요청
        public bool SendCommand()
        {
            if (!isConnected || serialPort == null) return false;

            try
            {
                // #READ + CR + LF 형식으로 명령 구성
                byte[] commandBytes = new byte[7]; // # + R + E + A + D + CR + LF
                commandBytes[0] = PREFIX; // #
                commandBytes[1] = (byte)'R';
                commandBytes[2] = (byte)'E';
                commandBytes[3] = (byte)'A';
                commandBytes[4] = (byte)'D';
                commandBytes[5] = CR; // 0x0D
                commandBytes[6] = LF; // 0x0A

                Debug.WriteLine($"송신 데이터: {BitConverter.ToString(commandBytes).Replace("-", " ")}");

                serialPort.Write(commandBytes, 0, commandBytes.Length);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Read 오류: {ex.Message}");

                return false;
            }
        }

        public void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int bytesToRead = serialPort.BytesToRead;
                byte[] buffer = new byte[bytesToRead];
                serialPort.Read(buffer, 0, bytesToRead);

                Debug.WriteLine($"수신 바이트 수: {bytesToRead}");
                Debug.WriteLine($"원시 데이터: {BitConverter.ToString(buffer)}");

                if (bufferPosition + bytesToRead > inputBuffer.Length)
                {
                    Debug.WriteLine("버퍼 초기화 - 오버플로우 방지");
                    bufferPosition = 0;
                }

                Array.Copy(buffer, 0, inputBuffer, bufferPosition, bytesToRead);
                bufferPosition += bytesToRead;

                ProcessSensorBuffer();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"데이터 수신 오류: {ex.Message}");
            }
        }

        public void ProcessSensorBuffer()
        {
            string buffAsString = Encoding.ASCII.GetString(inputBuffer, 0, bufferPosition);
            int sttP = buffAsString.IndexOf((char)STX);
            int endP = buffAsString.IndexOf('$', sttP > 0 ? sttP : 0);

            if (sttP >= 0 && endP > sttP)
            {
                string strBuff = buffAsString.Substring(sttP + 1, (endP - sttP) - 1);
                string[] angles = strBuff.Split((char)ETB);

                Debug.WriteLine($"추출된 데이터: {strBuff}");
                Debug.WriteLine($"분할된 각도 값: {string.Join(", ", angles)}");

                if (angles.Length > 0)
                {
                    if (double.TryParse(angles[0], out double angle))
                    {
                        gatAngle = angle * -1;

                        Debug.WriteLine($"각도값: {gatAngle}");

                        getC++;

                        if (Math.Abs(Environment.TickCount / 1000.0 - getT) >= 1)
                        {
                            getT = Environment.TickCount / 1000.0;
                            Debug.WriteLine($"1초간 수신 횟수: {getC}");
                            getC = 0;
                        }

                        OnAngleChanged?.Invoke(gatAngle);
                    }

                    int packetLength = endP - sttP + 1;
                    byte[] packet = new byte[packetLength];
                    Array.Copy(inputBuffer, sttP, packet, 0, packetLength);

                    RxData rxData = new RxData(packet);

                    if (rxData.IsValid)
                    {
                        OnRxDataReceived?.Invoke(rxData);
                    }
                }

                bufferPosition = 0;

                if (endP + 1 < buffAsString.Length)
                {
                    string remaining = buffAsString.Substring(endP + 1);
                    byte[] remainingBytes = Encoding.ASCII.GetBytes(remaining);
                    Array.Copy(remainingBytes, 0, inputBuffer, 0, remainingBytes.Length);
                    bufferPosition = remainingBytes.Length;
                }
            }
        }
    }
}