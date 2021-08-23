using System.Windows.Input;
using Models;
using ViewModels.Commands;
using ViewModels.HelperClasses;

namespace ViewModels
{
    public class ConnectionViewModel : MyNotifyPropertyChanged
    {
        private ConnectionModel _connectionModel;
        private readonly ButtonCommand _objCommand;

        public ConnectionViewModel()
        {
            _connectionModel = new ConnectionModel();
            _objCommand = new ButtonCommand(this);
        }

        public ConnectionModel ConnectionModel
        {
            get
            {
                return _connectionModel;
            }
            set
            {
                _connectionModel = value;
                OnPropertyChanged("StatusModel");
            }
        }

        public string Status
        {
            get
            {
                return _connectionModel.State;
            }
            set
            {
                _connectionModel.State = value;
                OnPropertyChanged("Status");
            }
        }

        public string OauthKey
        {
            get
            {
                return _connectionModel.OauthKey;
            }
            set
            {
                _connectionModel.OauthKey = value;
                OnPropertyChanged("Oauth");
            }
        }

        public string Username
        {
            get
            {
                return _connectionModel.Username;
            }
            set
            {
                _connectionModel.Username = value;
                OnPropertyChanged("Username");
            }
        }

        public string Channel
        {
            get
            {
                return _connectionModel.Channel;
            }
            set
            {
                _connectionModel.Channel = value;
                OnPropertyChanged("Channel");
            }
        }

        public ICommand BtnClick
        {
            get
            {
                return _objCommand;
            }
        }
    }
}
