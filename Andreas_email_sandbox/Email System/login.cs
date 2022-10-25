using MailKit.Net.Smtp;
using System.ComponentModel;


namespace Email_System
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();

            if(Properties.Settings.Default.Password != null)
            {
                passwordTb.Text = Properties.Settings.Default.Password; 
            }

            if (Properties.Settings.Default.Username != null)
            {
                usernameTb.Text = Properties.Settings.Default.Username;
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

            //start retrieving folders
            foldersBackgroundWorker.RunWorkerAsync();
            //messagesBackgroundWorker.RunWorkerAsync();

            SmtpClient client = new SmtpClient();

            try
            {
                string mail = username.Substring(username.LastIndexOf("@") + 1);

                client.Connect("smtp." + mail, 465, true);

                client.Authenticate(username, password);

                this.Cursor = Cursors.WaitCursor;
                new Mailbox(username, password).Show();
                this.Hide();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

            finally
            {
                client.Disconnect(true);
                client.Dispose();

            }
        }

        private void exitBt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region BackgroundWork

        private void foldersBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;

            Data.loadFolders(bw);
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
                // The operation completed normally.
                string msg = String.Format("Result = {0}", e.Result);
                MessageBox.Show(msg);
            }
        }

        #endregion

        private void messagesBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker bw = sender as BackgroundWorker;

            Data.loadMessages(bw);
        }
    }
}