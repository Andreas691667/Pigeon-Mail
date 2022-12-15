using MailKit.Net.Smtp;
using MailKit.Security;
using System.ComponentModel;
using System.Diagnostics;
using EmailValidation;
using System.Net.NetworkInformation;

/*
 The login window is the window that is first seen by the client
 */


namespace Email_System
{
    public partial class login : Form
    {

        private static login instance = null!;

        // ------ CONSTRUCTOR -----
        private login()
        {
            InitializeComponent();
            autoFill();
        }

        private void autoFill()
        {
            // Auto fill username (email) and password field if details are stored locally
            if (Properties.Settings.Default.Password != null && Properties.Settings.Default.Username != null)
            {
                passwordTb.Text = Properties.Settings.Default.Password;
                usernameTb.Text = Properties.Settings.Default.Username;
            }
        }

        // ----- Get Instance -----
        //ensures singleton pattern is maintained (only one instance at all times)
        public static login GetInstance
        {
            get
            {
                if (instance == null || instance.IsDisposed)
                {
                    instance = new login();
                }
                return instance;
            }
        }

        // ----- BUTTON CLICK METHODS ----
        // ----- Login button clicked -----
        private async void loginBt_Click(object sender, EventArgs e)
        {
            //if we are not connected to internet and the user has chosen to enable offlineMode
            if (!Utility.connectedToInternet() && Properties.Settings.Default.offlineModeEnabled)
            {
                //implement stuff to enable offline mode
                //check if the messages are downloaded
                //should also check for internet connection
                if (File.Exists(Properties.Settings.Default.Username + "messages.json") && File.Exists(Properties.Settings.Default.Username + "folders.json"))
                {
                    Utility.setUsername(usernameTb.Text);
                    Utility.setPassword(passwordTb.Text);

                    Data.loadExistingFolders();
                    await Data.loadExistingMessages();
                    Mailbox m = Mailbox.GetInstance;
                    m.Show();
                    this.Hide();

                    internetBW.RunWorkerAsync();

                    return;
                }
            }

            if (rememberMeCB.Checked)
            {
                Properties.Settings.Default.Username = usernameTb.Text;
                Properties.Settings.Default.Password = passwordTb.Text;
                Properties.Settings.Default.Save();
            }

            if(!rememberMeCB.Checked)
            {
                Properties.Settings.Default.Username = "";
                Properties.Settings.Default.Password = "";
                Properties.Settings.Default.Save();
            }


            // Get username and password
            string username = usernameTb.Text;
            string password = passwordTb.Text;

            bool valid = EmailValidator.Validate(username);
            if(!valid)
            {
                loginBt.Enabled = false;
            }

            else
            {
                loginBt.Enabled = true;
            }

            
            // Set username and password in Utility class
            Utility.setUsername(usernameTb.Text);
            Utility.setPassword(passwordTb.Text);  
            

            Data.loadOrCreateBlackListFile();


            // Get SMTP Client
            SmtpClient client = new SmtpClient();

            // Connect to the SMTP client corresponding to the provider
            try
            {
                // Should the comment below be removed?
                // string mail = username.Substring(username.LastIndexOf("@") + 1);

                // Gets and stores provider details
                // Details are used in the switch below
                setProviderSettings(username);

                // Connect to client
                switch (Properties.Settings.Default.SmtpServer)
                {
                    case "smtp.gmail.com":
                        client.Connect(Properties.Settings.Default.SmtpServer, Properties.Settings.Default.SmtpPort, true);
                        break;

                    case "smtp.office365.com":
                        client.Connect(Properties.Settings.Default.SmtpServer, Properties.Settings.Default.SmtpPort, SecureSocketOptions.StartTls);
                        break;

                    default:
                        return;
                } 
                Debug.WriteLine("connecting");
                
                // Authentication! 
                client.Authenticate(username, password);
                Debug.WriteLine("authenticating"); // Write to debug terminal

                // Indicate action
                this.Cursor = Cursors.WaitCursor;

                // Start retrieving folders
                if (!foldersBackgroundWorker.IsBusy)
                    foldersBackgroundWorker.RunWorkerAsync();

            }

            catch (Exception ex)
            {
                if(!Utility.connectedToInternet())
                {
                    MessageBox.Show("You are offline and have not enabled offline mode. You can do so in Settings.");
                }

                else
                    MessageBox.Show("Username and password combination not known");

            }

            finally
            {
                client.Disconnect(true);
                client.Dispose();
                this.Cursor = Cursors.Default;
            }
        }
        // ----- exit button clicked -----
        private void exitBt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // ------ HELPER FUNCTIONS ------
        // ------ Set Provider Settings -----
        // Set provider settings coresponding to the provider given by the client
        private void setProviderSettings(string username)
        {
            string Imapserver = "";
            int Imapport = -1;
            string SmtpServer = "";
            int SmtpPort = -1;
            string mail = username.Substring(username.LastIndexOf("@") + 1);

            switch (mail)
            {
                case "gmail.com":
                    Imapserver= "imap.gmail.com";
                    Imapport = 993;
                    SmtpServer = "smtp.gmail.com";
                    SmtpPort = 465;
                    break;

                case "hotmail.com":
                    Imapserver = "imap-mail.outlook.com";
                    Imapport = 993;
                    SmtpServer = "smtp.office365.com";
                    SmtpPort = 587;
                    break;

                default:
                    MessageBox.Show("E-mail provider not supported.");
                    break;
            }

            Properties.Settings.Default.SmtpServer = SmtpServer;
            Properties.Settings.Default.SmtpPort = SmtpPort;
            Properties.Settings.Default.ImapServer = Imapserver;
            Properties.Settings.Default.ImapPort = Imapport;
            Properties.Settings.Default.Save();
        }


        #region BackgroundWork

        private void messagesBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageBox.Show("Operation was canceled");
            }
            else if (e.Error != null)
            {
                // There was an error during the operation.
                string msg = String.Format("An error occurred: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }
            else
            {
                //only start listening if the application should not close
                var m = Mailbox.GetInstance;
                FormCollection fc = Application.OpenForms;

                foreach (Form f in fc)
                {
                    if (m.Name == f.Name)
                    {
                        folderListenerBW.RunWorkerAsync();
                        return;
                    }
                }

                Environment.Exit(0);
            }
        }

        private void foldersBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;
            Task task = Data.loadFolders(bw);
            task.Wait();
        }

        private void foldersBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                // The user canceled the operation.
                MessageBox.Show("Operation was canceled");
            }
            else if (e.Error != null)
            {
                // There was an error during the operation.
                string msg = String.Format("An error occurred: {0}", e.Error.Message);
                MessageBox.Show(msg);
            }
            else            
            {
                Mailbox m = Mailbox.GetInstance;
                m.Show();
                this.Hide();

                Debug.WriteLine("runworker folder finished");
                messagesBackgroundWorker.RunWorkerAsync();
            }
        }
        private void messagesBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Debug.WriteLine("start message worker");

            BackgroundWorker bw = sender as BackgroundWorker;

            Data.GetTrashFolder();
            Data.GetFlaggedFolder();
            Data.GetDraftFolder();
            Data.GetAllFolder();
            Task getSpamFolderTask = Data.GetSpamFolder();
            getSpamFolderTask.Wait();
            Task task = Data.loadMessages(bw);
            task.Wait();
        }        

        private void folderListenerBW_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();

            Debug.WriteLine("listener bw started");

            Thread.Sleep(5000);

            Data.listenAllFolders();
        }

        private void folderListenerBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Cancelled)
            {
                Debug.WriteLine("Operation was canceled");
            }
            else if (!e.Cancelled && e.Error != null)
            {
                // There was an error during the operation.
                //string msg = String.Format("An error occurred: {0}", e.Error.Message);
                //Debug.WriteLine(msg);
                folderListenerBW.RunWorkerAsync();
            }
            else
            {
                Debug.WriteLine("runworker inbox listen finished (no problem)");
            }

        }

        private void internetBW_DoWork(object sender, DoWorkEventArgs e)
        {
            while(!internetBW.CancellationPending)
            {
                try
                {
                    Ping myPing = new Ping();
                    String host = "google.com";
                    byte[] buffer = new byte[32];
                    int timeout = 1000;
                    PingOptions pingOptions = new PingOptions();
                    PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);

                    if(reply.Status == IPStatus.Success)
                    {
                        DialogResult d = MessageBox.Show("You have reconnected! Press OK to restart the app.","Connection restored",  MessageBoxButtons.OK);

                        //apparently this only works the second time??
                        if(d == DialogResult.OK)
                        {
                            internetBW.CancelAsync();
                            BeginInvoke(new Action(() => Utility.restartApplication()));
                        }                        
                    }
                }
                catch (Exception)
                {
                    Thread.Sleep(10000);
                }
            }
        }

        #endregion
        private void usernameTb_Validating(object sender, CancelEventArgs e)
        {
            // Get username and password
            string username = usernameTb.Text;

            bool valid = EmailValidator.Validate(username);

            if (!valid)
            {
                loginBt.Enabled = false;
            }

            else
            {
                loginBt.Enabled = true;
            }
        }
    }        
}