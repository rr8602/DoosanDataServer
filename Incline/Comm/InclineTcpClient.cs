
using Incline.Models;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Incline.Comm
{
    public class VehicleInfoEventArgs : EventArgs
    {
        public string AcceptNo { get; }
        public string VinNo { get; }
        public string Model { get; }

        public VehicleInfoEventArgs(string acceptNo, string vinNo, string model)
        {
            AcceptNo = acceptNo;
            VinNo = vinNo;
            Model = model;
        }
    }

    public class InclineTcpClient
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        private Thread _listenThread;
        private bool _isListening;
        private readonly Packet _packetHandler = new Packet();

        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler<string> LogMessage;
        public event EventHandler<VehicleInfoEventArgs> VehicleInfoReceived;

        public bool IsConnected => _tcpClient != null && _tcpClient.Connected;

        public void Connect(string ipAddress, int port)
        {
            if (IsConnected) return;

            Task.Run(() =>
            {
                try
                {
                    _tcpClient = new TcpClient();
                    _tcpClient.Connect(ipAddress, port);

                    Application.OpenForms[0].Invoke((MethodInvoker)delegate
                    {
                        _stream = _tcpClient.GetStream();
                        _isListening = true;

                        _listenThread = new Thread(ListenForData)
                        {
                            IsBackground = true
                        };

                        _listenThread.Start();

                        OnLogMessage($"서버에 연결되었습니다. ({ipAddress}:{port})");
                        Connected?.Invoke(this, EventArgs.Empty);
                    });
                }
                catch (Exception ex)
                {
                    Application.OpenForms[0].Invoke((MethodInvoker)delegate
                    {
                        OnLogMessage($"서버 연결 실패: {ex.Message}");
                    });
                }
            });
        }

        public void Disconnect()
        {
            if (!IsConnected) return;

            try
            {
                _isListening = false;
                _stream?.Close();
                _tcpClient?.Close();
                _listenThread?.Join(1000);
            }
            catch (Exception ex)
            {
                OnLogMessage($"서버 연결 종료 실패: {ex.Message}");
            }
            finally
            {
                _tcpClient = null;
                OnLogMessage("서버 연결이 종료되었습니다.");
                Disconnected?.Invoke(this, EventArgs.Empty);
            }
        }

        private void ListenForData()
        {
            byte[] buffer = new byte[1024];

            while (_isListening)
            {
                try
                {
                    if (_stream.DataAvailable)
                    {
                        int bytesRead = _stream.Read(buffer, 0, buffer.Length);

                        if (bytesRead > 0)
                        {
                            string receivedData = Encoding.GetEncoding("EUC-KR").GetString(buffer, 0, bytesRead);
                            OnLogMessage($"수신: {receivedData}");
                            ProcessReceivedData(receivedData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    OnLogMessage($"데이터 수신 오류: {ex.Message}");
                    Disconnect();
                    break;
                }

                Thread.Sleep(100);
            }
        }

        private void ProcessReceivedData(string data)
        {
            if (data.Contains("WHO"))
            {
                string response = _packetHandler.CreatePacket(Packet.CMD_CON, Packet.SRC_ANG, Packet.RESULT_OK, 3);
                SendData(response);
                OnLogMessage("WHO 명령에 응답: 설비 인증 확인");
            }
            else if (data.Contains("REG"))
            {
                string[] result = data.Split('/');

                if (result.Length >= 5)
                {
                    string acceptNo = result[2];
                    string vinNo = result[3];
                    string model = result[4].Replace(Packet.ETX.ToString(), "").Trim();
                    VehicleInfoReceived?.Invoke(this, new VehicleInfoEventArgs(acceptNo, vinNo, model));
                }
            }
        }

        public void SendData(string data)
        {
            if (!IsConnected)
            {
                OnLogMessage("전송 실패: 서버에 연결되지 않았습니다.");
                return;
            }

            try
            {
                byte[] bytes = Encoding.GetEncoding("EUC-KR").GetBytes(data);
                _stream.Write(bytes, 0, bytes.Length);
                _stream.Flush();
                OnLogMessage($"전송: {data}");
            }
            catch (Exception ex)
            {
                OnLogMessage($"데이터 전송 실패: {ex.Message}");
                Disconnect();
            }
        }

        private void OnLogMessage(string message)
        {
            LogMessage?.Invoke(this, message);
            Debug.WriteLine($"[InclineTcpClient] {message}");
        }
    }
}
