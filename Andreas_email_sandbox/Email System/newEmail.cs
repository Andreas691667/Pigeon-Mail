using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit; //allow us to use mime messages
using Org.BouncyCastle.Cms;

namespace Email_System
{
    public partial class newEmail : Form
    {
        string username;
        string password;
        public newEmail(string user, string pass, string recipient = "", string subject = "")
        {
            InitializeComponent();
            username = user;
            password = pass;

            recipientsTb.Text = recipient;
            subjectTb.Text = subject;
        }

        private void sendBt_Click(object sender, EventArgs e)
        {
            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress(username, username));            

            if(string.IsNullOrEmpty(recipientsTb.Text))
            {
                MessageBox.Show("No recipient!");
                return;
            }

            else
            {
                string[] recipients = recipientsTb.Text.Split(",");

                foreach (var rec in recipients)
                {
                    message.To.Add(MailboxAddress.Parse(rec));
                }                
            }

            if (!string.IsNullOrEmpty(ccRecipientsTb.Text))
            {
                string[] ccRecipients = ccRecipientsTb.Text.Split(",");

                foreach (var ccRec in ccRecipients)
                {
                    message.Cc.Add(MailboxAddress.Parse(ccRec));
                }

            }

            if(string.IsNullOrEmpty(subjectTb.Text))
            {
                DialogResult result = MessageBox.Show("No subject. Do you wish to send the e-mail anyway?", "Fault", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }

                else if(result == DialogResult.Yes)
                {
                    message.Subject = "<no subjext>";
                }
            }            
            else
            {
                message.Subject = subjectTb.Text;
            }                       

            if(string.IsNullOrEmpty(messageBodyTb.Text))
            {
                DialogResult result = MessageBox.Show("No message. Do you wish to send the e-mail anyway?", "Fault", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }

                else if(result == DialogResult.Yes)
                {
                    message.Body = new TextPart("plain") { };                    
                }
            }             
            else
            {
                message.Body = new TextPart("plain")
                {
                    Text = messageBodyTb.Text
                };
            }


            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(username, password);
                client.Send(message);

                MessageBox.Show("Message sent successfully!");

                recipientsTb.Clear();
                ccRecipientsTb.Clear();
                subjectTb.Clear();
                messageBodyTb.Clear();
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
