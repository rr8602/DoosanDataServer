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
        private byte[] inputBuffer = new byte[6];
        private int bufferPosition = 0;
        public bool isConnected { get; private set; }

        private const byte STX = 0x02;
        private const byte ETX = 0x03;
        private const byte CMD_INPUT = 0x33;
        private const byte CMD_OUTPUT = 0x30;
        private const byte CS = 0x33;

        private const byte PREFIX = 0x23;
        private const byte ETB = 0x17;
        private const byte SUFFIX = 0x24;
        private const byte CR = 0x0D;
        private const byte LF = 0x0A;

        public delegate void DataReceivedHandler(byte data);
        public event DataReceivedHandler OnInputDataReceived;

        public delegate void OutputDataHandler(byte outputData, bool success);
        public event OutputDataHandler onOutputDataSent;

        public delegate void RxDataReceivedHandler(RxData rxData);
        public event RxDataReceivedHandler OnRxDataReceived;

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

                //serialPort.DataReceived += SerialPort_DataReceived;
                serialPort.Open();
                isConnected = true;

                MessageBox.Show("보드에 연결되었습니다.", "연결 성공", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                //serialPort.DataReceived -= SerialPort_DataReceived;
                isConnected = false;
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

        /*public bool SendCommand()
        {
            if (!isConnected || serialPort == null) return false;

            try
            {
                TxData txData = new TxData("READ");
                Debug.WriteLine($"송신 데이터: {txData.GetHexString()}");

                serialPort.Write(txData.Packet, 0, txData.Packet.Length);

                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Read 오류: {ex.Message}");

                return false;
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int bytesToRead = serialPort.BytesToRead;
                byte[] buffer = new byte[bytesToRead];
                serialPort.Read(buffer, 0, bytesToRead);

                if (bytesToRead == 6 && buffer[0] == STX && buffer[4] == ETX)
                {
                    ProcessCompletePacket(buffer);

                    return;
                }

                for (int i = 0; i < bytesToRead; i++)
                {
                    if (bufferPosition < inputBuffer.Length)
                    {
                        inputBuffer[bufferPosition++] = buffer[i];
                    }
                }

                ProcessSensorBuffer();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"데이터 수신 오류: {ex.Message}");
            }
        }

        private void ProcessSensorBuffer()
        {
            int startIndex = -1;
            int endIndex = -1;

            for (int i = 0; i < bufferPosition - 2; i++)
            {
                if (inputBuffer[i] == PREFIX)
                {
                    startIndex = i;

                    for (int j = i + 2; j < bufferPosition; j++)
                    {
                        if (inputBuffer[j] == SUFFIX && inputBuffer[j - 1] == ETB)
                        {
                            endIndex = j;

                            break;
                        }
                    }

                    if (endIndex != -1)
                        break;
                }
            }

            if (startIndex != -1 && endIndex != -1)
            {
                int packetLength = endIndex - startIndex + 1;
                byte[] packet = new byte[packetLength];
                Array.Copy(inputBuffer, startIndex, packet, 0, packetLength);

                RxData rxData = new RxData(packet);
                Debug.WriteLine($"수신 데이터: {rxData.GetHexString()}, 유효함: {rxData.IsValid}");

                if (rxData.IsValid)
                {
                    Debug.WriteLine($"데이터 내용: {rxData.DataAsString}");
                    OnRxDataReceived?.Invoke(rxData);
                }

                RemoveProcessedDataFromBuffer(endIndex + 1);
            }
        }*/

        private void RemoveProcessedDataFromBuffer(int endPos)
        {
            if (endPos >= bufferPosition)
            {
                bufferPosition = 0;

                return;
            }

            int remainingBytes = bufferPosition - endPos;
            Array.Copy(inputBuffer, endPos, inputBuffer, 0, remainingBytes);
            bufferPosition = remainingBytes;
        }

        private void ProcessCompletePacket(byte[] packet)
        {
            if (packet.Length != 6) return;

            // 패킷이 입력 명령인 경우
            if (packet[1] == CMD_INPUT)
            {
                byte calculatedSum = CalculateChecksum(new byte[] { packet[1], packet[2], packet[3] });

                if (calculatedSum == packet[5])
                {
                    byte inputData = packet[2];
                    Debug.WriteLine($"수신 데이터(HEX): {packet[0]:X2} {packet[1]:X2} {packet[2]:X2} {packet[3]:X2} {packet[4]:X2} {packet[5]:X2}");
                    OnInputDataReceived?.Invoke(inputData);
                }
                else
                {
                    Debug.WriteLine($"체크섬 오류: 계산={calculatedSum:X2}, 수신={packet[5]:X2}");
                }
            }
            // 패킷이 출력 명령인 경우
            else if (packet[1] == CMD_OUTPUT)
            {
                byte calculatedSum = CalculateChecksum(new byte[] { packet[1], packet[2], packet[3] });

                if (calculatedSum == packet[5])
                {
                    byte outputData = packet[2];
                    Debug.WriteLine($"출력 응답(HEX): {packet[0]:X2} {packet[1]:X2} {packet[2]:X2} {packet[3]:X2} {packet[4]:X2} {packet[5]:X2}");
                }
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

        // 버퍼를 한 칸씩 이동시켜 유효한 시작 바이트(STX) 찾기
        private void ShiftBuffer()
        {
            for (int i = 0; i < bufferPosition - 1; i++)
            {
                inputBuffer[i] = inputBuffer[i + 1];
            }

            bufferPosition--;
        }
    }
}
