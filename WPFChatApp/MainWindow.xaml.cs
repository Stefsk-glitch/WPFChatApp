using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Save, SaveExecuted, SaveCanExecute));    // connects keyboard shortcuts
            CommandBindings.Add(new CommandBinding(ApplicationCommands.Open, OpenExecuted, OpenCanExecute));
            ConnectToServer();
            ReceiveMessages(); // starts receiving messages when the client is initialized
        }

        private void SaveExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            SaveMessages_Click(sender, e);
        }

        private void SaveCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            LoadMessages_Click(sender, e);
        }

        private void OpenCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                SendButton.IsEnabled = false;
                messageTextBox.Text = "No connection with server.";
                messageTextBox.IsEnabled = false;
            }
        }

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
            string timestamp = DateTime.Now.ToString("HH.mm");
            string message = timestamp + ": "+ messageTextBox.Text;
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
            string timestamp = DateTime.Now.ToString("dd-MM-yyy-HH.mm.ss");
            string messageFileName = $"messages_{timestamp}.txt";

            try
            {
                using (StreamWriter writer = File.CreateText(messageFileName))
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

                    MessageDisplayForm messageDisplayForm = new MessageDisplayForm();

                    foreach (var line in lines)
                    {
                        messageDisplayForm.msgsList.Add(line);
                    }

                    messageDisplayForm.setMessages();
                    messageDisplayForm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }
    }
}
