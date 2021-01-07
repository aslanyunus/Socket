using NetSocket.Core;
using System;
using System.Net;
using System.Net.Sockets;

namespace NetSocket.Tcp
{
    public class TcpReceiveSocket : SenderSocket
    {
        public TcpReceiveSocket(Socket socket, SocketSide socketSide, SocketEventProvider eventProvider) :
            base(((IPEndPoint)socket.RemoteEndPoint).Address, SocketType.Stream, socketSide, eventProvider)
        {
            Sock = socket;
        }

        public override bool Start()
        {
            try
            {
                Sock.BeginReceive(Pack, 0, Pack.Size, SocketFlags.None, ReceivedCallback, null);
            }
            catch (Exception ex)
            {
                EventProvider.ExecuteErrorHandler(ex, SocketErrorLocation.Receive);
                return false;
            }
            return true;
        }

        void ReceivedCallback(IAsyncResult asyncResult)
        {
            try
            {
                ReceivedDataLen = Sock.EndReceive(asyncResult);
            }
            catch (SocketException) { return; }
            catch (ObjectDisposedException) { return; }

            if (ReceivedDataLen <= 0)
            {
                EventProvider.ExecuteDisconnectedHandler(IP);
                Dispose(this);
                return;
            }

            EventProvider.ExecuteReceivedDataHandler(Pack.Clone(), this);

            Array.Clear(Pack, 0, Pack.Size);

            Sock.BeginReceive(Pack, 0, Pack.Size, SocketFlags.None, ReceivedCallback, null);
        }
    }
}
