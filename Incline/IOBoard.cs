using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Incline
{
    public class IoBoard
    {
        private SerialPort serialPort;
        public bool isConnected { get; private set; }

        private Timer communicationTimer;
        private const int DEFAULT_POLLING_INTERVAL = 100;

        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const byte CMD_INPUT = 0x33;
        private const byte CMD_OUTPUT = 0x30;
        private const byte CS = 0x33;

        public delegate void DataReceivedHandler(byte data);
        public event DataReceivedHandler OnInputDataReceived;

        public delegate void OutputDataHandler(byte outputData, bool success);
        public event OutputDataHandler onOutputDataSent;

        public IoBoard()
        {
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            communicationTimer = new Timer();
            communicationTimer.Interval = DEFAULT_POLLING_INTERVAL;
            communicationTimer.Tick += communicationTimer_Tick;
        }

        private void communicationTimer_Tick(object sender, EventArgs e)
        {
            RequestInputStatus();
        }

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

                communicationTimer.Start();

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
            communicationTimer.Stop();

            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.DataReceived -= SerialPort_DataReceived;
                serialPort.Close();
                serialPort.Dispose();
                isConnected = false;
            }
        }

        public void SetPollingInterval(int milliseconds)
        {
            if (milliseconds > 0)
            {
                communicationTimer.Interval = milliseconds;
            }
        }

        public bool IsConnected => isConnected;

        // 보드에 입력 상태 요청
        public void RequestInputStatus()
        {
            if (!isConnected || serialPort == null) return;

            byte[] message = { STX, CMD_INPUT, 0x00, 0x00, ETX, CS };

            try
            {
                serialPort.Write(message, 0, message.Length);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"데이터 전송 오류 : {ex.Message}");
            }
        }

        // 출력 상태 보드에 전송
        public void SetOutputStatus(byte outputData)
        {
            if (!isConnected || serialPort == null)
            {
                onOutputDataSent?.Invoke(outputData, false);

                return;
            }

            byte checksum = CalculateChecksum(new byte[] { CMD_OUTPUT, outputData, 0x00 });
            byte[] message = { STX, CMD_OUTPUT, outputData, 0x00, ETX, checksum };

            try
            {
                Debug.WriteLine($"전송 데이터(HEX): {message[0]:X2} {message[1]:X2} {message[2]:X2} {message[3]:X2} {message[4]:X2} {message[5]:X2}");
                serialPort.Write(message, 0, message.Length);

                onOutputDataSent?.Invoke(outputData, true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"입력 요청 오류 : {ex.Message}");
                onOutputDataSent?.Invoke(outputData, false);
            }
        }

        // 체크섬 계산
        private byte CalculateChecksum(byte[] datas)
        {
            byte sum = 0;

            foreach (byte data in datas)
            {
                sum += data;
            }

            return sum;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (!isConnected || serialPort == null) return;

            try
            {
                System.Threading.Thread.Sleep(50);

                int bytesToRead = serialPort.BytesToRead;
                byte[] buffer = new byte[bytesToRead];
                serialPort.Read(buffer, 0, bytesToRead);

                ProcessReceivedData(buffer);
            }
            catch(Exception ex)
            {
                Debug.WriteLine($"데이터 수신 오류 : {ex.Message}");
            }
        }

        private void ProcessReceivedData(byte[] buffer)
        {
            if (buffer[0] == STX && buffer[buffer.Length - 2] == ETX)
            {
                if (buffer[1] == CMD_INPUT)
                {
                    byte inputData = buffer[2];
                    Debug.WriteLine($"입력 데이터 수신: {inputData:X2}");

                    OnInputDataReceived?.Invoke(inputData);

                    if ((inputData & 0x01) != 0)
                    {
                        HandlerFirstInputActive();
                    }
                    else if ((inputData & 0x02) != 0)
                    {
                        HandlerSecondInputActive();
                    }
                    else
                    {
                        HandlerAllInputsInactive();
                    }
                }
            }
        }

        private void HandlerFirstInputActive()
        {
            byte outputValue = 0x0B;  // 0, 1, 3번 출력 활성화 (0000 1011)

            SetOutputStatus(outputValue);
            Debug.WriteLine("0번 입력 신호 감지: 0, 1, 3번 출력 활성화");
        }

        private void HandlerSecondInputActive()
        {
            byte outputValue = 0x0D;  // 0, 2, 3번 출력 활성화 (0000 1101)

            SetOutputStatus(outputValue);
            Debug.WriteLine("1번 입력 신호 감지: 0, 2, 3번 출력 활성화");
        }

        private void HandlerAllInputsInactive()
        {
            byte outputValue = 0x00;  // 모든 출력 비활성화 (0000 0000)

            SetOutputStatus(outputValue);
            Debug.WriteLine("입력 신호 없음: 모든 출력 비활성화");
        }
    }
}
