using MailKit;
using MailKit.Net.Imap;
using System.Diagnostics;

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

        // maintains singleton pattern
        private static Mailbox instance = null!;

        // Constructor
        private Mailbox()
        {
            InitializeComponent();

            //
            RetrieveFolders();
/*            Thread.Sleep(100);
            folderLb.SelectedIndex = 0;
            instance.folderLb.Update();
            instance.folderLb.Focus(); */
            //refreshCurrentFolder();
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

        private void addMessageToMailbox(Data.msg item)
        {
            string subject = "";

            string flagString = item.flags;

            if (flagString.Contains("Flagged") || flagString.Contains("(FLAGGED)"))
            {
                subject += "(FLAGGED) ";
            }

            if(flagString.Contains("Draft"))
            {
                subject += "(DRAFT) ";
            }

            if (!(flagString.Contains("Seen")))
            {
                string draftFolder = Data.draftFolderName;
                if (item.folder != draftFolder)
                    subject += "(UNREAD) ";
            }

            if (item.subject != "")
            {
                subject += item.subject;
            }

            else
            {
                item.subject = "<no subject>";
                subject += item.subject;
            }

            string date = item.date.Remove(item.date.LastIndexOf(' '));

            var dt2 = DateTime.ParseExact(date, "dd-MM-yyyy HH:mm:ss", null);

            messagesDGV.Rows.Insert(messagesDGV.Rows.Count, item.folder, item.sender, subject, item.body, dt2);

            messagesDGV.Sort(messagesDGV.Columns["Date"], System.ComponentModel.ListSortDirection.Descending);
        }

        private void toggleButtons(bool value)
        {
            addFlagBt.Visible = value;
            moveToTrashBt.Visible = value;
            deleteBt.Visible = value;
            messagesDGV.Enabled = value;
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
                                    //addMessageToMailbox(item);
                                }

                            }

                            await client.DisconnectAsync(true);
                        }
                        //this.Cursor = Cursors.Default;
                        this.Enabled = true;
                        messagesLoaded = true;*/

/*            foreach (var item in Data.existingMessages[0])
            {
                addMessageToMailbox(item);
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
                                    addMessageToMailbox(item);
                                }
                            }

                            // disconnect from the client
                            await client.DisconnectAsync(true);

                            messagesLoaded = true;
                        }

                    //    this.Cursor = Cursors.Default;
                        this.Enabled = true;*/

            messagesDGV.Rows.Clear();

            int folder = folderLb.SelectedIndex;

            try
            {

                if (Data.existingMessages[folder].Count <= 0)
                {
                    toggleButtons(false);
                    Utility.logMessage("No messages in this folder!");
                }

                else
                {
                    toggleButtons(true);

                    currentFolderMessages = Data.existingMessages[folder];

                    foreach (var item in Data.existingMessages[folder])
                    {
                        addMessageToMailbox(item);
                    }

                    foreach (DataGridViewRow row in messagesDGV.Rows)
                    {
                        string sub = row.Cells[2].Value.ToString();

                        if (sub.Contains("UNREAD"))
                        {
                            row.DefaultCellStyle.BackColor = Color.DarkSalmon;
                        }
                    }
                }
            }

            catch
            {
                toggleButtons(false);
                Utility.logMessage("Messages are being fetched from the server! Please be patient:)");
            }
        }

        // method to read the message when it is double clicked
        //NOT USED ANYMORE
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




/*            int messageIndex = messageLb.SelectedIndex;
            Data.msg m = currentFolderMessages[messageIndex];

            if (m.flags.Contains("Draft"))
            {
                new newEmail(4, null!, m.body, m.subject,m.to, m.from, m.cc, m.attachments, m.folder).Show();
            }

            else
            {
                string subject = messageLb.SelectedItem.ToString();
                if (subject.Contains("(UNREAD)"))
                {
                    var index = Data.existingMessages[folderLb.SelectedIndex].IndexOf(m);
                    m.flags = m.flags.Replace("(UNREAD)", "");
                    m.flags += ", Seen";
                    Data.existingMessages[folderLb.SelectedIndex][index] = m;
                }

                new readMessage(m.body, m.from, m.to, m.date, m.subject, m.attachments, m.folder, m.uid).Show();               
            }*/
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
           // Utility.refreshCurrentFolder();
           // this.SendToBack();
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
                int messageIndex = messagesDGV.CurrentCell.RowIndex;
                Data.msg m = currentFolderMessages[messageIndex];

                string subject = messagesDGV[2, messageIndex].Value.ToString();

                if (!subject.Contains("(FLAGGED)"))
                {
                    server.killListeners();

                    var index = Data.existingMessages[folderLb.SelectedIndex].IndexOf(m);
                    m.flags = "(FLAGGED) " + m.flags;

                    Data.existingMessages[folderLb.SelectedIndex][index] = m;

                    int folderIndex = Data.existingFolders.IndexOf(Data.flaggedFolderName);

                    Data.existingMessages[folderIndex].Add(m);

                    refreshCurrentFolder();

                    server.addFlagServer(m.folder, index);
                }

                //we have a bug right here when removing flags from messages
                else if (subject.Contains("(FLAGGED)"))
                {
                    server.killListeners();

                    int folderIndex = Data.existingFolders.IndexOf(Data.flaggedFolderName);
                    Data.existingMessages[folderIndex].Remove(m);

                    var index = Data.existingMessages[folderLb.SelectedIndex].IndexOf(m);

                    if (index != -1)
                    {
                        m.flags = m.flags.Replace("(FLAGGED)", "");
                        m.flags = m.flags.Replace("Flagged", "");
                        Data.existingMessages[folderLb.SelectedIndex][index] = m;
                        refreshCurrentFolder();
                        server.removeFlagServer(m.folder, index);
                        return;
                    }

                    refreshCurrentFolder();

                    /*else
                    {
                        foreach (var folder in Data.existingMessages)
                        {
                            foreach (var msg in folder)
                            {
                                if (msg.uid == m.uid)
                                {
                                    //message found
                                    m.flags = m.flags.Replace("(FLAGGED)", "");
                                    folderIndex = Data.existingMessages.IndexOf(folder);
                                    var msgIndex = Data.existingMessages[folderIndex].IndexOf(msg);
                                    Data.existingMessages[folderIndex][msgIndex] = m;

                                    server.removeFlagServer(m.folder, msgIndex);

                                }
                            }
                        }
                    }*/


                }
            }

            catch
            {
                MessageBox.Show("No message selected!");
            }
        }

        private void removeFlagBt_Click(object sender = null!, EventArgs e = null!)
        {
        }

        private void deleteBt_Click(object sender, EventArgs e)
        {
            try
            {
                int messageIndex = messagesDGV.CurrentCell.RowIndex;
                Data.msg m = currentFolderMessages[messageIndex];
                Utility.deleteMsg(m.uid, m.subject, m.folder);
            }

            catch(Exception ex)
            {
                Utility.logMessage("No message selected!");
                Debug.WriteLine(ex.Message);
            }         

        }

        private void moveToTrashBt_Click(object sender, EventArgs e)
        {
            try
            {
                int messageIndex = messagesDGV.CurrentCell.RowIndex;
                Data.msg m = currentFolderMessages[messageIndex];
                Utility.moveMsgTrash(m.uid, m.subject, m.folder);
            }

            catch(Exception ex)
            {
                Utility.logMessage("No message selected!");
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

        private void search(string searchQuery)
        {

            List<Data.msg> searchResults = new List<Data.msg>();

            foreach(var folder in Data.existingMessages)
            {
                foreach(var msg in folder)
                {
                    if (contentRBT.Checked)
                    {
                        if(msg.body.Contains(searchQuery))
                        {
                            searchResults.Add(msg);
                        }
                    }

                    else if (subjectRBT.Checked)
                    {
                        if (msg.subject.Contains(searchQuery))
                        {
                            searchResults.Add(msg);
                        }
                    }

                    else if (senderRBT.Checked)
                    {
                        if (msg.from.Contains(searchQuery))
                        {
                            searchResults.Add(msg);
                        }
                    }

                    else
                        MessageBox.Show("Please specify a search query");
                }
            }

            showSearchResults(searchResults);

        }

        private void showSearchResults(List<Data.msg> msgs)
        {
            currentFolderMessages = msgs;

            if (currentFolderMessages.Count <= 0)
            {
                toggleButtons(false);
                Utility.logMessage("No results!");
                messagesDGV.Rows.Clear();
            }

            else
            {
                messagesDGV.Rows.Clear();

                foreach (var item in currentFolderMessages)
                {
                    toggleButtons(true);
                    addMessageToMailbox(item);
                }
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

        public static void setText(string message)
        {
            instance.logLabel.Text = message;
        }

        private void messagesDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int messageIndex = messagesDGV.CurrentCell.RowIndex;
            Data.msg m = currentFolderMessages[messageIndex];

            if (m.flags.Contains("Draft"))
            {
                new newEmail(4, null!, m.body, m.subject, m.to, m.from, m.cc, m.attachments, m.folder, m.uid).Show();
            }

            else
            {
                string subject = messagesDGV[2, messageIndex].Value.ToString();

                Debug.WriteLine(subject);

                if (subject.Contains("(UNREAD)"))
                {
                    server.killListeners();

                    var folderIndex = Data.existingFolders.IndexOf(m.folder);

                    var index = Data.existingMessages[folderIndex].IndexOf(m);
                    m.flags = m.flags.Replace("(UNREAD)", "");
                    m.flags += ", Seen";
                    Data.existingMessages[folderIndex][index] = m;

                    server.markMsgAsReadServer(m.folder, index);
                }

                new readMessage(m.body, m.from, m.to, m.date, m.subject, m.attachments, m.folder, m.uid).Show();
            }
        }
    }
}
