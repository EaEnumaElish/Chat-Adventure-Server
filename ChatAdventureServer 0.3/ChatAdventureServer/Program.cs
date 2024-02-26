using System;

// ChatAdventureServer v0.3
namespace ChatAdventureServer
{
    internal class Program
    {
        static void Main()
        {
            Console.Title = "Server: Chat Adventure v 0.3";
            Console.ForegroundColor = ConsoleColor.Green;

            MyServerTcp.TcpUp();
        }
    }
}