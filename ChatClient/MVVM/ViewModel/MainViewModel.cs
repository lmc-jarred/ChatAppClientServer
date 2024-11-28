using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatClient.MVVM.Core;
using ChatClient.Net;

namespace ChatClient.MVVM.ViewModel
{
    public class MainViewModel
    {
        private Server _server;

        public RelayCommand ConnectToServerCommand { get; set; }
        public string Username { get; set; }

        public MainViewModel()
        {
            _server = new Server();
            _server.ConnectedEvent += UserConnected;
            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrWhiteSpace(Username));
        }

        private void UserConnected()
        {
            throw new NotImplementedException();
        }
    }
}
