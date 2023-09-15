using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

        private int seconds;
        private int minutes;
        private int hours;
        private int days;
        private int months;
        private int years;

        public MainWindow()
        {
            InitializeComponent();
            ConnectToServer();
            ReceiveMessages(); // Start receiving messages when the client is initialized
            updateTimeDate();
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
            updateTimeDate();
            string message = $"{hours}:{minutes}: " + messageTextBox.Text;
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
            updateTimeDate();
            string filePath = $"chat_messages_{days}-{months}-{years}-{hours}.{minutes}.{seconds}.txt";
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

            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    string[] lines = File.ReadAllLines(filePath);

                    // Create an instance of MessageDisplayForm
                    MessageDisplayForm messageDisplayForm = new MessageDisplayForm();

                    // Pass the loaded messages to the form
                    foreach (var line in lines)
                    {
                        messageDisplayForm.msgsList.Add(line);
                    }

                    // Show the form
                    messageDisplayForm.setMessages();
                    messageDisplayForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }


        private void updateTimeDate()
        {
            DateTime currentDate = DateTime.Now;
            this.hours = currentDate.Hour;
            this.minutes = currentDate.Minute;
            this.seconds = currentDate.Second;

            this.days = currentDate.Day;
            this.months = currentDate.Month;
            this.years = currentDate.Year;
        }
    }
}
