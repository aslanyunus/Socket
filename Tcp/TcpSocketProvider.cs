using NetSocket.Core;
using System;
using System.Net;

namespace NetSocket.Tcp
{
    public class TcpSocketProvider : SocketProvider
    {
        public TcpSocketProvider(IPAddress ip, SocketSide socketSide)
        {
            Sender = TcpSocketFactory.Create(ip, socketSide, EventProvider);
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

        public override void SendData(Packet package, Action success = null, Action error = null)
        {
            Sender.SendData(package, success, error);
        }

        public override void Broadcast(Packet package)
        {
            Sender.Broadcast(package);
        }
    }
}
