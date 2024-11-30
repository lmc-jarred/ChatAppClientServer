using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using ChatServer.Net.IO;

namespace ChatServer
{
    internal class Program
    {
        #region Fields
        private readonly static List<Client> _users = new List<Client>();
        private readonly static TcpListener _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
        #endregion

        #region Entry Point
        private static void Main(string[] args)
        {
            _listener.Start();

            while (true)
            {
                Client clientInfo = new Client(_listener.AcceptTcpClient());
                _users.Add(clientInfo);

                BroadcastConnection();
            }
        }
        #endregion

        #region Public Methods
        public static void ConsoleWriteLine(string msg) => Console.WriteLine($"[{DateTime.UtcNow}]: {msg}");

        public static void BroadcastConnection()
        {
            foreach (Client user1 in _users)
            {
                foreach (Client user2 in _users)
                {
                    PacketBuilder broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1); //Server broadcast connection to client
                    broadcastPacket.WriteMessage(user2.Username);
                    broadcastPacket.WriteMessage(user2.UID.ToString());

                    user1.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }

        public static void BroadcastDisconnect(Guid uid)
        {
            Client? disconnectedUser = _users.Where(x => x.UID == uid).FirstOrDefault();
            if (disconnectedUser == null)
                return;

            _users.Remove(disconnectedUser);

            foreach (Client user in _users)
            {
                PacketBuilder broadcastPacket = new PacketBuilder();
                broadcastPacket.WriteOpCode(10); //Server broadcast disconnect to client
                broadcastPacket.WriteMessage(uid.ToString());

                user.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
            }

            BroadcastMessage($"{disconnectedUser.Username} disconnected");
        }

        public static void BroadcastMessage(string message)
        {
            foreach (Client user in _users)
            {
                PacketBuilder messagePacket = new PacketBuilder();
                messagePacket.WriteOpCode(5);
                messagePacket.WriteMessage(message);

                user.ClientSocket.Client.Send(messagePacket.GetPacketBytes());
            }
        }
        #endregion
    }
}