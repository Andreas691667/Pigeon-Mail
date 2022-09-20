using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MimeKit;

namespace Email_System
{
    public partial class readMessage : Form
    {
        string username;
        string password;

        MimeMessage message;
        public readMessage(MimeMessage m, string user, string pass)
        {
            InitializeComponent();
            message = m;
            username = user;
            password = pass;

            fromTb.Text = message.From.ToString();

            subjectTb.Text = message.Subject.ToString();

            bodyRtb.Text = message.Body.ToString();

            
        }

        private void closeBt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void replyBt_Click(object sender, EventArgs e)
        {
            new newEmail(username, password, fromTb.Text, "Re: " + subjectTb.Text).Show();
        }
    }
}
