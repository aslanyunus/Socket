using System;

namespace NetSocket.Core
{
    public interface ISenderSocket
    {
        void SendData(Packet package, Action success = null, Action error = null);
        void SendDataTo(Packet package, Action success = null, Action error = null);
        void Broadcast(Packet package);
        void BroadcastTo(Packet package);
    }
}
