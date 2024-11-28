using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using ChatServer.Net.IO;

namespace ChatServer
{
    class Program
    {
        private static List<Client> _users;
        static TcpListener _listener;

        static void Main(string[] args)
        {
            _users = new List<Client>();
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            while (true)
            {
                Client clientInfo = new Client(_listener.AcceptTcpClient());
                _users.Add(clientInfo);

                BroadcastConnection();
            }
        }

        static void BroadcastConnection()
        {
            foreach (Client user1 in _users)
            {
                foreach (Client user2 in _users)
                {
                    PacketBuilder broadcastPacket = new PacketBuilder();
                    broadcastPacket.WriteOpCode(1); //Server broadcast to client
                    broadcastPacket.WriteMessage(user2.Username);
                    broadcastPacket.WriteMessage(user2.UID.ToString());

                    user1.ClientSocket.Client.Send(broadcastPacket.GetPacketBytes());
                }
            }
        }
    }
}