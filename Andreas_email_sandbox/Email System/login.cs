using System.Windows.Forms;
using MailKit.Net.Smtp;
using MailKit;

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
    }
}