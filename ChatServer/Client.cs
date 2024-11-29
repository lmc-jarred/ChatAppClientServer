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

            Program.ConsoleWriteLine($"Client has connected with the username: {Username} ({UID})");

            Task.Run(() => ProcessPackets());
        }

        void ProcessPackets()
        {
            while (true)
            {
                try
                {
                    // TODO Advanced - Not using events on server side because we're only reading one type of packet

                    byte opcode = _packetReader.ReadByte();
                    switch (opcode)
                    {
                        case 5: //Client sends server a message
                            string message = _packetReader.ReadMessage();
                            Program.ConsoleWriteLine($"Received message from {Username}: {message}");
                            Program.BroadcastMessage(message);
                            break;
                    }
                }
                catch (Exception ex) // TODO Advanced - Check for NetworkException specifically
                {
                    throw;
                }
            }
        }
    }
}
