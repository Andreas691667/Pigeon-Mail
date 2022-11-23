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
        List<Data.msg> currentFolderMessages = new List<Data.msg>();

        // maintains singleton pattern
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

            try
            {
                var dt2 = DateTime.ParseExact(date, "dd-MM-yyyy HH:mm:ss", null);
                messagesDGV.Rows.Insert(messagesDGV.Rows.Count, item.folder, item.sender, subject, item.body, dt2);
            }

            catch
            {
                messagesDGV.Rows.Insert(messagesDGV.Rows.Count, item.folder, item.sender, subject, item.body);
            }


            currentFolderMessages.Add(item);
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
            //folderLb.Items.Clear();
            folderDGV.Rows.Clear();

            folderDGV.Columns[0].HeaderText = Utility.username;
            //folderDGV.Columns[0].HeaderCell.Visible = (true);

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
                    folderDGV.Rows.Add(folderString);
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
        // NOT USED ANYMORE!!!
/*        private void RetrieveMessages(object sender = null!, MouseEventArgs e = null!)
        {
            currentFolderMessages.Clear();  

            int folder = folderDGV.CurrentCell.RowIndex;

            //folderDGV.SelectedCells
            string folderName = folderDGV.CurrentCell.Value.ToString();            

            try
            {
                if (Data.existingMessages[folder].Count <= 0)
                {
                    toggleButtons(false);
                    Utility.logMessage("No messages in this folder!");
                }

                else
                {
                    if (folderName == Data.allFolderName)
                        toggleButtons(false);

                    else
                        toggleButtons(true);


                    //currentFolderMessages = Data.existingMessages[folder];

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

                    refreshCurrentFolder(0);

                }
            }

            catch
            {
                toggleButtons(false);
                Utility.logMessage("Messages are being fetched from the server! Please be patient:)");
            }
        }*/
        
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

        public static void refreshCurrentFolder(int flag = -1)
        {

            int folderRow = instance.folderDGV.CurrentCell.RowIndex;
            int folderColumn = instance.folderDGV.CurrentCell.ColumnIndex;

            instance.RetrieveFolders();

            instance.folderDGV.ClearSelection();
            instance.folderDGV.Rows[folderRow].Selected = true;

            instance.folderDGV.CurrentCell = instance.folderDGV.Rows[folderRow].Cells[folderColumn];

            Thread.Sleep(50);

            if (flag == -1)
                instance.folderDGV_CellClick();
        }

        // ----- ADD FLAG -----
        // DOCUMENTATION NEEDED
        private void addFlagBt_Click(object sender, EventArgs e)
        {
            try
            {
                // Get message index 
                int messageIndex = messagesDGV.CurrentCell.RowIndex;

                // Get message
                Data.msg m = currentFolderMessages[messageIndex];

                // Get subject
                string subject = messagesDGV[2, messageIndex].Value.ToString();

<<<<<<< HEAD
<<<<<<< HEAD
                // Flag if not already
                if (!subject.Contains("(FLAGGED)"))
                {
                    // Stop thread for 400 millis and ???
                    server.killListeners();

                    // Add FLAGGED to message flags
                    m.flags = "(FLAGGED) " + m.flags;

                    // Set message to flagged locally
                    var index = Data.existingMessages[folderLb.SelectedIndex].IndexOf(m);
                    Data.existingMessages[folderLb.SelectedIndex][index] = m;

                    // Folder index of the floagged folder
                    int folderIndex = Data.existingFolders.IndexOf(Data.flaggedFolderName);

                    // Add flagged message to local flagged folder
                    Data.existingMessages[folderIndex].Add(m);
=======

                if (!subject.Contains("(FLAGGED)"))
                {
                    int fromFolderIndex = folderDGV.CurrentCell.RowIndex;

                    //server.killListeners();

                    var index = Data.existingMessages[fromFolderIndex].IndexOf(m);
                    m.flags = "(FLAGGED) " + m.flags;
                    Data.existingMessages[fromFolderIndex][index] = m;
                    int destFolderIndex = Data.existingFolders.IndexOf(Data.flaggedFolderName);
                    //Data.existingMessages[destFolderIndex].Add(m);


                    server.addFlagServer(m.folder, index, m.uid);
>>>>>>> eef3a8c429bc2be0923a9a94b4d1ab246ba9ebad

                    // Why is this neccesary?
=======

                if (!subject.Contains("(FLAGGED)"))
                {
                    int fromFolderIndex = folderDGV.CurrentCell.RowIndex;

                    server.killListeners();

                    var index = Data.existingMessages[fromFolderIndex].IndexOf(m);
                    m.flags = "(FLAGGED) " + m.flags;
                    Data.existingMessages[fromFolderIndex][index] = m;
                    int destFolderIndex = Data.existingFolders.IndexOf(Data.flaggedFolderName);
                    Data.existingMessages[destFolderIndex].Add(m);
>>>>>>> 174adcb2ba206edea8016b69dcc787cbbcbbf1a0
                    refreshCurrentFolder();
<<<<<<< HEAD

                    // Add flag on server
                    server.addFlagServer(m.folder, index, m.uid);
=======
>>>>>>> eef3a8c429bc2be0923a9a94b4d1ab246ba9ebad
                }

                //we have a bug right here when removing flags from messages
                else if (subject.Contains("(FLAGGED)"))
                {
                    //server.killListeners();

                    Thread.Sleep(100);

                    int folderIndex = Data.existingFolders.IndexOf(Data.flaggedFolderName);
                    Data.existingMessages[folderIndex].Remove(m);
<<<<<<< HEAD

=======
>>>>>>> eef3a8c429bc2be0923a9a94b4d1ab246ba9ebad
                    var index = Data.existingMessages[folderDGV.CurrentCell.RowIndex].IndexOf(m);

                    if (index != -1)
                    {
                        m.flags = m.flags.Replace("(FLAGGED)", "");
                        m.flags = m.flags.Replace("Flagged", "");
                        Data.existingMessages[folderDGV.CurrentCell.RowIndex][index] = m;
                        refreshCurrentFolder();
<<<<<<< HEAD
                        server.removeFlagServer(m.folder, index);

                        return;
=======
                        server.removeFlagServer(m.folder, m.uid);
                    }

                    else
                    {
                        refreshCurrentFolder();
                        server.removeFlagServer(m.folder, m.uid);
>>>>>>> eef3a8c429bc2be0923a9a94b4d1ab246ba9ebad
                    }

                    refreshCurrentFolder();
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
            Settings s = Settings.GetInstance;

            if(!s.IsDisposed)
                s.Dispose();

            l.folderListenerBW.CancelAsync();

            if(!l.messagesBackgroundWorker.CancellationPending)
                l.messagesBackgroundWorker.CancelAsync();

            l.Show();

            if (Properties.Settings.Default.offlineModeEnabled)
                Data.saveMessages(Data.existingMessages);

            if (!Properties.Settings.Default.offlineModeEnabled)
            {
                Data.deleteFiles();
                Environment.Exit(0);
            }

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
            currentFolderMessages.Clear();

            if (msgs.Count <= 0)
            {
                toggleButtons(false);
                Utility.logMessage("No results!");
                messagesDGV.Rows.Clear();
            }

            else
            {
                messagesDGV.Rows.Clear();

                foreach (var item in msgs)
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
                new newEmail(4, null!, m.body, m.subject, m.to, m.from, m.cc, m.attachments, m.folder, m.uid, m.flags, m.sender).Show();
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

                    server.markMsgAsReadServer(m.folder, m.uid);
                }

                new readMessage(m.body, m.from, m.to, m.date, m.subject, m.attachments, m.folder, m.uid).Show();
            }
        }

        private void settingsBt_Click(object sender, EventArgs e)
        {
            var s = Settings.GetInstance;
            s.Show();
        }

        //retrieve messages in selected folder on click
        private void folderDGV_CellClick(object sender = null!, DataGridViewCellEventArgs e = null!)
        {

            messagesDGV.Rows.Clear();
            currentFolderMessages.Clear();

            int folder = folderDGV.CurrentCell.RowIndex;

            //folderDGV.SelectedCells
            string folderName = folderDGV.CurrentCell.Value.ToString();
            folderName = folderName.Remove(folderName.LastIndexOf(' '));


            try
            {
                if (Data.existingMessages[folder].Count <= 0)
                {
                    toggleButtons(false);
                    Utility.logMessage("No messages in this folder!");
                }

                else
                {
                    if (folderName == Data.allFolderName)
                    {
                        toggleButtons(false);
                        messagesDGV.Enabled = true;
                    }

                    else
                        toggleButtons(true);


                    //currentFolderMessages = Data.existingMessages[folder];

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

                    refreshCurrentFolder(0);
                }
            }

            catch
            {
                toggleButtons(false);
                Utility.logMessage("Messages are being fetched from the server! Please be patient:)");
            }
        }

        
        private void folderDGV_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            //folderDGV_CellClick();
        }
    }
}
