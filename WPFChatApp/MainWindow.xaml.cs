using Microsoft.Win32;
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

        private void SaveMessages_Click(object sender, RoutedEventArgs e)
        {
            string filePath = "chat_messages.txt";
            try
            {
                using (StreamWriter writer = File.CreateText(filePath))
                {
                    foreach (var message in chatListBox.Items)
                    {
                        writer.WriteLine(message.ToString());
                    }
                }
                MessageBox.Show("Messages saved to file successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception)
            {
                MessageBox.Show("Error saving messages", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadMessages_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files (*.txt)|*.txt";

            openFileDialog.ShowDialog();
            string filePath = openFileDialog.FileName;
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                string messageText = string.Join(Environment.NewLine, lines);
                MessageBox.Show(messageText, "Loaded Messages", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception)
            {
                MessageBox.Show("error");
            }
        }
    }
}
