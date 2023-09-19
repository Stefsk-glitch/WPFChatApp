using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public interface IClient
    {
        //public NetworkStream getNetworkStream();
        //public StreamReader GetStreamReader();
        //public StreamWriter GetStreamWriter();
        public TcpClient GetTcpClient();
    }
}
