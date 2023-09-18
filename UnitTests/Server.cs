using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class Server : IServer
    {
        public TcpListener getServer()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 12345);
            server.Start();

            return server;
        }


        /*static async Task HandleClient(TcpClient client)
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
                        clients.Remove(client);
                        Console.WriteLine($"Client disconnected: {((IPEndPoint)client.Client.RemoteEndPoint).Address}");
                        break;
                    }

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
        }*/
    }
}
    /*class CustomLogger : TextWriter
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
    }*/
//}
