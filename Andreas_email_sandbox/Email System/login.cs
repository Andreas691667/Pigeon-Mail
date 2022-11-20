using MailKit.Net.Smtp;
using MailKit.Security;
using System.ComponentModel;
using System.Diagnostics;
using EmailValidation;

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

            // SHOULD BE MOVED TO A DISTINCT PRIVATE METHOD
            // Auto fill username (email) and password field if details are stored locally
            if (Properties.Settings.Default.Password != null)
            {
                passwordTb.Text = Properties.Settings.Default.Password;
            }

            if (Properties.Settings.Default.Username != null)
            {
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
        private void loginBt_Click(object sender, EventArgs e)
        {
            // Store login details locally if chosen by the client
            if(rememberMeCB.Checked)
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

            // SHOULD BE DONE USING SETTER FUNCTIONS!
            // Set username and password in Utility class
            Utility.username = usernameTb.Text;
            Utility.password = passwordTb.Text;            


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
                folderListenerBW.RunWorkerAsync();
                //allFoldersbackgroundWorker.RunWorkerAsync();
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

            Task task = Data.loadMessages(bw);
            task.Wait();
        }        

        private void folderListenerBW_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();

            Debug.WriteLine("listener bw started");

            Thread.Sleep(10000);

            Data.listenAllFolders();
            //Data.listenInboxFolder();           
        }

        private void login_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Data.saveMessages(Data.existingMessages);
        }

        private void folderListenerBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

                Debug.WriteLine("runworker inbox listen finished (no problem)");
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