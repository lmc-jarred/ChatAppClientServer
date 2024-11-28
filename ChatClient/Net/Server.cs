using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using ChatClient.Net.IO;

namespace ChatClient.Net
{
    internal class Server
    {
        TcpClient _client;

        public PacketReader? PacketReader;

        public event Action? ConnectedEvent;

        public Server()
        {
            _client = new TcpClient();
        }

        public void ConnectToServer(string? username)
        {
            if (!_client.Connected)
            {
                _client.Connect("127.0.0.1", 7891);
                PacketReader = new PacketReader(_client.GetStream());

                if (!string.IsNullOrWhiteSpace(username))
                {
                    PacketBuilder connectionPacket = new PacketBuilder();
                    connectionPacket.WriteOpCode(0); //Client connect to server
                    connectionPacket.WriteMessage(username);

                    _client.Client.Send(connectionPacket.GetPacketBytes());
                }

                ReadPackets();
            }
        }

        private void ReadPackets()
        {
            Task.Run(() => // TODO Advanced - Keep track of thread
            {
                if (PacketReader == null)
                    throw new ApplicationException("Unable to read packets - PacketReader is null");

                while (true)
                {
                    byte opcode = PacketReader.ReadByte(); // TODO Advanced - Add ReadOpCode method that reads and returns first byte from stream
                    switch (opcode)
                    {
                        case 1: //Server broadcast to client
                            ConnectedEvent?.Invoke();
                            break;
                        default:
                            Console.WriteLine($"Received opcode {opcode}");
                            break;
                    }
                }
            });
        }
    }
}
