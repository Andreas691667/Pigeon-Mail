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
        }

        private void loginBt_Click(object sender, EventArgs e)
        {
            string username = usernameTb.Text;
            string password = passwordTb.Text;
            

            SmtpClient client = new SmtpClient();

            try
            {
                string mail = username.Substring(username.LastIndexOf("@") + 1);

                client.Connect("smtp." + mail, 465, true);

                client.Authenticate(username, password);
   
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