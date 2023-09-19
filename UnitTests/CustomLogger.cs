using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests
{
    public class CustomLogger : TextWriter, IDisposable
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

        public void Dispose()
        {
            // Close and dispose of the logFileWriter to release the file lock.
            logFileWriter.Close();
            logFileWriter.Dispose();
        }
    }
}
