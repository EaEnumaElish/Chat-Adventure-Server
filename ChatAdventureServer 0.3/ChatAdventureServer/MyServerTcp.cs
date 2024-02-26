using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace ChatAdventureServer
{
    internal class MyServerTcp
    {
        public static void TcpUp()
        {
            // any ip address received
            IPAddress IpAddress = IPAddress.Any;

            // start up server
            Console.WriteLine("# Start time is: " + DateTime.Now);
            TcpListener listener = new TcpListener(IpAddress, 4512);

            Console.WriteLine("# The server is running at: " + listener.LocalEndpoint);
            Console.WriteLine("# Status: server is running\n");

            // listen on port
            listener.Start();

            // Receive message ( main loop )
            while (true)
            {
                try
                {
                    Socket socket = listener.AcceptSocket();

                    // Get connected
                    //Console.WriteLine(DateTime.Now + " #Connection accepted from " + socket.RemoteEndPoint);

                    // Read data
                    byte[] ReceivedMsg = new byte[1024];
                    int MsgSize = socket.Receive(ReceivedMsg);

                    byte[] MsgBytes = OnlyDataBytes(ReceivedMsg, MsgSize);

                    var readOnlySpan = new ReadOnlySpan<byte>(MsgBytes);
                    DetectStatus data = JsonSerializer.Deserialize<DetectStatus>(readOnlySpan);


                    switch (data.Status)
                    {
                        case "MainWinData":
                            Console.WriteLine(DateTime.Now + " | " + socket.RemoteEndPoint + " | Main win data | send");
                            
                            if(true)
                            {
                                // Take self data
                                ClientMainWinData dataMWD = JsonSerializer.Deserialize<ClientMainWinData>(readOnlySpan);

                                string GameName = MyDataBase.ProfileData(dataMWD.Login);
                                // create Json data packet
                                var obj = new Dictionary<string, string>
                                {
                                    ["Status"] = "MainWinData",
                                    ["GameName"] = GameName
                                };
                                string message = JsonSerializer.Serialize(obj);

                                // Send response
                                byte[] buffer = Encoding.ASCII.GetBytes(message);
                                socket.Send(buffer);
                            }
                            break;


                        case "connecting":
                            Console.WriteLine(DateTime.Now + " | " + socket.RemoteEndPoint + " | Connected");
                            break;

                        case "disconnecting":
                            Console.WriteLine(DateTime.Now + " | " + socket.RemoteEndPoint + " | Disconnected");
                            break;

                        case "sign_in":
                            ClientDataLogin dataLog = JsonSerializer.Deserialize<ClientDataLogin>(readOnlySpan);

                            bool CheckSignIn = MyDataBase.ApplicationSignIn(dataLog.Login, dataLog.Password);
                            if (CheckSignIn == true)
                            {
                                Console.WriteLine(DateTime.Now + " | " + socket.RemoteEndPoint + " | Sign in | Success");

                                // create Json data packet
                                var obj = new Dictionary<string, string>
                                {
                                    ["Status"] = "welcome"
                                };
                                string message = JsonSerializer.Serialize(obj);

                                // Send response
                                byte[] buffer = Encoding.ASCII.GetBytes(message);
                                socket.Send(buffer);
                            }
                            else
                            {
                                Console.WriteLine(DateTime.Now + " | " + socket.RemoteEndPoint + " | Sign in | Declined");
                            }
                            break;

                        case "loggout":
                            Console.WriteLine(DateTime.Now + " | " + socket.RemoteEndPoint + " | Logged out");
                            break;

                        case "sign_up":
                            ClientDataSignUp dataSignUp = JsonSerializer.Deserialize<ClientDataSignUp>(readOnlySpan);

                            string AlreadyIs = MyDataBase.UserAlreadyIs(dataSignUp.Login, dataSignUp.Email, dataSignUp.GameName);
                            if (AlreadyIs != null)
                            {
                                Console.WriteLine(DateTime.Now + " | " + socket.RemoteEndPoint + " | Sign up | Exist");

                                // Send response
                                byte[] buffer = Encoding.ASCII.GetBytes(AlreadyIs);
                                socket.Send(buffer);
                                break;
                            }

                            bool CheckSignUn = MyDataBase.ApplicationSignUp(dataSignUp.Login, dataSignUp.Email, dataSignUp.Password, dataSignUp.GameName);

                            if (CheckSignUn == true)
                            {
                                Console.WriteLine(DateTime.Now + " | " + socket.RemoteEndPoint + " | Sign up | Success");

                                // create Json data packet
                                var obj = new Dictionary<string, string>
                                {
                                    ["Status"] = "success"
                                };
                                string message = JsonSerializer.Serialize(obj);

                                // Send response
                                byte[] buffer = Encoding.ASCII.GetBytes(message);
                                socket.Send(buffer);
                            }
                            else
                            {
                                Console.WriteLine(DateTime.Now + " | " + socket.RemoteEndPoint + " | Sign up | Declined");
                            }
                            break;
                    }

                    /* clean up and close */
                    socket.Close();
                }
                catch (Exception ex)
                {
                    listener.Stop();
                    Console.WriteLine("# Error.. :" + ex.StackTrace);
                    Console.ReadKey();
                    break;
                }
            }
        }
        // Cut received zero bytes
        static byte[] OnlyDataBytes(byte[] bytes_income, int msg_size)
        {
            byte[] bytes_data = new byte[msg_size];

            for (int i = 0; i < msg_size; i++)
            {
                bytes_data[i] = bytes_income[i];
            }

            return bytes_data;
        }
    }
    // Data structures classes | Detect what to do
    public class DetectStatus
    {
        public string Status { get; set; }
    }

    public class ClientMainWinData
    {
        public string Login { get; set; }
        public string GameName { get; set; }
    }

    // Data class which needed to receive data from client
    public class ClientDataLogin
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
    public class ClientDataSignUp
    {
        public string Login { get; set; }
        public string GameName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}