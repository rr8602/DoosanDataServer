using DoosanDataServer.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DoosanDataServer.Comm
{
    // TCP 서버 클라이언트 연결
    public class ClientConnectedEventArgs : EventArgs
    {
        public TcpClient Client { get; }
        public string IpAddress { get; }
        public int Port { get; }

        public ClientConnectedEventArgs(TcpClient client)
        {
            Client = client;
            var endpoint = (IPEndPoint)client.Client.RemoteEndPoint;
            IpAddress = endpoint.Address.ToString();
            Port = endpoint.Port;
        }
    }

    // 패킷 수신
    public class PacketReceivedEventArgs : EventArgs
    {
        public string Command { get; }
        public string Source { get; }
        public string Data { get; }
        public int DataCount { get; }
        public TcpClient Client { get; }

        public PacketReceivedEventArgs(string command, string source, string data, int dataCount, TcpClient client)
        {
            Command = command;
            Source = source;
            Data = data;
            DataCount = dataCount;
            Client = client;
        }
    }

    // 차량 검사 결과
    public class InspectionResultEventArgs : EventArgs
    {
        public string ChassisNumber { get; }
        public EquipmentType EquipmentType { get; }
        public string[] Result { get; }
        public DateTime Time { get; }

        public InspectionResultEventArgs(string chassisNumber, EquipmentType equipmentType, string[] result)
        {
            ChassisNumber = chassisNumber;
            EquipmentType = equipmentType;
            Result = result;
            Time = DateTime.Now;
        }
    }

    public class TcpServer
    {
        private TcpListener _listener;
        private bool _isRunning;
        private Thread _listenThread;
        private readonly List<ClientInfo> _connectedClients = new List<ClientInfo>();
        private readonly Packet _packetHandler = new Packet();
        private readonly object _clientsLock = new object();

        // 클라이언트 정보 관리를 위한 내부 클래스
        private class ClientInfo
        {
            public TcpClient Client { get; }
            public EquipmentType? EquipmentType { get; set; }
            public string ChassisNumber { get; set; }
            public string IpAddress { get; }
            public int Port { get; }

            public ClientInfo(TcpClient client)
            {
                Client = client;
                var endpoint = (IPEndPoint)client.Client.RemoteEndPoint;
                IpAddress = endpoint.Address.ToString();
                Port = endpoint.Port;
                EquipmentType = null;
                ChassisNumber = null;
            }
        }

        public event EventHandler<ClientConnectedEventArgs> ClientConnected;
        public event EventHandler<PacketReceivedEventArgs> PacketReceived;
        public event EventHandler<InspectionResultEventArgs> InspectionResultReceived;
        public event EventHandler<string> ServerLog;

        public TcpServer(string ipAddress = null, int port = 5000)
        {
            IPAddress address = ipAddress == null ? IPAddress.Any : IPAddress.Parse(ipAddress);
            _listener = new TcpListener(address, port);
        }

        public void Start()
        {
            if (_isRunning)
                return;

            _isRunning = true;
            _listener.Start();

            LogMessage($"서버가 시작되었습니다. ({((IPEndPoint)_listener.LocalEndpoint).Address}:{((IPEndPoint)_listener.LocalEndpoint).Port})");

            _listenThread = new Thread(ListenForClients)
            {
                IsBackground = true
            };

            _listenThread.Start();
        }

        public void Stop()
        {
            if (!_isRunning)
                return;

            _isRunning = false;

            // 연결된 모든 클라이언트 종료
            lock (_clientsLock)
            {
                foreach (var clientInfo in _connectedClients)
                {
                    try
                    {
                        clientInfo.Client.Close();
                    }
                    catch (Exception ex)
                    {
                        LogMessage($"클라이언트 연결 종료 중 오류: {ex.Message}");
                    }
                }
                _connectedClients.Clear();
            }

            // 리스너 종료
            try
            {
                _listener.Stop();
                LogMessage("서버가 종료되었습니다.");
            }
            catch (Exception ex)
            {
                LogMessage($"서버 종료 중 오류: {ex.Message}");
            }
        }

        // 클라이언트의 연결 대기
        private void ListenForClients()
        {
            try
            {
                while (_isRunning)
                {
                    TcpClient client = _listener.AcceptTcpClient();

                    var clientInfo = new ClientInfo(client);

                    lock (_clientsLock)
                    {
                        _connectedClients.Add(clientInfo);
                    }

                    LogMessage($"클라이언트가 연결되었습니다. {clientInfo.IpAddress}:{clientInfo.Port}");

                    ClientConnected?.Invoke(this, new ClientConnectedEventArgs(client));

                    ThreadPool.QueueUserWorkItem(HandleClientComm, clientInfo);
                }
            }
            catch (SocketException ex)
            {
                if (_isRunning) // 정상 종료가 아닌 경우에만 로그
                {
                    LogMessage($"리스닝 중 소켓 오류: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                if (_isRunning) // 정상 종료가 아닌 경우에만 로그
                {
                    LogMessage($"리스닝 중 예외 발생: {ex.Message}");
                }
            }
        }

        // 연결된 클라이언트와의 통신 처리
        private void HandleClientComm(object clientInfoObj)
        {
            ClientInfo clientInfo = (ClientInfo)clientInfoObj;
            TcpClient client = clientInfo.Client;
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[64];

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
                        LogMessage($"클라이언트 연결이 종료되었습니다. {clientInfo.IpAddress}:{clientInfo.Port}");
                        break;
                    }

                    string message = Encoding.GetEncoding("EUC-KR").GetString(buffer, 0, bytesRead);
                    LogMessage($"수신된 원본 데이터: {message}");

                    int etxIndex;

                    while ((etxIndex = message.IndexOf(Packet.ETX)) >= 0)
                    {
                        string packetStr = message.Substring(0, etxIndex + 1);

                        LogMessage($"수신 패킷: {packetStr}");

                        if (_packetHandler.ParsePacket(packetStr, out string command, out string source, out string data, out int dataCount))
                        {
                            LogMessage($"패킷 파싱 성공: 명령어={command}, 소스={source}, 데이터={data}, 데이터 카운트={dataCount}");
                            DetermineClientType(clientInfo, source);
                            PacketReceived?.Invoke(this, new PacketReceivedEventArgs(command, source, data, dataCount, client));
                            ProcessPacket(clientInfo, command, source, data, dataCount, packetStr);
                        }
                        else
                        {
                            LogMessage($"\n유효하지 않은 패킷: {packetStr}");
                        }

                        message = message.Substring(etxIndex + 1);
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
                        _connectedClients.Remove(clientInfo);
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"클라이언트 연결 종료 중 오류: {ex.Message}");
                }
            }
        }

        // 클라이언트 타입을 식별, 저장
        private void DetermineClientType(ClientInfo clientInfo, string source)
        {
            if (clientInfo.EquipmentType != null)
                return; // 이미 식별됨

            switch (source)
            {
                case Packet.SRC_ABS:
                    clientInfo.EquipmentType = EquipmentType.ABS;
                    LogMessage($"클라이언트 식별: {clientInfo.IpAddress}:{clientInfo.Port} - ABS 설비");
                    break;

                case Packet.SRC_HLT:
                    clientInfo.EquipmentType = EquipmentType.HLT;
                    LogMessage($"클라이언트 식별: {clientInfo.IpAddress}:{clientInfo.Port} - HLT 설비");
                    break;

                case Packet.SRC_ADAS:
                    clientInfo.EquipmentType = EquipmentType.ADAS;
                    LogMessage($"클라이언트 식별: {clientInfo.IpAddress}:{clientInfo.Port} - ADAS 설비");
                    break;

                case Packet.SRC_ANG:
                    clientInfo.EquipmentType = EquipmentType.ANG;
                    LogMessage($"클라이언트 식별: {clientInfo.IpAddress}:{clientInfo.Port} - 경사각도 설비");
                    break;

                case Packet.SRC_WGT:
                    clientInfo.EquipmentType = EquipmentType.WGT;
                    LogMessage($"클라이언트 식별: {clientInfo.IpAddress}:{clientInfo.Port} - 무게 측정 설비");
                    break;
            }
        }

        // 수신된 패킷 처리
        private void ProcessPacket(ClientInfo clientInfo, string command, string source, string data, int dataCount, string packetStr)
        {
            string[] parts = packetStr.Split('/');

            // WHO 명령어: 설비 인증 요청
            if (command == Packet.CMD_WHO)
            {
                string response = _packetHandler.CreatePacket(Packet.CMD_CON, Packet.SRC_SVR, Packet.RESULT_OK, dataCount);
                SendData(clientInfo.Client, response);
            }
            // REG 명령어: 차량 정보 등록
            else if (command == Packet.CMD_REG)
            {
                clientInfo.ChassisNumber = data;
                LogMessage($"접수번호 등록: {data} - {clientInfo.EquipmentType}");

                string response = _packetHandler.CreatePacket(Packet.CMD_REG, Packet.SRC_SVR, Packet.RESULT_OK, dataCount);
                SendData(clientInfo.Client, response);
            }
            // RST 명령어: 설비 검사 결과 수신
            else if (command == Packet.CMD_RST && clientInfo.EquipmentType.HasValue)
            {
                InspectionResultReceived?.Invoke(this, new InspectionResultEventArgs(
                    clientInfo.ChassisNumber,
                    clientInfo.EquipmentType.Value,
                    parts
                ));

                string response = _packetHandler.CreatePacket(Packet.CMD_RST, Packet.SRC_SVR, Packet.RESULT_OK, dataCount);
                SendData(clientInfo.Client, response);
            }
        }

        // 특정 클라이언트에게 데이터 전송
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

                    LogMessage($"전송 패킷: [RX] {data}");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"패킷 전송 중 오류: {ex.Message}");
            }
        }

        // 로그 메시지 출력
        private void LogMessage(string message)
        {
            Debug.WriteLine($"[TcpServer] {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}\n");
            ServerLog?.Invoke(this, message);
        }
    }
}
