using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Collections;

namespace AccountingtestServer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IPAddress IpAddress = IPAddress.Any;

            Console.WriteLine(DateTime.Now + "\n#The server is running at port 4512.");
            TcpListener listener = new TcpListener(IpAddress, 4512);

            Console.WriteLine("#The local End point is  :" + listener.LocalEndpoint);
            Console.WriteLine("Listening.....");
            while (true)
            {
                try
                {
                    listener.Start();
                    Socket socket = listener.AcceptSocket();
                    Console.WriteLine(DateTime.Now + " #Connection accepted from " + socket.RemoteEndPoint);


                    byte[] MsgBytes = new byte[100];
                    socket.Receive(MsgBytes);

                    Console.Write(Encoding.UTF8.GetString(MsgBytes) + "\n");

                    //
                    // sending answer to client
                    //
                    //ASCIIEncoding asen = new ASCIIEncoding();
                    //socket.Send(asen.GetBytes("#The string was recieved by the server."));



                    /* clean up */
                    socket.Close();
                    listener.Stop();
                }
                catch (Exception e)
                {
                    Console.WriteLine("#Error..... " + e.StackTrace);
                }
            }
        }
    }
}













//DB db = new DB();
//DataTable table = new DataTable();
//MySqlDataAdapter adapter = new MySqlDataAdapter();

//MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `login` = @ul AND `password` = @up", db.GetConnection());
//command.Parameters.Add("@ul", MySqlDbType.VarChar).Value = userLogin;
//command.Parameters.Add("@up", MySqlDbType.VarChar).Value = userPassword;

//adapter.SelectCommand = command;
//adapter.Fill(table);

//if (table.Rows.Count > 0)
//    MessageBox.Show("Yes");
//else
//    MessageBox.Show("No");
