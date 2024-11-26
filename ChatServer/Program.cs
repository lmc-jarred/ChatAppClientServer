using System;
using System.Net;
using System.Net.Sockets;

namespace ChatServer
{
    class Program
    {
        static TcpListener _listener;

        static void Main(string[] args)
        {
            _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 7891);
            _listener.Start();

            TcpClient client = _listener.AcceptTcpClient();
            Console.WriteLine("Client has connected!");
        }
    }
}