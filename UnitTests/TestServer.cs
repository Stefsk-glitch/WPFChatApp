using Xunit;
using Moq;
using System.Net.Sockets;
using System.Net;

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

        // TODO: create client able to connect test

        // TODO: logger is able to open file and write to it.

        // TODO: check if server able to receive messages
    }
}
