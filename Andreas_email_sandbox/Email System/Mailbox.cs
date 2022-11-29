using MailKit;
using MailKit.Net.Imap;
using Org.BouncyCastle.Asn1.Cmp;
using System.Diagnostics;
using static Email_System.Data;

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
            RetrieveFolders();

            if (!Utility.connectedToInternet())
                newEmailBt.Enabled = false;
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

        //adds message to mailbox and checks for flags
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

        //toggle all functionality associated with e-mails
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
            
            if(Data.updatePending)
            {
                var task = Data.loadExistingMessages();
                task.Wait();

                Data.updatePending = false;
            }

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
                    int count = Data.UIMessages[folderIndex].Count;
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

        // open instance of newEmail form when the button is clicked (typeKey 0 = blank email)
        private void newEmailBt_Click(object sender, EventArgs e)
        {
            new newEmail(0).Show();
        }

        //method to refresh the current folder when button is clicked
        private void refreshBt_Click(object sender, EventArgs e)
        {
            try
            {
                this.Enabled = false;
                Utility.refreshCurrentFolder();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                this.Enabled = true;
            }
        }

        //refreshes and timer tick (10 s)
        private void refreshTimer_Tick(object sender, EventArgs e)
        {
           Utility.refreshCurrentFolder();
           Debug.WriteLine("refreshed the folder automatially");
        }

        //refreshed the current folder
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

        //adds or remove flag to message
        private void toggleFlag(object sender, EventArgs e)
        {
            try
            {
                int messageIndex = messagesDGV.CurrentCell.RowIndex;            //get messageindex
                Data.msg m = currentFolderMessages[messageIndex];               //retrieve the messae object
                string subject = messagesDGV[2, messageIndex].Value.ToString(); //get subject of message

                //if the message is not already flagged
                if (!subject.Contains("(FLAGGED)"))
                {
                    int fromFolderIndex = folderDGV.CurrentCell.RowIndex;       //get folderindex

                    var index = Data.UIMessages[fromFolderIndex].IndexOf(m);    //get index in the UImessages
                    m.flags = "(FLAGGED) " + m.flags;                           //add the flag locally

                    //update the message in both UI and pending
                    Data.UIMessages[fromFolderIndex][index] = m;
                    Data.pendingMessages[fromFolderIndex][index] = m;

                    //add message to flagged folder locally
                    int destFolderIndex = Data.existingFolders.IndexOf(Data.flaggedFolderName);
                    Data.UIMessages[destFolderIndex].Add(m);

                    //make the changes on server and refresh
                    server.addFlagServer(m.folder, index, m.uid);
                    refreshCurrentFolder();
                }

                //remove flag instead if the mail was already flagged
                else if (subject.Contains("(FLAGGED)"))
                {
                    int folderIndex = Data.existingFolders.IndexOf(Data.flaggedFolderName);
                    Data.UIMessages[folderIndex].Remove(m);
                    var index = Data.UIMessages[folderDGV.CurrentCell.RowIndex].IndexOf(m);

                    if (index != -1)
                    {
                        m.flags = m.flags.Replace("(FLAGGED)", "");
                        m.flags = m.flags.Replace("Flagged", "");
                        Data.UIMessages[folderDGV.CurrentCell.RowIndex][index] = m;
                        Data.pendingMessages[folderDGV.CurrentCell.RowIndex][index] = m;
                        refreshCurrentFolder();
                        server.removeFlagServer(m.folder, m.uid);
                    }

                    else
                    {
                        refreshCurrentFolder();
                        server.removeFlagServer(m.folder, m.uid);
                    }


                    refreshCurrentFolder();
                }
            }

            catch
            {
                MessageBox.Show("No message selected!");
            }

           
        }

        //deletes the selected message completely
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

        //moves selected message to trash.
        //calls utility function
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

        //if user presses 'close' igen instead of pressing logut button
        private void Mailbox_FormClosed(object sender, FormClosedEventArgs e)
        {
            logoutBt.PerformClick();
        }

        //closes mailbox form and cancels all backgroundworkers
        private void logoutBt_Click(object sender, EventArgs e)
        {
            login l = login.GetInstance;
            Settings s = Settings.GetInstance;

            if(!s.IsDisposed)
                s.Dispose();

            if (!l.folderListenerBW.CancellationPending)
            {
                l.folderListenerBW.CancelAsync();
                l.folderListenerBW.Dispose();
            }

            if (!l.messagesBackgroundWorker.CancellationPending)
            {
                l.messagesBackgroundWorker.CancelAsync();
                l.messagesBackgroundWorker.Dispose();
            }

            l.Show();

            if (Properties.Settings.Default.downloadMessagesEnabled)
                Data.saveMessages(Data.UIMessages);

            if (!Properties.Settings.Default.downloadMessagesEnabled)
            {
                Data.deleteFiles();
                Environment.Exit(0);
            }

            instance.Dispose();
        }
        //performing search in all emails
        private void search(string searchQuery)
        {

            List<Data.msg> searchResults = new List<Data.msg>();

            foreach(var folder in Data.UIMessages)
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

        //adding seach results to the messagesDGV
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

        //method called when searching in emails
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

        //event handler for pressing enter in the search field
        private void searchTb_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                searchBt.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        //sets the text of the logging label
        public static void setText(string message)
        {
            instance.logLabel.Text = message;
        }

        //method to read some message when it is double clicked
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

                    var index = Data.UIMessages[folderIndex].IndexOf(m);
                    m.flags = m.flags.Replace("(UNREAD)", "");
                    m.flags += ", Seen";
                    Data.UIMessages[folderIndex][index] = m;

                    server.markMsgAsReadServer(m.folder, m.uid);
                }

                new readMessage(m.body, m.from, m.to, m.date, m.subject, m.attachments, m.folder, m.uid).Show();
            }
        }

        //open the settings form
        private void settingsBt_Click(object sender, EventArgs e)
        {
            var s = Settings.GetInstance;
            s.Show();
        }

        //retrieve messages in selected folder on click
        private void folderDGV_CellClick(object sender = null!, DataGridViewCellEventArgs e = null!)
        {
            //check for stages chnages in data?
            //Data.existingMessages != Data.staged
            //then existing should be overwritten with stages 

            if (Data.updatePending)
            {
                var task = Data.loadExistingMessages();
                task.Wait();
                Data.updatePending = false;
            }

            messagesDGV.Rows.Clear();
            currentFolderMessages.Clear();

            int folder = folderDGV.CurrentCell.RowIndex;

            //folderDGV.SelectedCells
            string folderName = folderDGV.CurrentCell.Value.ToString();
            folderName = folderName.Remove(folderName.LastIndexOf(' '));

            try
            {
                if (Data.UIMessages[folder].Count <= 0)
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
                    {
                        var l = login.GetInstance;
                        if (l.folderListenerBW.IsBusy || l.messagesBackgroundWorker.IsBusy || !Utility.connectedToInternet())         //if we are downloading the e-mails or are disconnected
                        {
                            toggleButtons(false);
                            messagesDGV.Enabled = true;
                        }

                        else
                            toggleButtons(true);
                    }                     

                    foreach (var item in Data.UIMessages[folder])
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

    }
}
