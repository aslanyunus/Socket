using NetSocket.Core;
using System;
using System.Net;
using System.Net.Sockets;

namespace NetSocket.Udp
{
    class UdpSocket : SenderSocket
    {
        readonly object obj = new object();

        public UdpSocket(IPAddress ip, SocketSide socketSide, SocketEventProvider eventProvider)
            : base(ip, SocketType.Dgram, socketSide, eventProvider)
        {

        }

        public override bool Start()
        {
            try
            {
                Sock.BeginReceiveFrom(Pack, 0, Pack.Size, SocketFlags.None, ref SocketSettings.ServerEndPoint, ReceivedCallback, obj);
                State = SocketState.Started;
            }
            catch (Exception ex)
            {
                EventProvider.ExecuteErrorHandler(ex, SocketErrorLocation.BeginStart);
                return false;
            }
            return true;
        }

        private void ReceivedCallback(IAsyncResult asyncResult)
        {
            try
            {
                ReceivedDataLen = Sock.EndReceiveFrom(asyncResult, ref SocketSettings.ServerEndPoint);
            }
            catch (SocketException) { return; }
            catch (ObjectDisposedException) { return; }

            if (ReceivedDataLen > 0)
            {
                EventProvider.ExecuteReceivedDataHandler(Pack.Clone(), this);
                Array.Clear(Pack, 0, Pack.Size);
            }

            Sock.BeginReceiveFrom(Pack, 0, Pack.Size, SocketFlags.None, ref SocketSettings.ServerEndPoint, ReceivedCallback, obj);
        }
    }
}
