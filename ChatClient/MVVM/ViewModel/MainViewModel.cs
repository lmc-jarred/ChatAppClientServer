using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ChatClient.MVVM.Core;
using ChatClient.MVVM.Model;
using ChatClient.Net;

namespace ChatClient.MVVM.ViewModel
{
    public class MainViewModel
    {
        private Server _server;

        public RelayCommand ConnectToServerCommand { get; set; }
        public RelayCommand SendChatMessageCommand { get; set; }
        public ObservableCollection<UserModel> Users { get; set; }
        public string? Username { get; set; }
        public string? Message { get; set; }

        public MainViewModel()
        {
            _server = new Server();
            _server.ConnectedEvent += UserConnected;

            ConnectToServerCommand = new RelayCommand(o => _server.ConnectToServer(Username), o => !string.IsNullOrWhiteSpace(Username));
            SendChatMessageCommand = new RelayCommand(o => _server.SendChatMessage(Message), o => !string.IsNullOrWhiteSpace(Message));

            Users = new ObservableCollection<UserModel>();
        }

        private void UserConnected()
        {
            if (_server.PacketReader == null)
                throw new ApplicationException("Unable to read packets - PacketReader is null");

            UserModel user = new UserModel()
            {
                Username = _server.PacketReader.ReadMessage(),
                UID = _server.PacketReader.ReadMessage()
            };

            if (!Users.Any(x => x.UID == user.UID)) // TODO Advanced - Check for duplicate UIDs on server side
            {
                Application.Current.Dispatcher.Invoke(() => Users.Add(user));
            }
        }
    }
}
