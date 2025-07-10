using Incline.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Incline.Comm
{
    public class PacketReceivedEventArgs : EventArgs
    {
        public string Command { get; set; }
        public string Source { get; set; }
        public string Data { get; set; }
        public int DataCount { get; set; }
        public TcpClient Client { get; set; }

        public PacketReceivedEventArgs(string command, string source, string data, int dataCount, TcpClient client)
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
        private readonly Packet _packetHandler = new Packet();
        private readonly object _clientsLock = new object();
        private readonly List<TcpClient> _clients = new List<TcpClient>();

        public event EventHandler<PacketReceivedEventArgs> PacketReceived;
        public event EventHandler<VehicleInfoEventArgs> VehicleInfoReceived;
        public event EventHandler<string> ServerLog;

        public TcpServer(string ipAddress, int port)
        {
            /*IPAddress address = ipAddress == null ? IPAddress.Any : IPAddress.Parse(ipAddress);
            _listener = new TcpListener(address, port);*/

            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            if (_isRunning) return;

            _isRunning = true;
            _listener.Start();

            LogMessage($"Incline 서버가 시작되었습니다. ({((IPEndPoint)_listener.LocalEndpoint).Address}:{((IPEndPoint)_listener.LocalEndpoint).Port})");

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

            // 연결된 모든 클라이언트 종료
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
                LogMessage("Incline 서버가 종료되었습니다.");
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

                    LogMessage($"클라이언트가 연결되었습니다: {clientEndpoint}");

                    ThreadPool.QueueUserWorkItem(HandleClientComm, client);
                }
            }
            catch (SocketException ex)
            {
                if (_isRunning)
                {
                    LogMessage($"리스닝 중 소켓 오류: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                if (_isRunning)
                {
                    LogMessage($"리스닝 중 예외 발생: {ex.Message}");
                }
            }
        }

        private void HandleClientComm(object clientObj)
        {
            TcpClient client = (TcpClient)clientObj;
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
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
                        LogMessage($"클라이언트 연결이 종료되었습니다: {clientEndpoint}");
                        break;
                    }

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    LogMessage($"수신된 원본 데이터: {message}");

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
                        LogMessage($"수신 패킷: {packetStr}");

                        if (_packetHandler.ParsePacket(packetStr, out string command, out string source, out string data, out int dataCount))
                        {
                            LogMessage($"패킷 파싱 성공: 명령어={command}, 소스={source}, 데이터={data}, 데이터 카운트={dataCount}");
                            PacketReceived?.Invoke(this, new PacketReceivedEventArgs(command, source, data, dataCount, client));
                            ProcessPacket(client, command, source, data);
                        }
                        else
                        {
                            LogMessage($"유효하지 않은 패킷: {packetStr}");
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
            // WHO 명령어: 설비 인증 요청 - 서버가 Incline 설비인지 확인
            if (command == Packet.CMD_WHO && source == Packet.SRC_SVR)
            {
                string response = _packetHandler.CreatePacket(Packet.CMD_CON, Packet.SRC_ANG, Packet.RESULT_OK, 3);
                SendData(client, response);
                LogMessage("WHO 명령에 응답: 설비 인증 확인");
            }
            // REG 명령어: 차량 정보 등록
            else if (command == Packet.CMD_REG && source == Packet.SRC_SVR)
            {
                string response = _packetHandler.CreatePacket(Packet.CMD_REG, Packet.SRC_ANG, Packet.RESULT_OK, 3);
                SendData(client, response);

                try
                {
                    // 차량 정보 파싱
                    string[] vehicleData = data.Split('/');
                    string chassisNumber = vehicleData.Length > 0 ? vehicleData[0] : string.Empty;
                    string model = vehicleData.Length > 1 ? vehicleData[1] : string.Empty;
                    string manufacturer = vehicleData.Length > 2 ? vehicleData[2] : string.Empty;

                    LogMessage($"차량 정보 수신: 차대번호={chassisNumber}, 모델={model}, 제조사={manufacturer}");
                    VehicleInfoReceived?.Invoke(this, new VehicleInfoEventArgs(chassisNumber, model, manufacturer));
                }
                catch (Exception ex)
                {
                    LogMessage($"차량 정보 파싱 중 오류: {ex.Message}");
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

                    LogMessage($"전송 패킷: {data}");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"패킷 전송 중 오류: {ex.Message}");
            }
        }

        private void LogMessage(string message)
        {
            Debug.WriteLine($"[Incline_TcpServer] {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}");
            ServerLog?.Invoke(this, message);
        }
    }
}
