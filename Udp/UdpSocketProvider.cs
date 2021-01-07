using NetSocket.Core;
using System;
using System.Net;

namespace NetSocket.Udp
{
    public class UdpSocketProvider : SocketProvider
    {
        public UdpSocketProvider(IPAddress ip, SocketSide socketSide)
        {
            Sender = new UdpSocket(ip, socketSide, EventProvider);
        }

        public override void Broadcast(Packet package)
        {
            Sender.BroadcastTo(package);
        }

        public override void SendData(Packet package, Action success = null, Action error = null)
        {
            Sender.SendDataTo(package, success, error);
        }

        public override void Start()
        {
            if (Sender.State != SocketState.None)
            {
                Sender.ReUse();
            }

            if (Sender.Start())
            {
                base.Start();
                EventProvider.ExecuteProcessHandler(SocketState.Started);
            }
        }

        public override void Stop()
        {
            Sender.Stop();
            Sender.Dispose();
            base.Stop();
            EventProvider.ExecuteProcessHandler(SocketState.Stoped);
        }
    }
}
