using NetSocket.Core;
using System;
using System.Net;
using System.Net.Sockets;

namespace NetSocket.Tcp
{
    class TcpClientSocket : SenderSocket
    {
        public TcpClientSocket(IPAddress ip, SocketEventProvider eventProvider) 
            : base(ip, SocketType.Stream, SocketSide.Client, eventProvider)
        {

        }

        public override bool Start()
        {
            try
            {
                Sock.BeginConnect(IP, SocketSettings.TcpPort, ConnectionCallback, null);
                State = SocketState.Started;
            }
            catch (Exception ex)
            {
                EventProvider.ExecuteErrorHandler(ex, SocketErrorLocation.BeginStart);
                return false;
            }
            return true;
        }

        private void ConnectionCallback(IAsyncResult asyncResult)
        {
            try
            {
                Sock.EndConnect(asyncResult);
            }
            catch (Exception ex)
            {
                EventProvider.ExecuteErrorHandler(ex, SocketErrorLocation.EndStart);
                return;
            }

            TcpReceiveSocket receiveSocket = new TcpReceiveSocket(Sock, SocketSide.Client, EventProvider);
            receiveSocket.Start();

            EventProvider.ExecuteHandshakeHandler(((IPEndPoint)Sock.RemoteEndPoint).Address, null);
        }
    }
}
