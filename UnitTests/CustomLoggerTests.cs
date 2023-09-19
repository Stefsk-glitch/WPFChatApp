using System;
using System.IO;
using Moq;
using Xunit;

namespace UnitTests
{
    public class CustomLoggerTests : IDisposable
    {
        private readonly CustomLogger customLogger;
        private readonly string logFilePath;

        public CustomLoggerTests()
        {
            // Create a temporary log file path for testing.
            logFilePath = Path.Combine(Path.GetTempPath(), "test_log.txt");

            // Create an instance of CustomLogger for testing.
            customLogger = new CustomLogger(logFilePath);
        }

        public void Dispose()
        {
            // Clean up by disposing of the CustomLogger and deleting the test log file.
            customLogger.Dispose();
        }

        [Fact]
        public void TestLoggerFileCreatedAndTextWritten()
        {
            // Arrange
            const string logMessage = "Test log message";

            // Act
            customLogger.WriteLine(logMessage);

            // Assert
            // Check if the log file has been created.
            Assert.True(File.Exists(logFilePath));

            Dispose();

            // Read the contents of the log file.
            var fileContents = File.ReadAllText(logFilePath);

            // Verify that the message is written to the log file.
            Assert.Contains(logMessage, fileContents);

            File.Delete(logFilePath);
        }
    }
}
