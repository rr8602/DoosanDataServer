using DoosanDataServer.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace DoosanDataServer.Comm
{
    public class TcpClientConnector
    {
        private readonly Packet _packetHanler = new Packet();
        private readonly string _ipAddress;
        private readonly int _port;
        private readonly Action<string> _logAction;
        private TcpClient _client;

        public TcpClientConnector(string ipAddress, int port, Action<string> logAction)
        {
            _ipAddress = ipAddress;
            _port = port;
            _logAction = logAction;
        }

        public bool Connect()
        {
            try
            {
                _client = new TcpClient();
                _client.Connect(_ipAddress, _port);
                _logAction($"설비에 연결함 : {_ipAddress}:{_port}");
                
                return true;
            }
            catch (Exception ex)
            {
                _logAction($"설비 연결 실패 : {ex.Message}");

                return false;
            }
        }

        public bool SendWhoCommand()
        {
            // WHO 명령 전송 - 고정된 형식으로 직접 생성 <WHO/SVR/2>
            string whoPacket = $"{Packet.STX}{Packet.CMD_WHO}{Packet.DELIMETER}{Packet.SRC_SVR}{Packet.DELIMETER}2{Packet.ETX}";

            _logAction($"WHO 명령 전송 (고정 형식): {whoPacket}");

            return SendPacket(whoPacket);
        }

        public bool SendRegCommand(string chassisNumber, string model, string manufacturer)
        {
            string data = $"{chassisNumber}/{model}/{manufacturer}";
            string regpacket = _packetHanler.CreatePacket(Packet.CMD_REG, Packet.SRC_SVR, data, 6);

            return SendPacket(regpacket);
        }

        private bool SendPacket(string packet)
        {
            try
            {
                if (_client != null && _client.Connected)
                {
                    NetworkStream stream = _client.GetStream();
                    byte[] buffer = Encoding.UTF8.GetBytes(packet);
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();

                    _logAction($"패킷 전송 : {packet}");

                    return WaitForResponse();
                }
                else
                {
                    _logAction("설비에 연결되어 있지 않습니다.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logAction($"패킷 전송 실패 : {ex.Message}");
                return false;
            }
        }

        private bool WaitForResponse()
        {
            try
            {
                NetworkStream stream = _client.GetStream();
                byte[] buffer = new byte[64];
                stream.ReadTimeout = 5000;

                int bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    _logAction($"응답 수신 : {response}");

                    if (_packetHanler.ParsePacket(response, out string command, out string source, out string data, out int dataCount))
                    {
                        _logAction($"응답 파싱 성공 - Command: {command}, Source: {source}, Data: {data}, Count: {dataCount}");
                        
                        return data == Packet.RESULT_OK;
                    }
                    else
                    {
                        _logAction("유효하지 않은 응답 패킷");

                        return false;
                    }
                }
                else
                {
                    _logAction("응답 없음");

                    return false;
                }
            }
            catch (Exception ex)
            {
                _logAction($"응답 대기 중 오류 발생 : {ex.Message}");

                return false;
            }
        }

        public void Close()
        {
            if (_client != null && _client.Connected)
            {
                _client.Close();
                _logAction("설비 연결 종료");
            }
        }
    }
}
