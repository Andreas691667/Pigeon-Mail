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
using Org.BouncyCastle.Cms;

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

            ccRecipientsTb.Text = message.Cc.ToString();
        }

        private void closeBt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void replyBt_Click(object sender, EventArgs e)
        {
            string rec = fromTb.Text.Substring(fromTb.Text.IndexOf("<"));
            new newEmail(username, password, 1, message).Show();
        }

        private void forwardBt_Click(object sender, EventArgs e)
        {
            new newEmail(username, password, 3, message).Show();
        }

        private void replyAllBt_Click(object sender, EventArgs e)
        {
            new newEmail(username, password, 2, message).Show();
        }
    }
}
