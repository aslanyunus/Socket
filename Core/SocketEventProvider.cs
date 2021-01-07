using System;
using System.Net;

namespace NetSocket.Core
{
    public class SocketEventProvider
    {
        public event Action<SocketState> ProcessHandler;
        public event Action<IPAddress, SenderSocket> HandshakeHandler;
        public event Action<Exception, SocketErrorLocation> ErrorHandler;
        public event Action<Packet, SenderSocket> DataReceivedHandler;
        public event Action<IPAddress> DisconnectedEventHandler;

        public void ExecuteProcessHandler(SocketState socketProcessTypes)
        {
            ProcessHandler?.Invoke(socketProcessTypes);
        }

        public void ExecuteDisconnectedHandler(IPAddress ip)
        {
            DisconnectedEventHandler?.Invoke(ip);
        }

        public void ExecuteErrorHandler(Exception exception, SocketErrorLocation socketErrorLocations)
        {
            ErrorHandler?.Invoke(exception, socketErrorLocations);
        }

        public void ExecuteHandshakeHandler(IPAddress ipAddress, SenderSocket rSocket)
        {
            HandshakeHandler?.Invoke(ipAddress, rSocket);
        }

        public void ExecuteReceivedDataHandler(Packet package, SenderSocket receiveSocket)
        {
            DataReceivedHandler?.Invoke(package, receiveSocket);
        }
    }
}
