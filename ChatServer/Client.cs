using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using ChatServer.Net.IO;

namespace ChatServer
{
    internal class Client
    {
        #region Fields
        private readonly PacketReader _packetReader;
        #endregion

        #region Properties
        public Guid UID { get; set; }
        public string Username { get; set; }
        public TcpClient ClientSocket { get; set; }
        #endregion

        #region Constructor
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
        #endregion

        #region Private Helper Methods
        private void ProcessPackets()
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
                catch (Exception) // TODO Advanced - Check for NetworkException specifically
                {
                    Program.ConsoleWriteLine($"Client {Username} ({UID}) has disconnected");
                    Program.BroadcastDisconnect(UID);
                    ClientSocket.Close();
                    break;
                }
            }
        }
        #endregion
    }
}
