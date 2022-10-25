
using MailKit.Net.Imap;
using MailKit;
using System.Threading;
using static System.Windows.Forms.AxHost;
using System.Windows.Forms;


namespace Email_System
{

    public partial class Mailbox : Form
    {

        IList<IMessageSummary> messageSummaries = null!;

        //constructor
        public Mailbox(string user, string pass)
        {
            InitializeComponent();

            RetrieveFolders();
        }

        // method that retrieve folders and add the names to the listbox
        private async void RetrieveFolders()
        {
            RetrieveInboxMessages();

            bool foldersLoaded = false;
            //new waitForm().Show();

            if (!foldersLoaded)
            {
                this.Cursor = Cursors.WaitCursor;

                var client = await Utility.establishConnectionImap();

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
                
                //disconnect from the client
                await client.DisconnectAsync(true);
                this.Cursor = Cursors.Default;
            }
            foldersLoaded = true;
        }


        private async void RetrieveInboxMessages()
        {
            bool messagesLoaded = false;
            messageLb.Items.Clear();

            if (!messagesLoaded)
            {
                this.Cursor = Cursors.WaitCursor;

                var client = await Utility.establishConnectionImap();
                var folder = client.Inbox;
                await folder.OpenAsync(FolderAccess.ReadOnly);

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
/*                        if (item.Flags.Value.HasFlag(MessageFlags.Flagged))
                        {
                            var sub = "(FLAGGED) " + item.Envelope.Subject;
                            messageLb.Items.Add(sub);
                        }*/

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

                await client.DisconnectAsync(true);
            }
            this.Cursor = Cursors.Default;
            messagesLoaded = true;
        }

        // open instance of newEmail form when the button is clicked (typeKey 0 = blank email)
        private void newEmailBt_Click(object sender, EventArgs e)
        {
            new newEmail(0).Show();
        }

        // method to retrieve the messages from the folder when this folder is double clicked
        private async void RetrieveMessages(object sender, MouseEventArgs e)
        {
            bool messagesLoaded = false;
            messageLb.Items.Clear();

            if (!messagesLoaded)
            {
                this.Cursor = Cursors.WaitCursor;                         

                var client = await Utility.establishConnectionImap();

                var folder = await client.GetFolderAsync(((ListBox)sender).SelectedValue.ToString());

                await folder.OpenAsync(FolderAccess.ReadOnly);

                var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                if (messages.Count <= 0)
                {
                    messageLb.Items.Add("No messages in this folder!");
                    messageLb.Enabled = false;
                }

                else
                {
                    addFlagBt.Visible = true;
                    messageSummaries = messages;

                    foreach (var item in messages.Reverse())
                    {
                        if (item.Flags.Value.HasFlag(MessageFlags.Flagged))
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

                var client = await Utility.establishConnectionImap();

                // get the value of the selected item
                var messageItem = (((ListBox)sender).SelectedIndex);

                var currentMessage = messageSummaries[messageSummaries.Count - messageItem - 1];

                //add 'seen' flag to message
                var folder = await client.GetFolderAsync(currentMessage.Folder.ToString());
                await folder.OpenAsync(FolderAccess.ReadWrite);

                await folder.AddFlagsAsync(currentMessage.UniqueId, MessageFlags.Seen, true);
                    

                // create a new instance of the readMessage form with the retrieved message as input
                new readMessage(currentMessage).Show();

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

        private async void addFlagBt_Click(object sender, EventArgs e)
        {
/*            var messageIndex = messageLb.SelectedIndex;
            var message = messageSummaries[messageSummaries.Count - messageIndex];

            var client = await Utility.establishConnectionImap();


            //add flag to message
            var folder = await client.GetFolderAsync(message.Folder.ToString());
            await folder.OpenAsync(FolderAccess.ReadWrite);

            await folder.AddFlagsAsync(message.UniqueId, MessageFlags.Flagged, true);

            var importantFolder = client.GetFolder(SpecialFolder.Flagged);

            importantFolder.Open(FolderAccess.ReadWrite);

            await folder.CopyToAsync(message.UniqueId, importantFolder);

            await client.DisconnectAsync(true);*/
        }
    }
}
