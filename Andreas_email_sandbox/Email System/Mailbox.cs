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
using System.Collections;

namespace Email_System
{

    public partial class Mailbox : Form
    {
        string username;
        string password;
        string server;

        Dictionary<string, string> messages = new Dictionary<string, string>();

        public Mailbox(string user, string pass)
        {
            InitializeComponent();
            username = user;
            password = pass;

            server = username.Substring(username.LastIndexOf("@") + 1);

            RetrieveFolders();
        }

        async void RetrieveFolders()
        {
            using var client = new ImapClient();
            {
                client.Connect("imap." + server, 993, true);
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
            using var client = new ImapClient();
            {
                client.Connect("imap." + server, 993, true);
                client.Authenticate(username, password);

                var folder = await client.GetFolderAsync(((ListBox)sender).SelectedItem.ToString());

                await folder.OpenAsync(FolderAccess.ReadOnly);

                Dictionary<string, string> message_instance = new Dictionary<string, string>();

                foreach (var item in folder)
                    {
                        if (!string.IsNullOrEmpty(item.Subject))
                        {
                            message_instance.Add(key: item.MessageId, value: item.Subject);
                        }
                        else
                        {
                            message_instance.Add(key: item.MessageId, value: "<no subject>");
                        }
                    }

                    messageLb.DataSource = new BindingSource(message_instance, null);

                    messageLb.DisplayMember = "Value";
                    messageLb.ValueMember = "Key";

                    messages = message_instance;
                }

                client.Disconnect(true);
            
        }

        private async void ReadMessage(object sender, MouseEventArgs e)
        {
            using var client = new ImapClient();
            {
                client.Connect("imap." + server, 993, true);
                client.Authenticate(username, password);

                var messageItem = (((ListBox)sender).SelectedValue);

                //var messageId = messages.First(m => m.Value == messageItem.ToString());
                
                var folder = client.GetFolder(folderLb.SelectedItem.ToString());
                await folder.OpenAsync(FolderAccess.ReadOnly);

                var message = folder.First(m => m.MessageId == messageItem.ToString());

                new readMessage(message, username, password).Show();                
            }
        }
    }
}
