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
            //create instance of a IMAP client
            using var client = new ImapClient();
            {
                //connect and authenticate to the server
                client.Connect("imap." + server, 993, true);
                
                client.Authenticate(username, password);

                // get the folders from the server (to a List)
                var folders = await client.GetFoldersAsync(new FolderNamespace(',', ""));

                //add all folders to a list
                List<string> foldersList = new List<string>();

                foreach(var folder in folders)
                {
                    foldersList.Add(folder.FullName.ToString());
                }
                
                // separate the items in folderLb by '/' and add them to the folder listbox
                foreach(string item in foldersList)
                {
                    string f = item.Substring(item.LastIndexOf("/") + 1);
                    folderLb.Items.Add(f);
                }            

                //disconnect from the client
                client.Disconnect(true);
            }
        }

        // open instance of newEmail form when the button is clicked (typeKey = blank email)
        private void newEmailBt_Click(object sender, EventArgs e)
        {
            new newEmail(username, password, 0).Show();
        }

        // method to retrieve the messages from the folder when this folder is double clicked
        private async void RetrieveMessages(object sender, MouseEventArgs e)
        {
            using var client = new ImapClient();
            {
                client.Connect("imap." + server, 993, true);
                client.Authenticate(username, password);

                //var folder = ((ListBox)sender).SelectedIndex.ToString();


                // get access to the selected folder
                var folder = await client.GetFolderAsync(((ListBox)sender).SelectedItem.ToString());

                // open the folder to read the messages
                await folder.OpenAsync(FolderAccess.ReadOnly);

                // the messages are stored in a dictionary with message-id's as keys and the subject as values
                Dictionary<string, string> message_instance = new Dictionary<string, string>();

                // get the messages from the folder and add them to the dictionary
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

                // specify the data source for the listbox
                messageLb.DataSource = new BindingSource(message_instance, null);

                // specify the display member and value member
                messageLb.DisplayMember = "Value";
                messageLb.ValueMember = "Key";

                //how do we revrese these items?
                //reverse the items in the listbox messageLb
               

                // update the global variable 'messages' to the current instance
                messages = message_instance;
                }

            // disconnect from the client
            client.Disconnect(true);
            
        }

        // method to read the message when it is double clicked
        private async void ReadMessage(object sender, MouseEventArgs e)
        {
            using var client = new ImapClient();
            {
                client.Connect("imap." + server, 993, true);
                client.Authenticate(username, password);

                // get the value of the selected item
                var messageItem = (((ListBox)sender).SelectedValue);

                // get access to the selected content of the message
                var folder = client.GetFolder(folderLb.SelectedItem.ToString());
                await folder.OpenAsync(FolderAccess.ReadOnly);

                //get the message object from the key associated with the selected value
                var message = folder.First(m => m.MessageId == messageItem.ToString());

                // create a new instance of the readMessage form with the retrieved message as input
                new readMessage(message, username, password).Show();                
            }
        }

        private void refreshBt_Click(object sender, EventArgs e)
        {
            folderLb.Items.Clear();
            RetrieveFolders();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            folderLb.Items.Clear();
            RetrieveFolders();
        }
    }
}
