﻿using MailKit.Net.Smtp;
using MailKit;
using MimeKit; //allow us to use mime messages

namespace Email_System
{
    public partial class newEmail : Form
    {
        IMessageSummary message = null!;

        //type keys:
        // 0: blank email
        // 1: reply
        // 2: reply all
        // 3: forward
        // 4: drafts (not implemented)
        public newEmail(int typeKey, IMessageSummary m = null!, string body = null!)
        {
            InitializeComponent();

            if (typeKey == 1 && m != null)
            {
                message = m;

                string recipient = (message.Envelope.From.ToString()).Substring(message.Envelope.From.ToString().LastIndexOf("<"));  
                recipientsTb.Text = recipient;
                subjectTb.Text = "Re: " + message.Envelope.Subject;
            }

            else if(typeKey == 2 && m != null)
            {
                message = m;

                subjectTb.Text = "Re: " + message.Envelope.Subject;

                string recipient = (message.Envelope.From.ToString()).Substring(message.Envelope.From.ToString().LastIndexOf("<"));
                recipientsTb.Text = recipient;

                string[] ccRecipients = message.Envelope.Cc.ToString().Split(",");
                
                foreach (var rec in ccRecipients)
                {
                    ccRecipientsTb.AppendText(rec + ", ");
                }

                ccRecipientsTb.Text = ccRecipientsTb.Text.Remove(ccRecipientsTb.Text.Length - 2);
            }

            else if(typeKey == 3 && m != null)
            {
                message = m;

                subjectTb.Text = "Fwrd: " + message.Envelope.Subject;

                messageBodyTb.AppendText(Environment.NewLine);
                messageBodyTb.AppendText("-------- Forwarded message --------");
                messageBodyTb.AppendText(Environment.NewLine);
                messageBodyTb.AppendText(message.Envelope.From.ToString());
                messageBodyTb.AppendText(Environment.NewLine);
                messageBodyTb.AppendText(message.Date.ToString());
                messageBodyTb.AppendText(Environment.NewLine);
                messageBodyTb.AppendText(message.Envelope.To.ToString());
                messageBodyTb.AppendText(Environment.NewLine);
                messageBodyTb.AppendText(body);
            }
        }

        #region Build message

        private void addRecipient(MimeMessage message)
        {
            if (string.IsNullOrEmpty(recipientsTb.Text))
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
        }

        private void addCcRecipients(MimeMessage message)
        {
            if (!string.IsNullOrEmpty(ccRecipientsTb.Text))
            {
                string[] ccRecipients = ccRecipientsTb.Text.Split(",");

                foreach (var ccRec in ccRecipients)
                {
                    message.Cc.Add(MailboxAddress.Parse(ccRec));
                }
            }
        }

        private void addSubject(MimeMessage message)
        {
            if (string.IsNullOrEmpty(subjectTb.Text))
            {
                DialogResult result = MessageBox.Show("No subject. Do you wish to send the e-mail anyway?", "Fault", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }

                else if (result == DialogResult.Yes)
                {
                    message.Subject = "<no subject>";
                }
            }

            else
            {
                message.Subject = subjectTb.Text;
            }
        }

        private void addBody(MimeMessage message)
        {
            if (string.IsNullOrEmpty(messageBodyTb.Text))
            {
                DialogResult result = MessageBox.Show("No message. Do you wish to send the e-mail anyway?", "Fault", MessageBoxButtons.YesNo);
                if (result == DialogResult.No)
                {
                    return;
                }

                else if (result == DialogResult.Yes)
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
        }

        #endregion

        private void sendBt_Click(object sender, EventArgs e)
        {
            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress(Utility.username, Utility.username));            

            addRecipient(message);

            addCcRecipients(message);

            addSubject(message);

            addBody(message);  

            SmtpClient client = Utility.establishConnectionSmtp();

            try
            {
                client.Send(message);

                MessageBox.Show("Message sent successfully!");

                recipientsTb.Clear();
                ccRecipientsTb.Clear();
                subjectTb.Clear();
                messageBodyTb.Clear();
                this.Close();
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
