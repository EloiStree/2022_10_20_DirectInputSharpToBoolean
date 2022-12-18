using System.Net;
using System.Net.Sockets;
using System.Text;

namespace SharpDXInputObserver
{
    public class UdpCommandPusher
    {
        public string m_ip;
        public int m_port;


        Socket m_socket;
        IPAddress m_broadcast;
        IPEndPoint m_endPoint;
        public UdpCommandPusher(string ip, int port)
        {
            m_ip = ip;
            m_port = port;
            Start();
        }
        public UdpCommandPusher( int port)
        {
            m_ip = "127.0.0.1";
            m_port = port;
            Start();
        }

        public void Start() {

            m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            m_broadcast = IPAddress.Parse(m_ip);
            m_endPoint = new IPEndPoint(m_broadcast, m_port);
        }
        public void PushCommand(string message) {

            byte[] sendbuf = Encoding.Unicode.GetBytes(message);
            m_socket.SendTo(sendbuf, m_endPoint);
        }
    }
}