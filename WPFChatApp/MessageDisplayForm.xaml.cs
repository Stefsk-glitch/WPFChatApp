using System.Collections.Generic;
using System.Windows;

namespace WPFChatApp
{
    public partial class MessageDisplayForm : Window
    {
        public List<string> msgsList{ get; set; }
        public MessageDisplayForm()
        {
            msgsList = new List<string>();
            InitializeComponent();
        }

        public void setMessages()
        {
            foreach (var msg in msgsList)
            {
                loadedMessagesListBox.Items.Add(msg);
            }
        }
    }
}
