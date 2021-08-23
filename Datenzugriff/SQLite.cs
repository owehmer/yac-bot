using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Models.Datenzugriff;

namespace Datenzugriff
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    class SQLite
    {
        private SQLiteConnection sqlConnection;

        private SQLiteCommand sqliteCommand;

        public SQLite(string pfad = "")
        {
            bool importDb = true;
			
            if (pfad == "")
            {
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\Data\Database");

                pfad = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + @"\Data\Database\UsersDB.mdf";

                importDb = false;
            }
            else
                pfad = "Data Source=" + pfad;

            sqlConnection = new SQLiteConnection(pfad);
            sqliteCommand = new SQLiteCommand(sqlConnection);

            if (!importDb)
                CreateDatabase();
        }

        private void CreateDatabase()
        {
            ExecuteCommand("CREATE TABLE IF NOT EXISTS `Users`"                                 +
                           "("                                                                  +
                                "`Id`           INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,"    +
                                "`Currency`     INTEGER NOT NULL,"                              +
                                "`Username`     TEXT    NOT NULL,"                              +
                                "`Permission`   TEXT"                                           +
                           ");");
        }

        public void OpenConnection()
        {
            try
            {
                sqlConnection.Open();
            }
            catch (InvalidOperationException)
            {
                sqlConnection.Close();
                OpenConnection();
            }
        }

        public void CloseConnection()
        {
            sqlConnection.Close();
        }

        public void ExecuteCommand(string query)
        {
            sqliteCommand.CommandText = query;

            try
            {
                sqliteCommand.ExecuteNonQuery();
            }
            catch (InvalidOperationException)
            {
                CloseConnection();
                OpenConnection();
                ExecuteCommand(query);
            }
        }

        public string GetCellOfRow(string row, string table, string whereParameter, string equals)
        {
            sqliteCommand.CommandText = "SELECT " + row + " FROM " + table +
                " WHERE " + whereParameter + " == '" + equals + "'";

            SQLiteDataReader reader = sqliteCommand.ExecuteReader();

            var results = new List<string>();

            while (reader.Read())
            {
                results.Add(reader[row].ToString());
            }

            reader.Close();

            if (results.Count > 0)
                return results[0];

            return "";
        }

        public List<ViewerModel> GetAll(string table, string db)
        {
            sqliteCommand.CommandText = "SELECT * FROM " + table;

            var results = new List<ViewerModel>();

            try
            {
                SQLiteDataReader reader = sqliteCommand.ExecuteReader();

                if (db == "own")
                    results = GetOwnDbResults(reader);

                else if (db == "modbot")
                    results = GetModBotResults(reader);
            }
            catch (SQLiteException) { }

            return results;
        }

        private List<ViewerModel> GetOwnDbResults(SQLiteDataReader reader)
        {
            var results = new List<ViewerModel>();

            while (reader.Read())
            {
                var newUser = new ViewerModel();

                for (int x = 0; x < reader.FieldCount; x++)
                {
                    if (reader.GetName(x) == "Id")
                        newUser.Id = reader.GetInt32(x);

                    else if (reader.GetName(x) == "Username")
                        newUser.Username = reader.GetString(x);
                    // Currency zu Int ändern
                    else if (reader.GetName(x) == "Currency")
                        newUser.Currency = reader.GetInt32(x);

                    else if (reader.GetName(x) == "Permission" && !reader.IsDBNull(x))
                        newUser.Permission = reader.GetString(x);

                    else if (reader.GetName(x) == "Permission" && reader.IsDBNull(x))
                        newUser.Permission = "";
                }
                results.Add(newUser);
            }

            reader.Close();

            return results;
        }

        private List<ViewerModel> GetModBotResults(SQLiteDataReader reader)
        {
            var results = new List<ViewerModel>();

            while (reader.Read())
            {
                var newUser = new ViewerModel();

                for (int x = 0; x < reader.FieldCount; x++)
                {
                    if (reader.GetName(x) == "id")
                        newUser.Id = reader.GetInt32(x);

                    else if (reader.GetName(x) == "user")
                        newUser.Username = reader.GetString(x);

                    else if (reader.GetName(x) == "currency")
                        newUser.Currency = reader.GetInt32(x);

                    else if (reader.GetName(x) == "userlevel")
                        newUser.Permission = reader.GetInt32(x).ToString();
                }
                results.Add(newUser);
            }

            reader.Close();

            return results;
        }

        public List<string> GetUserData(string query)
        {
            List<string> list = new List<string>();

            sqliteCommand.CommandText = query;
            try
            {
                SQLiteDataReader dr = sqliteCommand.ExecuteReader();

                while (dr.Read())
                {
                    list.Add(dr["username"] + ":" + dr["Currency"]);
                }

                dr.Close();

                return list;
            }
            catch (InvalidOperationException)
            {
                OpenConnection();

                return GetUserData(query);
            }
        }

        public bool UserExists(string username)
        {
            List<string> usersWithSameName =
                GetUserData("SELECT * FROM Users WHERE Username == '" + username + "'");

            if (usersWithSameName.Count == 0)
                return false;

            return true;
        }

        public void AddUser(string username, int currency = 0)
        {
            ExecuteCommand("INSERT INTO Users (Username, Currency) VALUES ('" + username + "', " + currency + ")");
        }

        public int GetCurrency(string username)
        {
            List<string> currentCurrency;

            currentCurrency = GetUserData("SELECT * FROM Users WHERE Username == '" + username + "'");

            if (currentCurrency.Count == 0)
            {
                AddUser(username);
                currentCurrency = GetUserData("SELECT * FROM Users WHERE Username == '" + username + "'");
            }

            Regex regex = new Regex("(.*):");

            string currencyString = regex.Replace(currentCurrency[0], "");

            int currencyInt = Convert.ToInt32(currencyString);

            return currencyInt;
        }

        public void SetCurrency(string username, int newCurrencyValue)
        {
            string query = "UPDATE Users SET Currency = '" + newCurrencyValue +
                           "' WHERE Username == '" + username + "' ";

            ExecuteCommand(query);
        }

        public void RemoveCurrency(string username, int amount)
        {
            int currentCurrency = GetCurrency(username);

            if (currentCurrency - amount > 0)
                SetCurrency(username, currentCurrency - amount);
            else
                SetCurrency(username, 0);
        }

        public void AddCurrency(string username, int amount)
        {
            int currentCurrency = GetCurrency(username);

            SetCurrency(username, currentCurrency + amount);
        }
    }
}
