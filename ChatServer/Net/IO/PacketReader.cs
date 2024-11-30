using System.IO;
using System.Net.Sockets;
using System.Text;

namespace ChatServer.Net.IO
{
    internal class PacketReader(NetworkStream ns) : BinaryReader(ns) // TODO - Link to ChatClient as well, or make Chat.Core project
    {
        #region Fields
        private readonly NetworkStream _ns = ns;
        #endregion

        #region Constructor
        public string ReadMessage()
        {
            int length = ReadInt32();
            byte[] msgBuffer = new byte[length];
            _ns.ReadExactly(msgBuffer, 0, length);

            string msg = Encoding.ASCII.GetString(msgBuffer);
            return msg;
        }
        #endregion
    }
}
