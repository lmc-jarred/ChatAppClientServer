using System.IO;
using System.Text;

namespace ChatClient.Net.IO
{
    internal class PacketBuilder
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
