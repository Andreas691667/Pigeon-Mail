using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Search;
using MimeKit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Email_System
{

    public partial class Mailbox : Form
    {
        string username;
        string password;

        Dictionary<string, string> messages = new Dictionary<string, string>();

        public Mailbox(string user, string pass)
        {
            InitializeComponent();
            username = user;
            password = pass;

            RetrieveFolders();
        }

        async void RetrieveFolders()
        {
            using var client = new ImapClient();
            {
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate(username, password);

                var folders = await client.GetFoldersAsync(new FolderNamespace(',', ""));

                foreach(var folder in folders)
                {
                    folderLb.Items.Add(folder.FullName.ToString());
                }

                client.Disconnect(true);
            }
        }

        private void newEmailBt_Click(object sender, EventArgs e)
        {
            new newEmail(username, password).Show();
        }

        private async void RetrieveMessages(object sender, MouseEventArgs e)
        {
            messageLb.Items.Clear();

            using var client = new ImapClient();
            {
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate(username, password);

                var folder = await client.GetFolderAsync(((ListBox)sender).SelectedItem.ToString());

                await folder.OpenAsync(FolderAccess.ReadOnly);

                Dictionary<string, string> message_instance = new Dictionary<string, string>();

                foreach (var item in folder)
                {
                    message_instance.Add(key: item.MessageId, value: item.Subject);
                }   
                
                foreach (var item in message_instance)
                {
                    if(!(string.IsNullOrEmpty(item.Value)))
                    {
                        messageLb.Items.Add(item.Value);
                    }

                    else
                    {
                        messageLb.Items.Add("<no subject>");
                    }
                    
                }

                messages = message_instance;

                client.Disconnect(true);
            }
        }

        private async void ReadMessage(object sender, MouseEventArgs e)
        {
            using var client = new ImapClient();
            {
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate(username, password);

                var messageItem = ((ListBox)sender).SelectedItem;

                var messageId = messages.First(m => m.Value == messageItem.ToString()).Key;                

                var folder = client.GetFolder(folderLb.SelectedItem.ToString());
                await folder.OpenAsync(FolderAccess.ReadOnly);

                var message = folder.First(m => m.MessageId == messageId);

                new readMessage(message).Show();                
            }
        }
    }
}
