using System;

namespace NetSocket.Core
{
    public abstract class SocketProvider : ISocketProvider
    {
        protected SenderSocket Sender { get; set; }
        public SocketEventProvider EventProvider { get; set; }
        public SenderSocket PartnerReceiverSocket { get; set; }
        public SocketState State { get; set; }

        public SocketProvider()
        {
            EventProvider = new SocketEventProvider();
        }

        public virtual void Start()
        {
            State = Sender.State;
        }
        public virtual void Stop()
        {
            State = Sender.State;
        }

        public abstract void SendData(Packet package, Action success = null, Action error = null);
        public abstract void Broadcast(Packet package);
    }
}
