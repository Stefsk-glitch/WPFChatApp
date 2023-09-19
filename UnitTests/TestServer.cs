using Xunit;
using Moq;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace UnitTests
{
    public class TestServer
    {
        [Fact]
        public void TestCreateServer()
        {
            // Arrange
            var mock = new Mock<IServer>();
            mock.Setup(test => test.getServer()).Returns(() => new TcpListener(IPAddress.Any, 12345)); // Set up the mock to return a TcpListener

            // Act
            IServer serverProvider = mock.Object;
            TcpListener server = serverProvider.getServer();

            // Assert
            Assert.NotNull(server); // Check if the server is not null
        }

        [Fact]
        public void TestClientConnectedAndReceiveMessage()
        {
            var mock = new Mock<IServer>();
            mock.Setup(test => test.getServer()).Returns(() => new TcpListener(IPAddress.Any, 12345));

            IServer mockServer = mock.Object;
            TcpListener server = mockServer.getServer();
            server.Start();

            TcpClient client = new TcpClient("localhost", 12345);

            Assert.True(client.Connected);

            try
            {
                string messageToSend = "test message";

                // Client sends a message to the server
                using (NetworkStream clientStream = client.GetStream())
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes(messageToSend);
                    clientStream.Write(messageBytes, 0, messageBytes.Length);
                }

                // Server accepts the client connection
                TcpClient connectedClient = server.AcceptTcpClient();
                NetworkStream serverStream = connectedClient.GetStream();

                byte[] buffer = new byte[1024];
                int bytesRead = serverStream.Read(buffer, 0, buffer.Length);
                string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                // Assert that the received message matches the sent message
                Assert.Equal(messageToSend, receivedMessage);

                connectedClient.Close();
            }
            finally
            {
                client.Close();
                server.Stop();
            }
        }
    }
}
