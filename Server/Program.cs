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

                // Start a new task to handle communication with this client
                Task.Run(() => HandleClient(client));
            }

            while (true)
            {

            }
        }

        static async Task HandleClient(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                while (true)
                {
                    string message = await reader.ReadLineAsync();
                    if (message == null)
                    {
                        // Client disconnected
                        clients.Remove(client);
                        Console.WriteLine($"Client disconnected: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");
                        break;
                    }

                    // Relay the message to all other clients
                    foreach (var otherClient in clients)
                    {
                        if (otherClient != client)
                        {
                            StreamWriter writer = new StreamWriter(otherClient.GetStream(), Encoding.UTF8);
                            writer.WriteLine(message);
                            writer.Flush();
                        }
                    }

                    Console.WriteLine($"Received from {((IPEndPoint)client.Client.RemoteEndPoint).Address}: {message}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
        }
    }
}
