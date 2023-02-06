using MySql.Data.MySqlClient;
using Mysqlx.Connection;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Accountingtest
{
    internal class DB
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
    }
}
