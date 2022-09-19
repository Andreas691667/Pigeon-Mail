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
                    folderLb.Items.Add(folder.FullName);
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
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate(username, password);

                var folder = await client.GetFolderAsync(((ListBox)sender).SelectedItem.ToString());

                await folder.OpenAsync(FolderAccess.ReadOnly);

                List<string> emaillist = new List<string>();

                foreach (var item in folder)
                {
                    emaillist.Add(item.Subject);
                }

                foreach (var item in emaillist)
                {
                    messageLb.Items.Add(item);
                }               

                client.Disconnect(true);
            }

        }
    }
}
