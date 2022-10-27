
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
        private static Mailbox instance = null!;

        //constructor
        public Mailbox()
        {
            InitializeComponent();
            RetrieveFolders();
        }

        //ensures singleton pattern is maintained (only one instance at all times)
        public static Mailbox GetInstance
        {
            get
            {
                if (instance == null || instance.IsDisposed)
                {
                    instance = new Mailbox();
                }
                return instance;
            }
        }

        public async Task<IMailFolder> getCurrentFolder()
        {
            var client = await Utility.establishConnectionImap();

            var folder = await client.GetFolderAsync(folderLb.SelectedValue.ToString());

            return folder;
        }

        private void messageFlagCheck(IMessageSummary item)
        {
            string subject = "";

            if (item.Flags.Value.HasFlag(MessageFlags.Flagged))
            {
                subject += "(FLAGGED) ";
            }

            if(item.Flags.Value.HasFlag(MessageFlags.Draft))
            {
                subject += "(DRAFT) ";
            }

            if (!(item.Flags.Value.HasFlag(MessageFlags.Seen)))
            {
                subject += "(UNREAD) ";
            }

            if (item.Envelope.Subject != null)
            {
                subject += item.Envelope.Subject;
                messageLb.Items.Add(subject);
            }

            else
            {
                item.Envelope.Subject = "<no subject>";
                subject += item.Envelope.Subject;
                messageLb.Items.Add(subject);
            }
        }

        // method that retrieve folders and add the names to the listbox
        private async void RetrieveFolders()
        {
            RetrieveMessages();

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
                        //var unread_no = item.Unread.ToString();
                        var folderName = item.FullName.Substring(item.FullName.LastIndexOf('/') + 1);
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
                    addFlagBt.Visible = false;
                    removeFlagBt.Visible = false;
                    messageLb.Enabled = false;
                    messageLb.Items.Add("No messages in this folder!");
                    
                }

                else
                {
                    addFlagBt.Visible = true;
                    removeFlagBt.Visible = true;
                    messageSummaries = messages;                   

                    foreach (var item in messages.Reverse())
                    {
                        messageFlagCheck(item);
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

        private void openFolderAsDraft(IList<IMessageSummary> messages)
        {
            addFlagBt.Visible = false;
            removeFlagBt.Visible = false;
            messageSummaries = messages;

            foreach(var item in messages.Reverse())
            {
                string subject = "(DRAFT) ";

                if (item.Envelope.Subject != null)
                {
                    subject += item.Envelope.Subject;
                    messageLb.Items.Add(subject);
                }

                else
                {
                    item.Envelope.Subject += "<no subject>";
                    subject += item.Envelope.Subject;
                    messageLb.Items.Add(subject);
                }
            }
        }

        // method to retrieve the messages from the folder when this folder is double clicked
        private async void RetrieveMessages(object sender = null!, MouseEventArgs e = null!)
        {
            bool messagesLoaded = false;
            messageLb.Items.Clear();

            if (!messagesLoaded)
            {
                this.Cursor = Cursors.WaitCursor;                         

                var client = await Utility.establishConnectionImap();

                var folder = await getCurrentFolder();

                await folder.OpenAsync(FolderAccess.ReadOnly);

                var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                if (messages.Count <= 0)
                {
                    addFlagBt.Visible = false;
                    removeFlagBt.Visible = false;
                    deleteBt.Visible = false;
                    moveToTrashBt.Visible = false;
                    messageLb.Enabled = false;

                    messageLb.Items.Add("No messages in this folder!");
                    
                }

                if(folder.Attributes.HasFlag(FolderAttributes.Drafts))
                {
                    openFolderAsDraft(messages);
                }

                else
                {
                    addFlagBt.Visible = true;
                    removeFlagBt.Visible = true;
                    deleteBt.Visible = true;
                    moveToTrashBt.Visible = true;

                    messageSummaries = messages;                    

                    foreach (var item in messages.Reverse())
                    {
                        messageFlagCheck(item);
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

                if (folder.Attributes.HasFlag(FolderAttributes.Drafts))
                {
                    new newEmail(4, currentMessage).Show();
                }

                else
                {
                    await folder.AddFlagsAsync(currentMessage.UniqueId, MessageFlags.Seen, true);

                    // create a new instance of the readMessage form with the retrieved message as input
                    new readMessage(currentMessage).Show();

                }

                await client.DisconnectAsync(true);
            }
            
            this.Cursor = Cursors.Default;
            messageLoaded = true;
        }

        private void refreshBt_Click(object sender, EventArgs e)
        {
            Utility.refreshCurrentFolder();
        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            Utility.refreshCurrentFolder();
        }

        public static void refreshCurrentFolder()
        {
            instance.RetrieveMessages();
        }

        private async void addFlagBt_Click(object sender, EventArgs e)
        {
            try
            {
                var messageIndex = messageLb.SelectedIndex;
                var message = messageSummaries[messageSummaries.Count - messageIndex - 1];
                var client = await Utility.establishConnectionImap();

                //add flag to message
                var folder = await client.GetFolderAsync(message.Folder.ToString());
                await folder.OpenAsync(FolderAccess.ReadWrite);

                await folder.AddFlagsAsync(message.UniqueId, MessageFlags.Flagged, true);
                await client.DisconnectAsync(true);

                Utility.refreshCurrentFolder();
            }

            catch
            {
                MessageBox.Show("No message selected!");
            }
        }

        private async void removeFlagBt_Click(object sender, EventArgs e)
        {
            try
            {
                var messageIndex = messageLb.SelectedIndex;
                var message = messageSummaries[messageSummaries.Count - messageIndex - 1];
                var client = await Utility.establishConnectionImap();

                //add flag to message
                var folder = await client.GetFolderAsync(message.Folder.ToString());
                await folder.OpenAsync(FolderAccess.ReadWrite);

                await folder.RemoveFlagsAsync(message.UniqueId, MessageFlags.Flagged, true);
                await client.DisconnectAsync(true);

                Utility.refreshCurrentFolder();
            }

            catch
            {
                MessageBox.Show("No message selected!");
            }
        }

        private void deleteBt_Click(object sender, EventArgs e)
        {
            try
            {
                var messageIndex = messageLb.SelectedIndex;
                var message = messageSummaries[messageSummaries.Count - messageIndex - 1];

                Utility.deleteMessage(message);
            }

            catch
            {
                MessageBox.Show("No message selected!");
            }
        }

        private void moveToTrashBt_Click(object sender, EventArgs e)
        {
            try
            {
                var messageIndex = messageLb.SelectedIndex;
                var message = messageSummaries[messageSummaries.Count - messageIndex - 1];

                Utility.moveMessageToTrash(message);
            }

            catch
            {
                MessageBox.Show("No message selected!");
            }
        }

        private void Mailbox_FormClosed(object sender, FormClosedEventArgs e)
        {
            instance.Dispose();
            Environment.Exit(1);
        }
    }
}
