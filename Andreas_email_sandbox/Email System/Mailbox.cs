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
using Org.BouncyCastle.Asn1.X509;

namespace Email_System
{

    public partial class Mailbox : Form
    {
        string username;
        string password;
        string server;

        //constructor
        public Mailbox(string user, string pass)
        {
            InitializeComponent();
            username = user;
            password = pass;

            server = username.Substring(username.LastIndexOf("@") + 1);
            RetrieveFolders();
        }

        // method that retrieve folders and add the names to the listbox
        async void RetrieveFolders()
        {
            bool foldersLoaded = false;
            //new waitForm().Show();

            if (!foldersLoaded)
            {


                //create instance of a IMAP client
                using var client = new ImapClient();
                {
                    //connect and authenticate to the server
                    client.Connect("imap." + server, 993, true);

                    client.Authenticate(username, password);

                    // get the folders from the server (to a List)
                    var folders = await client.GetFoldersAsync(new FolderNamespace('.', ""));

                    //dictionary for storing all folders
                    Dictionary<string, string> foldersMap = new Dictionary<string, string>();

                    // get the messages from the folder and add them to the dictionary
                    foreach (var item in folders)
                    {
                        if (item.Exists)
                        {
                            var folderName = item.FullName.Substring(item.FullName.LastIndexOf('/') + 1);
                            foldersMap.Add(key: item.FullName, value: folderName);
                        }
                    }

                    // specify the data source for the listbox
                    folderLb.DataSource = new BindingSource(foldersMap, null);

                    // specify the display member and value member
                    folderLb.DisplayMember = "Value";
                    folderLb.ValueMember = "Key";


                }
                //disconnect from the client
                client.Disconnect(true);
                this.Cursor = Cursors.Default;
            }
            foldersLoaded = true;
        }

        // open instance of newEmail form when the button is clicked (typeKey = blank email)
        private void newEmailBt_Click(object sender, EventArgs e)
        {
            new newEmail(username, password, 0).Show();
        }

        // method to retrieve the messages from the folder when this folder is double clicked
        private async void RetrieveMessages(object sender, MouseEventArgs e)
        {
            bool messagesLoaded = false;

            if (!messagesLoaded)
            {
                this.Cursor = Cursors.WaitCursor;

                using var client = new ImapClient();
                {
                    client.Connect("imap." + server, 993, true);
                    client.Authenticate(username, password);

                    // get access to the selected folder
                    var folder = await client.GetFolderAsync(((ListBox)sender).SelectedValue.ToString());

                    // open the folder to read the messages
                    await folder.OpenAsync(FolderAccess.ReadOnly);

                    // the messages are stored in a dictionary with message-id's as keys and the subject as values
                    Dictionary<string, string> messageDictionary = new Dictionary<string, string>();

                    if (folder.Count <= 0)
                    {
                        messageDictionary.Add(key: "-1", value: "No messages in this folder");
                        messageLb.Enabled = false;
                    }

                    else
                    {
                        // get the messages from the folder and add them to the dictionary
                        foreach (var item in folder.Reverse())
                        {
                            if (!string.IsNullOrEmpty(item.Subject))
                            {
                                messageDictionary.Add(key: item.MessageId, value: item.Subject);
                            }
                            else
                            {
                                messageDictionary.Add(key: item.MessageId, value: "<no subject>");
                            }
                        }
                        messageLb.Enabled = true;
                    }
                    // specify the data source for the listbox
                    messageLb.DataSource = new BindingSource(messageDictionary, null);

                    // specify the display member and value member
                    messageLb.DisplayMember = "Value";
                    messageLb.ValueMember = "Key";
                }

                // disconnect from the client
                client.Disconnect(true);
            }
            this.Cursor = Cursors.Default;
            messagesLoaded = true;
        }

        // method to read the message when it is double clicked
        private async void ReadMessage(object sender, MouseEventArgs e)
        {
            bool messageLoaded = false;

            if (!messageLoaded) 
            {
                this.Cursor = Cursors.WaitCursor;

                using var client = new ImapClient();
                {
                    client.Connect("imap." + server, 993, true);
                    client.Authenticate(username, password);

                    // get the value of the selected item
                    var messageItem = (((ListBox)sender).SelectedValue);

                    // get access to the selected content of the message
                    var folder = client.GetFolder(folderLb.SelectedValue.ToString());
                    await folder.OpenAsync(FolderAccess.ReadOnly);

                    //get the message object from the key associated with the selected value
                    var message = folder.First(m => m.MessageId == messageItem.ToString());

                    // create a new instance of the readMessage form with the retrieved message as input
                    new readMessage(folder, message, username, password).Show();
                }

                client.Disconnect(true);
            }
            this.Cursor = Cursors.Default;
            messageLoaded = true;
        }

        private void refreshBt_Click(object sender, EventArgs e)
        {
            RetrieveFolders();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RetrieveFolders();
        }
    }
}
