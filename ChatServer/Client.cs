using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatServer.Net.IO;

namespace ChatServer
{
    internal class Client
    {
        private PacketReader _packetReader;

        public Guid UID { get; set; }
        public string Username { get; set; }
        public TcpClient ClientSocket { get; set; }

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();
            _packetReader = new PacketReader(ClientSocket.GetStream());

            byte opcode = _packetReader.ReadByte();
            // TODO Advanced - Validate op code

            Username = _packetReader.ReadMessage();

            Console.WriteLine($"[{DateTime.UtcNow}]: Client has connected with the username: {Username}");
        }
    }
}
