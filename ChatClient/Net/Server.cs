using System.Net.Sockets;
using ChatClient.Net.IO;

namespace ChatClient.Net
{
    internal class Server
    {
        #region Fields
        private readonly TcpClient _client;
        #endregion

        #region Properties
        public PacketReader? PacketReader;
        #endregion

        #region Events
        public event Action? ConnectedEvent;
        public event Action? MsgReceivedEvent;
        public event Action? UserDisconnectEvent;
        #endregion

        #region Constructor
        public Server()
        {
            _client = new TcpClient();
        }
        #endregion

        #region Public Methods
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

        public void SendChatMessage(string? message)
        {
            PacketBuilder messagePacket = new PacketBuilder();
            messagePacket.WriteOpCode(5); //Client sends server a message
            messagePacket.WriteMessage(message ?? string.Empty);

            _client.Client.Send(messagePacket.GetPacketBytes());
        }
        #endregion

        #region Private Helper Methods
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
                        case 1: //Server broadcast connection to client
                            ConnectedEvent?.Invoke();
                            break;

                        case 5: //Client sends a message
                            MsgReceivedEvent?.Invoke();
                            break;

                        case 10: //Server broadcast disconnect to client
                            UserDisconnectEvent?.Invoke();
                            break;

                        default:
                            Console.WriteLine($"Received opcode {opcode}");
                            break;
                    }
                }
            });
        }
        #endregion
    }
}
