using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using System.Xml;
using MySql.Data.MySqlClient;


namespace ChatAdventureServer
{
    internal class MyDataBase
    {

        readonly MySqlConnection connection = new MySqlConnection("server=localhost;" +
                                                           "port=3306;" +
                                                           "username=root;" +
                                                           "password=;" +
                                                           "database=chatadventure");

        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }
        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public MySqlConnection GetConnection()
        {
            return (MySqlConnection)connection;
        }






        public static string ProfileData(string Login)
        {
            Console.WriteLine(Login);

            MyDataBase db = new MyDataBase();
            
            MySqlCommand command = new MySqlCommand("Select id, email, GameName FROM users WHERE login = @Login", db.GetConnection());

            command.Parameters.Add("@Login", MySqlDbType.VarChar).Value = Login;
            command.CommandType = CommandType.Text;


            /*
            MySqlDataReader reader;
            //reader = command.ExecuteReader(CommandBehavior.SequentialAccess); //suka hui znaet sho s nim

            if(reader.Read())
            {
                string GameName = reader["GameName"].ToString();
                Console.WriteLine(GameName);
            }



            try
            {
                //Console.WriteLine(reader.GetString(0));
                //string GameName = reader.GetString("GameName");
                //reader.Close();
                //return GameName;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
            
            */
            return null;
        }













        // Check account in DB
        public static bool ApplicationSignIn(string Login, string Password)
        {
            MyDataBase db = new MyDataBase();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT `login`, `pass` FROM `users` WHERE `login` = @Login AND `pass` = @Password", db.GetConnection());

            command.Parameters.Add("@Login", MySqlDbType.VarChar).Value = Login;
            command.Parameters.Add("@Password", MySqlDbType.VarChar).Value = Password;
            adapter.SelectCommand = command;

            try
            {
                adapter.Fill(table);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        // Registering
        public static bool ApplicationSignUp(string Login, string Email, string GameName, string Password)
        {
            MyDataBase db = new MyDataBase();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("INSERT INTO `users` (login,email,pass,GameName) VALUES (@log,@mail,@pass,@gamename); ", db.GetConnection());

            command.Parameters.Add("@log", MySqlDbType.VarChar).Value = Login;
            command.Parameters.Add("@mail", MySqlDbType.VarChar).Value = Email;
            command.Parameters.Add("@pass", MySqlDbType.VarChar).Value = Password;
            command.Parameters.Add("@gamename", MySqlDbType.VarChar).Value = GameName;

            adapter.SelectCommand = command;

            try
            {
                //
                // Uncomment this before release xD
                //
                //adapter.Fill(table);                
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

            return false;
        }

        
        public static string UserAlreadyIs(string Login, string Email, string GameName)
        {
            var obj = new Dictionary<string, string>()
            {
                ["Status"] = "exist"    
            };

            MyDataBase db = new MyDataBase();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            // Check login
            MySqlCommand commandLog = new MySqlCommand("SELECT `login` FROM `users` WHERE `login` = @Login", db.GetConnection());
            commandLog.Parameters.Add("@Login", MySqlDbType.VarChar).Value = Login;
            adapter.SelectCommand = commandLog;
            try
            {
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    obj.Add("Login", "Exist");
                }
                table.Clear();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

            // Check mail
            MySqlCommand commandMail = new MySqlCommand("SELECT `email` FROM `users` WHERE `email` = @mail", db.GetConnection());
            commandMail.Parameters.Add("@mail", MySqlDbType.VarChar).Value = Email;
            adapter.SelectCommand = commandMail;
            try
            {
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    obj.Add("Email", "Exist");
                }
                table.Clear();
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }

            // Check game name
            MySqlCommand commandGameName = new MySqlCommand("SELECT `GameName` FROM `users` WHERE `GameName` = @gamename", db.GetConnection());
            commandGameName.Parameters.Add("@gamename", MySqlDbType.VarChar).Value = GameName;
            adapter.SelectCommand = commandGameName;
            try
            {
                adapter.Fill(table);
                if (table.Rows.Count > 0)
                {
                    obj.Add("GameName", "Exist");

                }
                table.Clear();
            }
            catch (Exception)
            {
                //Console.WriteLine(error);
            }

            if (obj.Count == 1)
            {
                return null;
            }

            return JsonSerializer.Serialize(obj);
        }  
    }
}
