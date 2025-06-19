using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using WGT.Comm;
using WGT.Models;

namespace WGT.Comm
{
    public class PacketReceivedEventArgs : EventArgs
    {
        public string Command { get; set; }
        public string Source { get; set; }
        public string Data { get; set; }
        public string DataCount { get; set; }
        public TcpClient Client { get; set; }

        public PacketReceivedEventArgs(string command, string source, string data, string dataCount, TcpClient client)
        {
            Command = command;
            Source = source;
            Data = data;
            DataCount = dataCount;
            Client = client;
        }
    }

    public class VehicleInfoEventArgs : EventArgs
    {
        public string ChassisNumber { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public VehicleInfoEventArgs(string chassisNumber, string model, string manufacturer)
        {
            ChassisNumber = chassisNumber;
            Model = model;
            Manufacturer = manufacturer ?? string.Empty;
        }
    }

    public class TcpServer
    {
        private TcpListener _listener;
        private bool _isRunning;
        private Thread _listenThread;
        private readonly Packet _packetHandler =new Packet();
        private readonly object _clientsLock = new object();
        private readonly List<TcpClient> _clients = new List<TcpClient>();

        public event EventHandler<PacketReceivedEventArgs> PacketReceived;
        public event EventHandler<VehicleInfoEventArgs> VehicleInfoReceived;
        public event EventHandler<string> ServerLog;

        public TcpServer(string ipAddress, int port)
        {
            IPAddress address = ipAddress == null ? IPAddress.Any : IPAddress.Parse(ipAddress);
            _listener = new TcpListener(address, port);
        }

        public void Start()
        {
            if (_isRunning) return;

            _isRunning = true;
            _listener.Start();

            LogMessage($"WGT 서버가 시작되었습니다. ({((IPEndPoint)_listener.LocalEndpoint).Address}:{((IPEndPoint)_listener.LocalEndpoint).Port})");

            _listenThread = new Thread(ListenForClients)
            {
                IsBackground = true
            };

            _listenThread.Start();
        }

        public void Stop()
        {
            if (!_isRunning) return;

            _isRunning = false;

            lock (_clientsLock)
            {
                foreach (var client in _clients)
                {
                    try
                    {
                        client.Close();
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"클라이언트 연결 종료 중 오류 발생: {ex.Message}");
                    }
                }

                _clients.Clear();
            }

            try
            {
                _listener.Stop();

                LogMessage($"WGT 서버가 종료되었습니다.");
            }
            catch (Exception ex)
            {
                LogMessage($"서버 종료 중 오류 발생: {ex.Message}");
            }
        }

        private void ListenForClients()
        {
            try
            {
                while (_isRunning)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    string clientEndpoint = ((IPEndPoint)client.Client.RemoteEndPoint).ToString();

                    lock (_clientsLock)
                    {
                        _clients.Add(client);
                    }

                    LogMessage($"클라이언트 연결됨: {clientEndpoint}");

                    ThreadPool.QueueUserWorkItem(HandleClientComm, client);
                }
            }
            catch (SocketException ex)
            {
                if (_isRunning)
                {
                    LogMessage($"소켓 오류 발생: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                if (_isRunning)
                {
                    LogMessage($"오류 발생: {ex.Message}");
                }
            }
        }

        private void HandleClientComm(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[64];
            string clientEndpoint = ((IPEndPoint)client.Client.RemoteEndPoint).ToString();

            try
            {
                while (_isRunning && client.Connected)
                {
                    int bytesRead = 0;

                    try
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length);
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"데이터 수신 중 오류: {ex.Message}");
                        break;
                    }

                    if (bytesRead == 0)
                    {
                        LogMessage($"클라이언트 연결 종료됨: {clientEndpoint}");
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    LogMessage($"수신된 데이터 ({clientEndpoint}): {message}");

                    int etxIndex;
                    int startIndex = 0;

                    while ((etxIndex = message.IndexOf(Packet.ETX, startIndex)) >= 0)
                    {
                        int stxIndex = message.LastIndexOf(Packet.STX, etxIndex);

                        if (stxIndex < 0 || stxIndex >= etxIndex)
                        {
                            startIndex = etxIndex + 1;
                            continue;
                        }

                        string packetStr = message.Substring(stxIndex, etxIndex - stxIndex + 1);

                        LogMessage($"패킷 수신됨: {packetStr}");

                        if (_packetHandler.ParsePacket(packetStr, out string command, out string source, out string data, out string dataCount))
                        {
                            LogMessage($"패킷 파싱 성공: {command}, {source}, {data}, {dataCount}");
                            PacketReceived?.Invoke(this, new PacketReceivedEventArgs(command, source, data, dataCount, client));
                            ProcessPacket(client, command, source, data);
                        }
                        else
                        {
                            LogMessage($"패킷 파싱 실패: {packetStr}");
                        }

                        startIndex = etxIndex + 1;
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"클라이언트 통신 처리 중 오류: {ex.Message}");
            }
            finally
            {
                try
                {
                    stream.Close();
                    client.Close();

                    lock (_clientsLock)
                    {
                        _clients.Remove(client);
                    }

                    LogMessage($"클라이언트 연결 종료: {clientEndpoint}");
                }
                catch (Exception ex)
                {
                    LogMessage($"클라이언트 연결 종료 중 오류: {ex.Message}");
                }
            }
        }

        private void ProcessPacket(TcpClient client, string command, string source, string data)
        {
            if (command == Packet.CMD_WHO && source == Packet.SRC_SVR)
            {
                string response = _packetHandler.CreatePacket(Packet.CMD_CON, Packet.SRC_SVR, Packet.RESULT_OK, 3);
                SendData(client, response);

                LogMessage("WHO 명령에 응답: 설비 인증 확인");
            }
            else if (command == Packet.CMD_REG && source == Packet.SRC_SVR)
            {
                string response = _packetHandler.CreatePacket(Packet.CMD_REG, Packet.SRC_WGT, Packet.RESULT_OK, 3);
                SendData(client, response);

                try
                {
                    string[] vehicleData = data.Split('/');
                    string chassisNumber = vehicleData.Length > 0 ? vehicleData[0] : string.Empty;
                    string model = vehicleData.Length > 1 ? vehicleData[1] : string.Empty;
                    string manufacturer = vehicleData.Length > 2 ? vehicleData[2] : string.Empty;

                    LogMessage($"차량 정보 수신: 차대번호={chassisNumber}, 모델={model}, 제조사={manufacturer}");
                    
                    VehicleInfoReceived?.Invoke(this, new VehicleInfoEventArgs(chassisNumber, model, manufacturer));
                }
                catch (Exception ex)
                {
                    LogMessage($"차량 정보 처리 중 오류: {ex.Message}");
                }
            }
        }

        private void SendData(TcpClient client, string data)
        {
            try
            {
                if (client.Connected)
                {
                    byte[] dataBytes = Encoding.UTF8.GetBytes(data);
                    NetworkStream stream = client.GetStream();
                    stream.Write(dataBytes, 0, dataBytes.Length);
                    stream.Flush();

                    LogMessage($"전송 데이터: {data}");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"데이터 전송 중 오류: {ex.Message}");
            }
        }

        private void LogMessage(string message)
        {
            Debug.WriteLine($"[WGT_TcpServer] {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}");
            ServerLog?.Invoke(this, message);
        }
    }
}
