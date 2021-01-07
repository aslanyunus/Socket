using NetSocket.Core;
using System;
using System.Net;
using System.Reflection;

namespace NetSocket.Tcp
{
    class TcpSocketFactory
    {
        public static SenderSocket Create(IPAddress ip, SocketSide socketSide, SocketEventProvider eventProvider)
        {
            switch (socketSide)
            {
                default:
                case SocketSide.Server:
                    return new TcpServerSocket(ip, eventProvider);
                case SocketSide.Client:
                    return new TcpClientSocket(ip, eventProvider);
            }
        }

        public static SenderSocket Create<T>(IPAddress ip, SocketEventProvider eventProvider)
        {
            return (SenderSocket)Activator.CreateInstance(typeof(T), BindingFlags.Public | BindingFlags.Instance, null, new object[] { ip, eventProvider }, null);
        }
    }
}
