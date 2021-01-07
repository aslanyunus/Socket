using System;
using System.Net;
using System.Net.Sockets;

namespace NetSocket.Core
{
    public abstract class BasicSocket : IDisposable
    {
        public Socket Sock;
        public IPAddress IP { get; set; }
        public SocketType SockType { get; set; }
        public SocketSide SockSide { get; set; }
        public SocketEventProvider EventProvider { get; set; }
        public SocketState State { get; set; }

        protected BasicSocket(IPAddress ip, SocketType socketType, SocketSide socketSide, SocketEventProvider eventProvider)
        {
            IP = ip;
            SockType = socketType;
            SockSide = socketSide;
            (EventProvider = eventProvider).ProcessHandler += EventProvider_ProcessHandler;

            ReUse();
        }

        public void ReUse()
        {
            if (SockType == SocketType.Stream)
            {
                Sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                State = SocketState.None;

                if (SockSide == SocketSide.Server)
                {
                    try
                    {
                        Sock.Bind(new IPEndPoint(IP, SocketSettings.TcpPort));
                        Sock.Listen(SocketSettings.ConnectionLength);
                    }
                    catch (Exception ex)
                    {
                        EventProvider.ExecuteErrorHandler(ex, SocketErrorLocation.BindAndListen);
                    }
                }
            }
            else if (SockType == SocketType.Dgram)
            {
                Sock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                Sock.Bind(SocketSettings.ServerEndPoint);
            }
        }

        private void EventProvider_ProcessHandler(SocketState state)
        {
            State = state;
        }

        public bool Stop()
        {
            try
            {
                if (Sock.Connected)
                {
                    Sock.Shutdown(SocketShutdown.Both);
                    State = SocketState.Stoped;
                }
            }
            catch (Exception ex)
            {
                EventProvider.ExecuteErrorHandler(ex, SocketErrorLocation.Shutdown);
                return false;
            }

            try
            {
                Sock.Close();
                State = SocketState.Stoped;
            }
            catch (Exception ex)
            {
                EventProvider.ExecuteErrorHandler(ex, SocketErrorLocation.Close);
                return false;
            }

            return true;
        }

        public virtual void Dispose()
        {
            Sock.Dispose();
        }
    }
}
