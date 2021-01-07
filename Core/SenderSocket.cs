using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace NetSocket.Core
{
    public abstract class SenderSocket : BasicSocket, ISenderSocket
    {
        protected Packet Pack;
        protected int ReceivedDataLen;

        public static List<SenderSocket> TcpClientSockets { get; set; }

        public abstract bool Start();

        protected SenderSocket(IPAddress ip, SocketType socketType, SocketSide socketSide, SocketEventProvider eventProvider) 
            : base(ip, socketType, socketSide, eventProvider)
        {
            Pack = new Packet();

            if (socketType == SocketType.Stream && socketSide == SocketSide.Server)
            {
                TcpClientSockets = new List<SenderSocket>();
            }
        }

        public void SendData(Packet packet, Action success = null, Action error = null)
        {
            Sock.BeginSend(packet, 0, packet.Size, SocketFlags.None, (ar) =>
            {
                if (ar.IsCompleted)
                    success?.Invoke();
                else
                    error?.Invoke();
            }, null);
        }

        public void SendDataTo(Packet packet, Action success = null, Action error = null)
        {
            Sock.BeginSendTo(packet, 0, packet.Size, SocketFlags.None, SocketSettings.ToEndPoint, (ar) =>
            {
                if (ar.IsCompleted)
                    success?.Invoke();
                else
                    error?.Invoke();
            }, null);
        }

        public void Broadcast(Packet packet)
        {
            for (int i = 0; i < TcpClientSockets.Count; i++)
            {
                TcpClientSockets[i].SendData(packet);
            }
        }

        public void BroadcastTo(Packet packet)
        {
            Sock.BeginSendTo(packet, 0, packet.Size, SocketFlags.None, SocketSettings.BroadcastEndPoint, (ar) => { }, null);
        }

        public void Dispose(SenderSocket senderSocket)
        {
            if (TcpClientSockets.Contains(senderSocket))
                TcpClientSockets.Remove(senderSocket);
            base.Dispose();
        }

        public override void Dispose()
        {
            TcpClientSockets.Clear();
            base.Dispose();
        }
    }
}
