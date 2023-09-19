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

            return server;
        }
    }
}