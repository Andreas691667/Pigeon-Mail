using MailKit.Net.Smtp;
using MailKit.Security;
using System.ComponentModel;
using System.Diagnostics;

namespace Email_System
{
    public partial class login : Form
    {

        private static login instance = null!;

        //constructor
        private login()
        {
            InitializeComponent();

            if (Properties.Settings.Default.Password != null)
            {
                passwordTb.Text = Properties.Settings.Default.Password;
            }

            if (Properties.Settings.Default.Username != null)
            {
                usernameTb.Text = Properties.Settings.Default.Username;
            }
        }

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

        private void loginBt_Click(object sender, EventArgs e)
        {
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

            string username = usernameTb.Text;
            string password = passwordTb.Text;

            Utility.username = usernameTb.Text;
            Utility.password = passwordTb.Text;            

            SmtpClient client = new SmtpClient();

            try
            {
                //string mail = username.Substring(username.LastIndexOf("@") + 1);
                setProviderSettings(username);

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
                client.Authenticate(username, password);
                Debug.WriteLine("authenticating");
                this.Cursor = Cursors.WaitCursor;



                //start retrieving folders
                foldersBackgroundWorker.RunWorkerAsync();

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            finally
            {
                client.Disconnect(true);
                client.Dispose();
                this.Cursor = Cursors.Default;
            }
        }

        private void exitBt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

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
                inboxBackgroundWorker.RunWorkerAsync();
                allFoldersbackgroundWorker.RunWorkerAsync();
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

            Task task = Data.loadMessages(bw);
            task.Wait();
        }

        

        private void inboxBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = new BackgroundWorker();

            Debug.WriteLine("inbox bw started");

            Task t = Data.listenInboxFolder();                

            
        }

        private void login_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Data.saveMessages(Data.existingMessages);
        }

        private void inboxBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

        private void allFoldersbackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Debug.WriteLine("folder bw started");

            Data.listenAllFolders();
        }

        private void allFoldersbackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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

                Debug.WriteLine("runworker all folders listen finished (no problem)");
            }
        }

        #endregion


    }
}