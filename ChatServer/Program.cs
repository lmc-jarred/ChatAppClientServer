using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

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

                // TODO - Broadcast the connection to everyone on the server
            }
        }
    }
}