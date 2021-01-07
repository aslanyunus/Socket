using System;

namespace NetSocket.Core
{
    public class Packet
    {
        private byte[] _buffer;

        public int Size => _buffer.Length;


        public static implicit operator byte[](Packet p) => p._buffer;


        public byte this[int index]
        {
            get => _buffer[index];
            set => _buffer[index] = value;
        }

        private Packet(byte[] buffer)
        {
            _buffer = buffer;
        }

        public Packet(int size = 0)
        {
            _buffer = new byte[size];
        }

        public Packet Clone()
        {
            byte[] buffer = new byte[_buffer.Length];
            Buffer.BlockCopy(_buffer, 0, buffer, 0, buffer.Length);
            return new Packet(buffer);
        }
    }
}
