/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;*/
using MailKit.Net.Imap;
using MailKit;
/*using MailKit.Search;
using MimeKit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Collections;
using Org.BouncyCastle.Asn1.X509;*/
using System.Threading;
using static System.Windows.Forms.AxHost;
using System.Windows.Forms;

namespace Email_System
{

    public partial class Mailbox : Form
    {
        string username;
        string password;
        string server;

        IList<IMessageSummary> messageSummaries = null!;

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
        private async void RetrieveFolders()
        {

            bool foldersLoaded = false;
            //new waitForm().Show();

            if (!foldersLoaded)
            {
                this.Cursor = Cursors.WaitCursor;
                //create instance of a IMAP client
                using var client = new ImapClient();
                {
                    //connect and authenticate to the server
                    await client.ConnectAsync("imap." + server, 993, true);

                    await client.AuthenticateAsync(username, password);

                    // get the folders from the server (to a List)
                    var folders = await client.GetFoldersAsync(new FolderNamespace('.', ""));

                    //dictionary for storing all folders
                    Dictionary<string, string> foldersMap = new Dictionary<string, string>();

                    // get the messages from the folder and add them to the dictionary
                    foreach (var item in folders)
                    {
                        if (item.Exists)
                        {
                            var unread_no = item.Unread.ToString();
                            var folderName = item.FullName.Substring(item.FullName.LastIndexOf('/') + 1) + "   (" + unread_no + ")";
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
                await client.DisconnectAsync(true);
                this.Cursor = Cursors.Default;
            }
            foldersLoaded = true;
        }

        // open instance of newEmail form when the button is clicked (typeKey 0 = blank email)
        private void newEmailBt_Click(object sender, EventArgs e)
        {
            new newEmail(username, password, 0).Show();
        }

        // method to retrieve the messages from the folder when this folder is double clicked
        private async void RetrieveMessages(object sender, MouseEventArgs e)
        {
            bool messagesLoaded = false;
            messageLb.Items.Clear();

            if (!messagesLoaded)
            {
                this.Cursor = Cursors.WaitCursor;

                using var client = new ImapClient();
                {
                    await client.ConnectAsync("imap." + server, 993, true);
                    await client.AuthenticateAsync(username, password);

                    var folder = await client.GetFolderAsync(((ListBox)sender).SelectedValue.ToString());

                    await folder.OpenAsync(FolderAccess.ReadWrite);

                    var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);                    

                    if (messages.Count <= 0)
                    {
                        messageLb.Items.Add("No messages in this folder!");
                        messageLb.Enabled = false;
                    }

                    else
                    {                        
                        messageSummaries = messages;

                        foreach (var item in messages.Reverse())
                        {
                            if(item.Flags.Value.HasFlag(MessageFlags.Flagged))
                            {
                                var sub = "(FLAGGED) " + item.Envelope.Subject;
                                messageLb.Items.Add(sub);
                            }

                            if (!(item.Flags.Value.HasFlag(MessageFlags.Seen)))
                            {
                                var sub = "(UNREAD) " + item.Envelope.Subject;
                                messageLb.Items.Add(sub);
                            }

                            else if (item.Envelope.Subject != null)
                                messageLb.Items.Add(item.Envelope.Subject);

                            else
                            {
                                item.Envelope.Subject = "<no subject>";
                                messageLb.Items.Add(item.Envelope.Subject);
                            }
                        }

                        messageLb.Enabled = true;
                    }

                }

                // disconnect from the client
                await client.DisconnectAsync(true);
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
                    await client.ConnectAsync("imap." + server, 993, true);
                    await client.AuthenticateAsync(username, password);

                    // get the value of the selected item
                    var messageItem = (((ListBox)sender).SelectedIndex);

                    var currentMessage = messageSummaries[messageSummaries.Count - messageItem - 1];

                    //add 'seen' flag to message
                    var folder = await client.GetFolderAsync(currentMessage.Folder.ToString());
                    await folder.OpenAsync(FolderAccess.ReadWrite);

                    await folder.AddFlagsAsync(currentMessage.UniqueId, MessageFlags.Seen, true);
                    

                    // create a new instance of the readMessage form with the retrieved message as input
                    new readMessage(currentMessage, username, password).Show();
                }

                await client.DisconnectAsync(true);
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
