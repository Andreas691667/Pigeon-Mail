using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using Org.BouncyCastle.Asn1.X509;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

/*
 Mailbox is the window where mails previews are shown, and partioned into folders
 
*/


namespace Email_System
{
    public partial class Mailbox : Form
    {
        // Data structure for storing summaries locally
        IList<IMessageSummary> messageSummaries = null!;

        // Data structure for ???
        List<Data.msg> currentFolderMessages = null!;

        // What does this do?
        private static Mailbox instance = null!;

        // Constructor
        private Mailbox()
        {
            InitializeComponent();

            // 
            RetrieveFolders();
        }

        // Ensures singleton pattern is maintained (only one instance at all times)
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

        private async Task<IMailFolder> getCurrentFolder(ImapClient client)
        {
            var folder = await client.GetFolderAsync(folderLb.SelectedValue.ToString());
            return folder;
        }

        private void messageFlagCheck(Data.msg item)
        {
            string subject = "";

            string flagString = item.flags;


            if (flagString.Contains("Flagged"))
            {
                subject += "(FLAGGED) ";
            }

            if(flagString.Contains("Draft"))
            {
                subject += "(DRAFT) ";
            }

            if (!(flagString.Contains("Seen")))
            {
                subject += "(UNREAD) ";
            }

            if (item.subject != "")
            {
                subject += item.subject;
                messageLb.Items.Add(subject);
            }

            else
            {
                item.subject = "<no subject>";
                subject += item.subject;
                messageLb.Items.Add(subject);
            }
        }

        private void toggleButtons(bool value)
        {
            //removeFlagBt.Visible = value;
            addFlagBt.Visible = value;
            moveToTrashBt.Visible = value;
            deleteBt.Visible = value;

            messageLb.Enabled = value;
        }

        // method that retrieve folders and add the names to the listbox
        private void RetrieveFolders()
        {
            /*            RetrieveInboxMessages();

                        bool foldersLoaded = false;

                        while (!foldersLoaded)
                        {
                            //this.Cursor = Cursors.WaitCursor;
                            this.Enabled = false;

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
                                    var folderName = item.FullName.Substring(item.FullName.LastIndexOf('/') + 1);
                                    foldersMap.Add(key: item.FullName, value: folderName);
                                }
                            }

                            // specify the data source for the listbox
                            folderLb.DataSource = new BindingSource(foldersMap, null);

                            // specify the display member and value member
                            folderLb.DisplayMember = "Value";
                            folderLb.ValueMember = "Key";

                            foldersLoaded = true;

                            //disconnect from the client
                            await client.DisconnectAsync(true);
                           // this.Cursor = Cursors.Default;
                            this.Enabled = true;
                        }  */




            
            folderLb.Items.Clear();

            // What is happening here?
            foreach (var f in Data.existingFolders)
            {
                string folderString = "";

                folderString += f;


                int folderIndex = Data.existingFolders.IndexOf(f);

                try
                {
                    int count = Data.existingMessages[folderIndex].Count;
                    folderString += " (" + count + ")";

                }

                catch
                {
                    folderString += " (0)";
                }

                finally
                {
                    folderLb.Items.Add(folderString);
                }
            }

        }
        private void RetrieveInboxMessages()
        {
            /*            bool messagesLoaded = false;
                        messageLb.Items.Clear();

                        if (!messagesLoaded)
                        {
                         //   this.Cursor = Cursors.WaitCursor;
                            this.Enabled = false;

                            var client = await Utility.establishConnectionImap();
                            var folder = client.Inbox;
                            await folder.OpenAsync(FolderAccess.ReadOnly);

                            var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                            if (folder.Count <= 0)
                            {
                                toggleButtons(false);
                                messageLb.Items.Add("No messages in this folder!");                    
                            }

                            else
                            {
                                toggleButtons(true);

                                messageSummaries = messages;                   

                                foreach (var item in messages.Reverse())
                                {
                                    //messageFlagCheck(item);
                                }

                            }

                            await client.DisconnectAsync(true);
                        }
                        //this.Cursor = Cursors.Default;
                        this.Enabled = true;
                        messagesLoaded = true;*/

/*            foreach (var item in Data.existingMessages[0])
            {
                messageFlagCheck(item);
            }*/

        }

        // open instance of newEmail form when the button is clicked (typeKey 0 = blank email)
        private void newEmailBt_Click(object sender, EventArgs e)
        {
            new newEmail(0).Show();
        }

        private void openFolderAsDraft(IList<IMessageSummary> messages, IMailFolder folder)
        {
            addFlagBt.Visible = false;
            removeFlagBt.Visible = false;

            moveToTrashBt.Visible = true;
            deleteBt.Visible = true;
            messageLb.Enabled = true;

            messageSummaries = messages;            

            foreach(var item in messages.Reverse())
            {
                if (!item.Flags.Value.HasFlag(MessageFlags.Draft))
                {
                    folder.AddFlags(item.UniqueId, MessageFlags.Draft, true);
                    folder.Expunge();
                }

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
        private void RetrieveMessages(object sender = null!, MouseEventArgs e = null!)
        {
            /*            bool messagesLoaded = false;            

                        while (!messagesLoaded)
                        {
                           // this.Cursor = Cursors.WaitCursor;
                            this.Enabled = false;

                            var client = await Utility.establishConnectionImap();

                            var folder = await getCurrentFolder(client);

                            await folder.OpenAsync(FolderAccess.ReadWrite);

                            var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                            messageLb.Items.Clear();

                            if (folder.Count <= 0)
                            {
                                toggleButtons(false);
                                messageLb.Items.Add("No messages in this folder!");
                            }

                            else if(folder.Attributes.HasFlag(FolderAttributes.Drafts))
                            {
                                openFolderAsDraft(messages, folder);
                                //await folder.ExpungeAsync();
                            }

                            else
                            {
                                toggleButtons(true);

                                messageSummaries = messages;                    

                                foreach (var item in messages.Reverse())
                                {
                                    messageFlagCheck(item);
                                }
                            }

                            // disconnect from the client
                            await client.DisconnectAsync(true);

                            messagesLoaded = true;
                        }

                    //    this.Cursor = Cursors.Default;
                        this.Enabled = true;*/


            messageLb.Items.Clear();
            int folder = folderLb.SelectedIndex;

            try
            {


                if (Data.existingMessages[folder].Count <= 0)
                {
                    toggleButtons(false);
                    messageLb.Items.Add("No messages in this folder!");
                }

                else
                {
                    toggleButtons(true);
                    currentFolderMessages = Data.existingMessages[folder];

                    foreach (var item in Data.existingMessages[folder])
                    {
                        messageFlagCheck(item);
                    }
                }
            }

            catch
            {
                toggleButtons(false);
                messageLb.Items.Add("Messages are being fetched from the server! Please be patient:)");
            }
        }

        // method to read the message when it is double clicked
        private void ReadMessage(object sender, MouseEventArgs e)
        {
            /*            if (!messageLoaded) 
                        {
                        //    this.Cursor = Cursors.WaitCursor;
                            this.Enabled = false;

                            var client = await Utility.establishConnectionImap();

                            // get the value of the selected item
                            var messageItem = (((ListBox)sender).SelectedIndex);

                            var currentMessage = messageSummaries[messageSummaries.Count - messageItem - 1];

                            //add 'seen' flag to message
                            var folder = await client.GetFolderAsync(currentMessage.Folder.ToString());
                            await folder.OpenAsync(FolderAccess.ReadWrite);

                            //if the message is draft, open as draft!
                            if (folder.Attributes.HasFlag(FolderAttributes.Drafts))
                            {
                                var body = (TextPart)folder.GetBodyPart(currentMessage.UniqueId, currentMessage.TextBody);
                                new newEmail(4, currentMessage, body.Text).Show();
                            }

                            else
                            {
                                await folder.AddFlagsAsync(currentMessage.UniqueId, MessageFlags.Seen, true);
                                // create a new instance of the readMessage form with the retrieved message as input
                                new readMessage(currentMessage).Show();
                            }

                            await client.DisconnectAsync(true);

                         //   this.Cursor = Cursors.Default;
                            this.Enabled = true;
                        }*/




            int messageIndex = messageLb.SelectedIndex;
            Data.msg m = currentFolderMessages[messageIndex];

            if (m.flags.Contains("Draft"))
            {
                new newEmail(4, null!, m.body, m.subject,m.to, m.from, m.cc, m.attachments, m.folder).Show();
            }

            //how do we remove the 'unread' part??
            else
            {
/*                m.flags = m.flags.Replace("(UNREAD)", "");

                int folder = folderLb.SelectedIndex;

                var msgs = Data.existingMessages[folder];
                var i = msgs.FindIndex[m];

                var i = Data.existingMessages[folder];*/
                
                new readMessage(m.body, m.from, m.to, m.date, m.subject, m.attachments, m.folder).Show();               
            }
        }

        private void refreshBt_Click(object sender, EventArgs e)
        {
            try
            {
        //        this.Cursor = Cursors.WaitCursor;
                this.Enabled = false;
                Utility.refreshCurrentFolder();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
       //         this.Cursor = Cursors.Default;
                this.Enabled = true;
            }

        }

        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            Utility.refreshCurrentFolder();
        }

        public static void refreshCurrentFolder()
        {
            int folderIndex = instance.folderLb.SelectedIndex;
            instance.RetrieveFolders();
            instance.folderLb.SelectedIndex = folderIndex;
            instance.folderLb.Update();
            instance.folderLb.Focus();

            Thread.Sleep(50);
            instance.RetrieveMessages();
        }

        private async void addFlagBt_Click(object sender, EventArgs e)
        {
            try
            {
                var messageIndex = messageLb.SelectedIndex;
                var message = messageSummaries[messageSummaries.Count - messageIndex - 1];

                if (message.Flags.Value.HasFlag(MessageFlags.Flagged))
                {
                    removeFlagBt_Click();
                    Debug.WriteLine("Removing flag");
                }

                else
                {

        //            this.Cursor = Cursors.WaitCursor;
                    this.Enabled = false;
                    var client = await Utility.establishConnectionImap();

                    //add flag to message
                    var folder = await client.GetFolderAsync(message.Folder.ToString());
                    await folder.OpenAsync(FolderAccess.ReadWrite);
                    await folder.AddFlagsAsync(message.UniqueId, MessageFlags.Flagged, true);

                    Utility.refreshCurrentFolder();

                    await client.DisconnectAsync(true);
                    //this.Enabled = true;
                }
            }

            catch
            {
                MessageBox.Show("No message selected!");
            }
        }

        private async void removeFlagBt_Click(object sender = null!, EventArgs e = null!)
        {
            try
            {
                var messageIndex = messageLb.SelectedIndex;
                var message = messageSummaries[messageSummaries.Count - messageIndex - 1];

        //        this.Cursor = Cursors.WaitCursor;
                this.Enabled = false;

                var client = await Utility.establishConnectionImap();

                //add flag to message
                var folder = await client.GetFolderAsync(message.Folder.ToString());
                await folder.OpenAsync(FolderAccess.ReadWrite);

                await folder.RemoveFlagsAsync(message.UniqueId, MessageFlags.Flagged, true);

                Utility.refreshCurrentFolder();

                await client.DisconnectAsync(true);

                //this.Enabled = true;
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
                int messageIndex = messageLb.SelectedIndex;
                Data.msg m = currentFolderMessages[messageIndex];
                Utility.deleteMsg(m.uid, m.subject, m.folder);
            }

            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //MessageBox.Show("No message selected!");
            }         

        }

        private void moveToTrashBt_Click(object sender, EventArgs e)
        {
            try
            {
                int messageIndex = messageLb.SelectedIndex;
                Data.msg m = currentFolderMessages[messageIndex];

                this.Enabled = false;

                Utility.moveMsgTrash(m.uid, m.subject, m.folder);
            }

            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void Mailbox_FormClosed(object sender, FormClosedEventArgs e)
        {
            logoutBt.PerformClick();
        }

        private void logoutBt_Click(object sender, EventArgs e)
        {
            login l = login.GetInstance;
            l.Show();

            l.folderListenerBW.CancelAsync();
            Data.saveMessages(Data.existingMessages);

            instance.Dispose();
        }

        private async void search(string searchQuery)
        {
   //         this.Cursor = Cursors.WaitCursor;
            this.Enabled = false;

            var client = await Utility.establishConnectionImap();

            var folder = await getCurrentFolder(client);

            folder.Open(FolderAccess.ReadWrite);

            if (contentRBT.Checked)
            {
                var uids = folder.Search(SearchQuery.BodyContains(searchQuery));
                searchresults(uids, folder);
            }

            else if (subjectRBT.Checked)
            {
                var uids = folder.Search(SearchQuery.SubjectContains(searchQuery));
                searchresults(uids, folder);
            }

            else if (senderRBT.Checked)
            {
                var uids = folder.Search(SearchQuery.FromContains(searchQuery));
                searchresults(uids, folder);
            }

            else
                MessageBox.Show("Please specify a search query");

   //         this.Cursor = Cursors.Default;
            this.Enabled = true;
        }

        private void searchresults(IList<UniqueId> uids, IMailFolder folder)
        {
            messageLb.Items.Clear();
            IList<IMessageSummary> messages = folder.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

            messageSummaries = messages;

            if (messages.Count <= 0)
            {
                toggleButtons(false);
                messageLb.Items.Add("No results!");
            }

            foreach (var item in messages.Reverse())
            {
                toggleButtons(true);
                //messageFlagCheck(item);
            }
        }

        private void searchBt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(searchTb.Text))
            {
                string searchQuery = searchTb.Text;
                search(searchQuery);
            }

            else
            {
                MessageBox.Show("No search query");
            }
        }

        private void searchTb_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                searchBt.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private void Mailbox_EnabledChanged(object sender, EventArgs e)
        {
            if (this.Enabled == false)
                this.Cursor = Cursors.WaitCursor;

            else
                this.Cursor = Cursors.Default;
        }
    }
}
