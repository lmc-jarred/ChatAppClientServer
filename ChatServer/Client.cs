using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    internal class Client
    {
        public Guid UID { get; set; }
        public string Username { get; set; }
        public TcpClient ClientSocket { get; set; }

        public Client(TcpClient client)
        {
            ClientSocket = client;
            UID = Guid.NewGuid();

            Console.WriteLine($"[{DateTime.UtcNow}]: Client has connected with the username: {Username}");
        }
    }
}
