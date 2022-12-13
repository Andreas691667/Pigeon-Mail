using MailKit;
using MailKit.Net.Imap;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using static Email_System.Data;

/*
 Mailbox is the window where mails previews are shown, and partioned into folders
 
*/

namespace Email_System
{
    public partial class Mailbox : Form
    {

        // Data structure for the messages currently displayed
        List<Data.msg> currentFolderMessages = new List<Data.msg>();

        // maintains singleton pattern
        private static Mailbox instance = null!;

        // Constructor
        private Mailbox()
        {
            InitializeComponent();


            if (File.Exists(Utility.username + "messages.json"))
                retrieveInboxMessages();

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
                var dt2 = DateTime.Parse(date);
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
            //deleteFolderBt.Visible = value;
            //addFolderBt.Visible = value;
            //newFolderTB.Visible = value;
            moveMessageBt.Visible = value;
            folderDropDown.Visible = value;
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
            folderDropDown.Items.Clear();
            folderDropDown.Text = "";

            folderDGV.Columns[0].HeaderText = Utility.username;
            //folderDGV.Columns[0].HeaderCell.Visible = (true);

            // What is happening here?
            foreach (var f in Data.existingFolders)
            {
                string folderString = "";

                folderString += f;

                string dropDownString = folderString;

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

                    // only add to dropdown if not already added and not one of these special folders:
                    if (!folderDropDown.Items.Contains(dropDownString) && dropDownString != Data.trashFolderName 
                        && dropDownString != Data.draftFolderName && dropDownString != Data.flaggedFolderName 
                        && dropDownString != Data.allFolderName)
                    {
                        folderDropDown.Items.Add(dropDownString);
                    }
                        

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
           //Utility.refreshCurrentFolder();
           //Debug.WriteLine("refreshed the folder automatially");
        }

        //refreshed the current folder
        public static void refreshCurrentFolder(int flag = -1)
        {

            int folderRow = instance.folderDGV.CurrentRow.Index;
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
/*                    Data.changedUids.Add(m.uid);
*/
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
                    //ulong? gmailMessageId = m.gmailMessageId;

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

                    //if the message is unflagged from flagged folder
                    else
                    {
                        for (int folder = 0; folder < Data.UIMessages.Count; folder++)
                        {
                            for (int msg = 0; msg < Data.UIMessages[folder].Count; msg ++)
                            {
                                Data.msg curMsg = Data.UIMessages[folder][msg];

                                // if(curMsg.gmailMessageId == gmailMessageId)
                                if (curMsg.subject == m.subject && curMsg.body == m.body && curMsg.cc == m.cc && curMsg.from == m.from
                                    && curMsg.to == m.to && curMsg.sender == m.sender && curMsg.date == m.date)
                                {
                                    curMsg.flags = curMsg.flags.Replace("(FLAGGED)", "");
                                    curMsg.flags = curMsg.flags.Replace("Flagged", "");
                                    Data.UIMessages[folder][msg] = curMsg;
                                    Data.pendingMessages[folder][msg] = curMsg;
                                }
                            }
                        }

                        refreshCurrentFolder();
                        server.removeFlagServer(m.folder, m.uid);                        
                    }
                }
            }

            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                MessageBox.Show("No message selected!");
            }           
        }

        private void markMessageBt_Click(object sender, EventArgs e)
        {
            try
            {
                int messageIndex = messagesDGV.CurrentCell.RowIndex;    // get messageindex
                Data.msg m = currentFolderMessages[messageIndex];       // retrieve the message object
                string subject = messagesDGV[2, messageIndex].Value.ToString(); // get subject of message
                Debug.WriteLine(subject);

                // if message is unread
                if (subject.Contains("(UNREAD)"))
                {
                    server.killListeners();

                    var folderIndex = Data.existingFolders.IndexOf(m.folder);

                    var index = Data.UIMessages[folderIndex].IndexOf(m);
                    m.flags = m.flags.Replace("(UNREAD)", "");
                    m.flags += ", Seen";

                    // update the message in both UI and pending
                    Data.UIMessages[folderIndex][index] = m;
                    Data.pendingMessages[folderIndex][index] = m;

                    // update on server
                    server.markMsgAsReadServer(m.folder, m.uid);
                }

                // if message is read
                else if (!subject.Contains("(UNREAD)"))
                {
                    server.killListeners();

                    var folderIndex = Data.existingFolders.IndexOf(m.folder);

                    var index = Data.UIMessages[folderIndex].IndexOf(m);
                    m.flags = m.flags.Replace("", "(UNREAD)");
                    m.flags = m.flags.Replace(", Seen", "");

                    // update the message in both UI and pending
                    Data.UIMessages[folderIndex][index] = m;
                    Data.pendingMessages[folderIndex][index] = m;

                    // update on server
                    server.markMsgAsUnreadServer(m.folder, m.uid);
                }
            }

            catch (Exception ex)
            {
                Utility.logMessage("No message selected!", 3000);
                Debug.WriteLine(ex.Message);
            }
        }

        //deletes the selected message completely
        private void deleteBt_Click(object sender, EventArgs e)
        {
            try
            {
                int messageIndex = messagesDGV.CurrentCell.RowIndex;
                Data.msg m = currentFolderMessages[messageIndex];
                Utility.deleteMsg(m.uid, m.folder);
            }

            catch(Exception ex)
            {
                Utility.logMessage("No message selected!", 3000);
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
                Utility.logMessage("No message selected!", 3000);
                Debug.WriteLine(ex.Message);
            }
        }

        //if user presses 'close' igen instead of pressing logut button
        private void Mailbox_FormClosed(object sender, FormClosedEventArgs e)
        {

            //logoutBt.PerformClick();
            logoutBt_Click(null, null);
        }

        //closes mailbox form and cancels all backgroundworkers
        private void logoutBt_Click(object sender, EventArgs e)
        {
            login l = login.GetInstance;
            Settings s = Settings.GetInstance;

            //if we are currently downloading user's messages
            if(l.messagesBackgroundWorker.IsBusy && Properties.Settings.Default.downloadMessagesEnabled)
            {
                DialogResult d = MessageBox.Show("We are currently fetching your messages. Do you wish to  continue in background (YES) or cancel (NO)?", "error", MessageBoxButtons.YesNo);

                //if continue in background
                if(d == DialogResult.Yes)
                {
                    instance.Dispose();
                    l.Hide();
                    s.Close();
                    return;
                }

                //else delete everything
                else
                {
                    Data.deleteFiles();
                    Environment.Exit(0);
                }
            }


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
            {
                Data.saveMessages(Data.UIMessages);
                Data.saveFolders(Data.existingFolders);
                Data.saveBlackListFile();
            }

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
                        //if(msg.body.Contains(searchQuery))
                        if (msg.body.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                        {
                            searchResults.Add(msg);
                        }
                    }

                    else if (subjectRBT.Checked)
                    {
                        //if (msg.subject.Contains(searchQuery))
                        
                        if(msg.subject.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                        {
                            searchResults.Add(msg);
                        }
                    }

                    else if (senderRBT.Checked)
                    {
                        //if (msg.from.Contains(searchQuery))
                        if (msg.from.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
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
                Utility.logMessage("No results!", 4000);
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
                    Data.pendingMessages[folderIndex][index] = m;

                    server.markMsgAsReadServer(m.folder, m.uid);
                }

                new readMessage(m.body, m.from, m.to, m.cc, m.date, m.subject, m.attachments, m.folder, m.uid).Show();
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

            //int folder = folderDGV.CurrentCell.RowIndex;
            int folder = folderDGV.CurrentRow.Index;

            //folderDGV.SelectedCells
            string folderName = folderDGV.CurrentCell.Value.ToString();
            folderName = folderName.Remove(folderName.LastIndexOf(' '));

            try
            {
                if (Data.UIMessages[folder].Count <= 0)
                {
                    toggleButtons(false);
                    Utility.logMessage("No messages in this folder!", 3000);
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
                        if (l.messagesBackgroundWorker.IsBusy || !Utility.connectedToInternet())         //if we are downloading the e-mails or are disconnected
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
                Utility.logMessage("Messages are being fetched from the server! Please be patient:)", 4000);
            }
        }


        private async void retrieveInboxMessages()
        {

            //check for stages chnages in data?
            //Data.existingMessages != Data.staged
            //then existing should be overwritten with stages 
            await Data.loadExistingMessages();

            messagesDGV.Rows.Clear();
            currentFolderMessages.Clear();

            //int folder = folderDGV.CurrentCell.RowIndex;
            int folder = 0;

            try
            {
                if (Data.UIMessages[folder].Count <= 0)
                {
                    toggleButtons(false);
                    //Utility.logMessage("No messages in this folder!");
                }

                else
                {
                    var l = login.GetInstance;
                    if (l.messagesBackgroundWorker.IsBusy || !Utility.connectedToInternet())         //if we are downloading the e-mails or are disconnected
                    {
                        toggleButtons(false);
                        messagesDGV.Enabled = true;
                    }

                    else
                        toggleButtons(true);

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
                }
            }

            catch
            {
                toggleButtons(false);
                //Utility.logMessage("Messages are being fetched from the server! Please be patient:)");
            }


        }

        //creates new folder
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(newFolderTB.Text))
                {
                    MessageBox.Show("Please enter a foldername!");
                    return;
                }

                server.killListeners();
                string folderName = newFolderTB.Text;
                createFolder(folderName);
            }

            catch(Exception ex)
            {
                MessageBox.Show("Please enter a foldername!");
            }

            finally
            {
                newFolderTB.Clear();
                server.startListeners();
                Utility.logMessage("Creating folder. This might take a while", 10000);
            }
        }

        private async void createFolder(string name)
        {
            var c = await Utility.establishConnectionImap();
            var toplevel = c.GetFolder(c.PersonalNamespaces[0]);
            toplevel.Create(name, true);
        }

        private void moveMessageBt_Click(object sender, EventArgs e)
        {
            try
            {
                int messageIndex = messagesDGV.CurrentCell.RowIndex;
                Data.msg m = currentFolderMessages[messageIndex];
                Utility.moveMsg(m.uid, m.subject, m.folder);
            }

            catch (Exception ex)
            {
                Utility.logMessage("No message selected!", 3000);
                Debug.WriteLine(ex.Message);
            }
        }

        private async void deleteFolderBt_Click(object sender, EventArgs e)
        {
            try
            {
                Utility.logMessage("Deleting folder. This might take a while", 11000);

                server.killListeners();

                var digitsToRemove = new[] { '0','1','2','3','4','5','6','7','8','9','(',')'}; //remove count digits from string

                string folderName = folderDGV.CurrentCell.Value.ToString(); //get foldername (i.e. selected folder in DGV)

                folderName = folderName.TrimEnd(digitsToRemove);
                folderName = folderName.Trim();

                int folderIndex = Data.existingFolders.IndexOf(folderName); //get index of folder in list     

                //check if folder is empty
                if (Data.UIMessages[folderIndex].Count > 0)
                {
                    DialogResult dr = MessageBox.Show("This folder contains messages, do you wish to continue?", "Warning", MessageBoxButtons.YesNo);

                    if(dr == DialogResult.No)
                    {
                        return;
                    }
                }

                var c = await Utility.establishConnectionImap();            //establish connection
                var folder = c.GetFolder(folderName);                       //get the folder from server from specifed name
                folder.Delete();                                            //delete the folder

                Data.existingFolders.Remove(folderName);                    //remove the folder from folderList
                Data.UIMessages.RemoveAt(folderIndex);                      //remove from UI-list
                Data.pendingMessages.RemoveAt(folderIndex);                 //remove from pending-list

            }

            catch(Exception ex)
            {
                MessageBox.Show("Unable to delete folder. You should only delete folders that you have created yourself.");
                Debug.WriteLine(ex.Message);
            }

            finally
            {
                RetrieveFolders();
                server.startListeners();
            }
        }
    }
}
