using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace UnitTests
{
    public interface IServer
    {
        public TcpListener getServer();
        public StreamWriter getStreamWriter();
        public TextWriter GetTextWriter();
    }
}
