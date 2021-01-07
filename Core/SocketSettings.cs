using System.Net;

namespace NetSocket.Core
{
    public abstract class SocketSettings
    {
        public static int TcpPort { get; set; }
        public static int UdpPort { get; set; }
        public static byte PacketDataSize { get; set; }
        public static int ConnectionLength { get; set; }

        public static EndPoint ServerEndPoint;
        public static EndPoint BroadcastEndPoint;
        public static EndPoint ToEndPoint;
    }
}
