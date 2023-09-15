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
            // Generate a timestamp for the log file name
            string timestamp = DateTime.Now.ToString("dd-MM-yyy-HH.mm.ss");

            // Construct the log file path with the timestamp
            string logFileName = $"log_{timestamp}.txt";
            string logFilePath = Path.Combine(Environment.CurrentDirectory, logFileName);

            // Create a custom logger that writes to both the console and the log file
            CustomLogger logger = new CustomLogger(logFilePath);

            // Set the console output to the custom logger
            Console.SetOut(logger);

            TcpListener server = new TcpListener(IPAddress.Any, 12345);
            server.Start();
            Console.WriteLine("Server started.\nWaiting for connections...");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                clients.Add(client);

                Console.WriteLine($"Client connected: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");

                // Start a new task to handle communication with this client
                Task.Run(() => HandleClient(client));
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
            catch(IOException)
            {
                clients.Remove(client);
                Console.WriteLine($"Client disconnected: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
        }
    }
    class CustomLogger : TextWriter
    {
        private StreamWriter logFileWriter;
        private TextWriter originalConsoleWriter;

        public CustomLogger(string logFilePath)
        {
            logFileWriter = new StreamWriter(logFilePath, append: true);
            originalConsoleWriter = Console.Out;
        }

        public override void WriteLine(string value)
        {
            originalConsoleWriter.WriteLine(value);
            logFileWriter.WriteLine(value);
            logFileWriter.Flush();
        }

        public override Encoding Encoding => Encoding.UTF8;
    }
}
