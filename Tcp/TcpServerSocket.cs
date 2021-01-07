using NetSocket.Core;
using System;
using System.Net;
using System.Net.Sockets;

namespace NetSocket.Tcp
{
    class TcpServerSocket : SenderSocket
    {
        public TcpServerSocket(IPAddress ip, SocketEventProvider eventProvider) 
            : base(ip, SocketType.Stream, SocketSide.Server, eventProvider)
        {

        }

        public override bool Start()
        {
            try
            {
                Sock.BeginAccept(AcceptCallback, null);
                State = SocketState.Started;
            }
            catch (Exception ex)
            {
                EventProvider.ExecuteErrorHandler(ex, SocketErrorLocation.BeginStart);
                return false;
            }
            return true;
        }

        private void AcceptCallback(IAsyncResult asyncResult)
        {
            Socket socket = null;
            try
            {
                socket = Sock.EndAccept(asyncResult);
            }
            catch (ObjectDisposedException) { return; }
            catch (Exception ex)
            {
                EventProvider.ExecuteErrorHandler(ex, SocketErrorLocation.EndStart);
                return;
            }

            TcpReceiveSocket receiveSocket = new TcpReceiveSocket(socket, SocketSide.Client, EventProvider);

            if (receiveSocket.Start())
            {
                TcpClientSockets.Add(receiveSocket);
                TcpClientSockets.TrimExcess();
            }

            EventProvider.ExecuteHandshakeHandler(((IPEndPoint)socket.RemoteEndPoint).Address, receiveSocket);

            Sock.BeginAccept(AcceptCallback, null);
        }
    }
}
