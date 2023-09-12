using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Server
{
    class Program
    {
        static List<TcpClient> clients = new List<TcpClient>();
        static async Task Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 12345);
            server.Start();
            Console.WriteLine("Server started.\nWaiting for connections...");

            while (clients.Count < 2)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                clients.Add(client);

                Console.WriteLine($"Client connected: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");
            }

            // TODO: De clients moeten berichten naar elkaar kunnen sturen en van elkaar kunnen ontvangen na dat er 2 connecties opgebouwt zijn.
        }
    }
}
