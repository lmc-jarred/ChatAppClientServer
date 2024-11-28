using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatClient.Net.IO
{
    internal class PacketReader : BinaryReader
    {
        private NetworkStream _ns;

        public PacketReader(NetworkStream ns) : base(ns)
        {
            _ns = ns;
        }

        public string ReadMessage()
        {
            int length = ReadInt32();
            byte[] msgBuffer = new byte[length];
            _ns.Read(msgBuffer, 0, length);

            string msg = Encoding.ASCII.GetString(msgBuffer);
            return msg;
        }
    }
}
