using MailKit.Net.Smtp;
using MailKit;
using MimeKit; //allow us to use mime messages
using System.Windows.Forms;
using Org.BouncyCastle.Asn1.X509;
using MailKit.Net.Imap;
using System.Net.Mail;
using System.Diagnostics;

namespace Email_System
{
    public partial class newEmail : Form
    {
        IMessageSummary messageSender = null!;
        MimeMessage message = new MimeMessage();
        BodyBuilder builder = new BodyBuilder();


        bool isDraft = false;
        bool exitFromBt = false;

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
                messageSender = m;

                string recipient = (messageSender.Envelope.From.ToString()).Substring(messageSender.Envelope.From.ToString().LastIndexOf("<"));  
                recipientsTb.Text = recipient;
                subjectTb.Text = "Re: " + messageSender.Envelope.Subject;
            }

            else if(typeKey == 2 && m != null)
            {
                messageSender = m;

                subjectTb.Text = "Re: " + messageSender.Envelope.Subject;

                string recipient = (messageSender.Envelope.From.ToString()).Substring(messageSender.Envelope.From.ToString().LastIndexOf("<"));
                recipientsTb.Text = recipient;

                string[] ccRecipients = messageSender.Envelope.Cc.ToString().Split(",");
                
                foreach (var rec in ccRecipients)
                {
                    ccRecipientsTb.AppendText(rec + ", ");
                }

                ccRecipientsTb.Text = ccRecipientsTb.Text.Remove(ccRecipientsTb.Text.Length - 2);
            }

            else if(typeKey == 3 && m != null)
            {
                messageSender = m;

                subjectTb.Text = "Fwrd: " + messageSender.Envelope.Subject;

                messageBodyTb.AppendText(Environment.NewLine);
                messageBodyTb.AppendText("-------- Forwarded message --------");
                messageBodyTb.AppendText(Environment.NewLine);
                messageBodyTb.AppendText(messageSender.Envelope.From.ToString());
                messageBodyTb.AppendText(Environment.NewLine);
                messageBodyTb.AppendText(messageSender.Date.ToString());
                messageBodyTb.AppendText(Environment.NewLine);
                messageBodyTb.AppendText(messageSender.Envelope.To.ToString());
                messageBodyTb.AppendText(Environment.NewLine);
                messageBodyTb.AppendText(body);
            }

            else if(typeKey == 4 && m!= null)
            {
                isDraft = true;

                messageSender = m;
                subjectTb.Text = messageSender.Envelope.Subject;
                messageBodyTb.AppendText(body);
                recipientsTb.Text = messageSender.Envelope.To.ToString();            
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
                    builder.TextBody = @"";
                }
            }
            else
            {
                builder.TextBody = messageBodyTb.Text;
            }
        }

        private void addAttachment(MimeMessage message)
        {
            string fileName = chooseAttachment();

            if (!string.IsNullOrEmpty(fileName))
                builder.Attachments.Add(@fileName);
        }

        private string chooseAttachment()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = "C:\\";
            //openFileDialog.Filter = "Database files (*.mdb, *.accdb)|*.mdb;*.accdb";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                attachmentsLabel.Visible = true;
                attachmentsLb.Visible = true;
                string fileName = openFileDialog.FileName;
                attachmentsLb.Items.Add(fileName.Substring(fileName.LastIndexOf('\\') + 1) + " ");
                return fileName;
            }

            else
                return "";
        }

        #endregion

        private void sendBt_Click(object sender, EventArgs e)
        {

            message.From.Add(new MailboxAddress(Utility.username, Utility.username));            

            addRecipient(message);

            addCcRecipients(message);

            addSubject(message);

            addBody(message);

            message.Body = builder.ToMessageBody();

            var client = Utility.establishConnectionSmtp();

            try
            {
                client.Send(message);

                MessageBox.Show("Message sent successfully!");
                //messageSent = true;

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
            exitFromBt = true;

            if(isDraft)
            {
                //implement here functionality such that the message is overwritten.
            }

            if (!allEmpty())
            {
                DialogResult result = MessageBox.Show("Do you wish to save the mail in 'Drafts'?", "Save as draft?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                    saveAsDraft();

                else if(result == DialogResult.No)
                {
                    this.Close();
                }
            }

            else
                this.Close();
        }

        private async Task<IMailFolder> getDraftFolder()
        {
            var client = await Utility.establishConnectionImap();
            var folder = client.GetFolder(SpecialFolder.Drafts);

            return folder;
        }

        private void buildDraftMessage(MimeMessage mg)
        {
            mg.Subject = subjectTb.Text;

            mg.Body = new TextPart("plain")
            {
                Text = messageBodyTb.Text
            };

            if (!string.IsNullOrEmpty(recipientsTb.Text))
            {
                string[] recipients = recipientsTb.Text.Split(",");

                foreach (var rec in recipients)
                {
                    mg.To.Add(MailboxAddress.Parse(rec));
                }
            }
        }

        private async void saveAsDraft()
        {
            try
            {
                MimeMessage mg = new MimeMessage();
                buildDraftMessage(mg);

                ImapClient client = await Utility.establishConnectionImap();

                IMailFolder draftsFolder = await getDraftFolder();

                await draftsFolder.OpenAsync(FolderAccess.ReadWrite);

                draftsFolder.Append(mg, MessageFlags.Draft);

                this.Close();
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void newEmail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!allEmpty() && !exitFromBt)
            {
                DialogResult result = MessageBox.Show("Do you wish to save the mail in 'Drafts'?", "Save as draft?", MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    saveAsDraft();
                }
            }
        }

        private bool allEmpty()
        {
            if (string.IsNullOrEmpty(this.subjectTb.Text) && string.IsNullOrEmpty(this.recipientsTb.Text) && string.IsNullOrEmpty(this.ccRecipientsTb.Text) && string.IsNullOrEmpty(this.messageBodyTb.Text) && attachmentsLb.Items.Count == 0)
            {
                return true;
            }

            else
                return false;
        }

        private void draftBt_Click(object sender, EventArgs e)
        {
            exitFromBt = true;
            saveAsDraft();
        }

        private void addAttachmentBt_Click(object sender, EventArgs e)
        {
            addAttachment(message);
        }
    }
}
