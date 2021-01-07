using System;

namespace NetSocket.Core
{
    public interface ISocketProvider
    {
        SocketState State { get; set; }
        SenderSocket PartnerReceiverSocket { get; set; }
        SocketEventProvider EventProvider { get; set; }

        void Start();
        void Stop();
        void SendData(Packet package, Action success = null, Action error = null);
        void Broadcast(Packet package);
    }
}
