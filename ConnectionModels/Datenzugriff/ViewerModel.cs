namespace Models.Datenzugriff
{
    public class ViewerModel
    {
        private int _id;
        private int _currency;

        private string _username;
        private string _permission;

        public ViewerModel()
        {
            _id         = int.MaxValue;
            _currency   = 0;

            _username   = "";
            _permission = "";
        }

        public ViewerModel(int id, string username, int currency, string permission)
        {
            Id          = id;
            Currency = currency;

            Username    = username;
            Permission  = permission;
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }



        public string Permission
        {
            get { return _permission; }
            set { _permission = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int Currency
        {
            get { return _currency; }
            set { _currency = value; }
        }
    }
}
