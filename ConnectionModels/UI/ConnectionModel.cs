namespace Models
{
    public class ConnectionModel
    {
        private string _sState;
        private string _sOauth;
        private string _sUsername;
        private string _sChannel;

        public ConnectionModel()
        {
            _sState     = "Offline";
            _sOauth     = "";
            _sUsername  = "";
            _sChannel   = "";
        }

        public string State
        {
            get
            {
                return _sState;
            }
            set
            {
                if (value != _sState)
                    _sState = value;
            }
        }

        public string OauthKey
        {
            get
            {
                return _sOauth;
            }
            set
            {
                if (value != _sOauth)
                    _sOauth = value;
            }
        }

        public string Username
        {
            get
            {
                return _sUsername;
            }
            set
            {
                if (value != _sUsername)
                    _sUsername = value;
            }
        }

        public string Channel
        {
            get
            {
                return _sChannel;
            }
            set
            {
                if (value != _sChannel)
                    _sChannel = value;
            }
        }
    }
}
