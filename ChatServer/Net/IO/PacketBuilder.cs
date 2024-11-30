using System;
using System.IO;
using System.Text;

namespace ChatServer.Net.IO
{
    internal class PacketBuilder // TODO - Link to ChatClient as well, or make Chat.Core project
    {
        #region Fields
        private readonly MemoryStream _ms;
        #endregion

        #region Constructor
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }
        #endregion

        #region Public Methods
        public void WriteOpCode(byte opcode)
        {
            _ms.WriteByte(opcode);
        }

        public void WriteMessage(string msg)
        {
            _ms.Write(BitConverter.GetBytes(msg.Length));
            _ms.Write(Encoding.ASCII.GetBytes(msg));
        }

        public byte[] GetPacketBytes() => _ms.ToArray();
        #endregion
    }
}
