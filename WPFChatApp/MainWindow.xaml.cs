using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WPFChatApp
{
    public partial class MainWindow : Window
    {
        private TcpClient client;
        private StreamWriter writer;
        private StreamReader reader;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
            ReceiveMessages(); // Start receiving messages when the client is initialized
        }

        private async void ConnectToServer()
        {
            try
            {
                client = new TcpClient("localhost", 12345);
                NetworkStream stream = client.GetStream();
                writer = new StreamWriter(stream, Encoding.UTF8);
                reader = new StreamReader(stream, Encoding.UTF8);

                await ReceiveMessages();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Add a method to receive messages from the server
        private async Task ReceiveMessages()
        {
            while (true)
            {
                try
                {
                    string message = await reader.ReadLineAsync();
                    if (message != null)
                    { 
                        chatListBox.Items.Add(message);
                    }
                }
                catch (IOException)
                {
                    MessageBox.Show("Connection to the server is lost.");
                    break;
                }
            }
        }

        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = messageTextBox.Text;
            if (!string.IsNullOrEmpty(message))
            {
                chatListBox.Items.Add("You -> " + message);
                writer.WriteLine(message);
                writer.Flush();
                messageTextBox.Clear();
            }
        }
    }
}
