using MailKit.Net.Smtp;
using MailKit;
using MimeKit; //allow us to use mime messages
using System.Windows.Forms;
using Org.BouncyCastle.Asn1.X509;
using MailKit.Net.Imap;
using System.Net.Mail;
using System.Diagnostics;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Windows.Forms.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using EmailValidation;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Cms;
using System.Linq.Expressions;
using MailKit.Search;

namespace Email_System
{
    public partial class newEmail : Form
    {
        IMessageSummary messageSender = null!;
        MimeMessage message = new MimeMessage();
        BodyBuilder builder = new BodyBuilder();

        //Dictionary<string, string> attachments = new Dictionary<string, string>();
        Data.msg msg = new Data.msg();

        static string[] DraftFolderNames = { "Drafts", "Kladder", "Draft" };


        bool isDraft = false;
        bool exitFromBt = false;
        bool messageSent = false;

        //type keys:
        // 0: blank email
        // 1: reply
        // 2: reply all
        // 3: forward
        // 4: drafts
        public newEmail(int flag, IMessageSummary m = null!, string body = null!, string subject = null!, string rec = null!, string from = null!, string ccRec = null!, string attachments = null!, string folder = null!)
        {
            InitializeComponent();

            msg.from = from;
            msg.subject = subject;
            msg.to = rec;
            msg.body = body;
            msg.attachments = attachments;
            msg.folder = folder;
            msg.cc = ccRec;

            switch (flag)
            {
                case 0:
                    break;
                case 1:
                    flagReply();
                    break;
                case 2:
                    flagReplyAll();
                    break;
                case 3:
                    flagForward();
                    break;
                case 4:
                    flagDraft();
                    break;
            }

        }

        #region switch methods

        private void flagReply()
        {
            string recipient = (msg.from).Substring(msg.from.LastIndexOf("<"));
            recipient = recipient.Replace('<', ' ');
            recipient = recipient.Replace('>', ' ');
            recipient = recipient.Trim();

            recipientsTb.Text = recipient;

            subjectTb.Text = "Re: " + msg.subject;
        }

        private void flagReplyAll()
        {
            //messageSender = m;

            subjectTb.Text = "Re: " + msg.subject;

            string recipient = (msg.from).Substring(msg.from.LastIndexOf("<"));
            recipient = recipient.Replace('<', ' ');
            recipient = recipient.Replace('>', ' ');
            recipient.Trim();

            try
            {
                if (msg.cc != "" || msg.cc != null)
                {
                    string[] ccRecipients = msg.cc.Split(",");

                    foreach (var rec in ccRecipients)
                    {
                        ccRecipientsTb.AppendText(rec + ", ");
                    }

                    ccRecipientsTb.Text = ccRecipientsTb.Text.Remove(ccRecipientsTb.Text.Length - 2);
                }
            }

            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        private void flagForward()
        {
            //messageSender = m;

            subjectTb.Text = "Fwrd: " + msg.subject;

            messageBodyTb.AppendText(Environment.NewLine);
            messageBodyTb.AppendText("-------- Forwarded message --------");
            messageBodyTb.AppendText(Environment.NewLine);
            messageBodyTb.AppendText(msg.from);
            messageBodyTb.AppendText(Environment.NewLine);
            messageBodyTb.AppendText(msg.date);
            messageBodyTb.AppendText(Environment.NewLine);
            messageBodyTb.AppendText(msg.to);
            messageBodyTb.AppendText(Environment.NewLine);
            messageBodyTb.AppendText(msg.body);
        }

        private async void flagDraft()
        {
            isDraft = true;

            subjectTb.Text = msg.subject;
            recipientsTb.Text = msg.to;
            ccRecipientsTb.Text = msg.cc;
            messageBodyTb.AppendText(msg.body);

            //get attachments from message
            if (msg.attachments != null)
            {
                string[] attachments = msg.attachments.Split(';');

                var client = await Utility.establishConnectionImap();
                var f = client.GetFolder(msg.folder);
                f.Open(FolderAccess.ReadWrite);
                var query = SearchQuery.SubjectContains(msg.subject);
                var uids = f.Search(query);
                var items = f.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure);



                //traverse over the attachments
                foreach (var item in items)
                {
                    attachmentsLabel.Visible = true;
                    attachmentsLb.Visible = true;
                    removeAttachmentBt.Visible = true;

                    foreach (var attachment in item.Attachments)
                    {
                        //add filename to listbox
                        attachmentsLb.Items.Add(attachment.FileName);

                        MimeEntity entity = f.GetBodyPart(messageSender.UniqueId, attachment);

                        var tempFolder = Path.GetTempPath();

                        if (entity is MessagePart)
                        {
                            var rfc822 = (MessagePart)entity;
                            var path = Path.Combine(tempFolder, attachment.PartSpecifier + ".eml");
                            rfc822.Message.WriteTo(path);

                            //display the file in file explorer
                            string argument = "/select, \"" + path + "\"";

                            addAttachment(path);
                        }

                        else
                        {
                            var part = (MimePart)entity;

                            // note: it's possible for this to be null, but most will specify a filename
                            var fileName = part.FileName;

                            var path = Path.Combine(tempFolder, fileName);

                            // decode and save the content to a file
                            using (var stream = File.Create(path))
                                part.Content.DecodeTo(stream);

                            //display the file in file explorer
                            string argument = "/select, \"" + path + "\"";

                            addAttachment(path);
                        }
                    }

                }
            }
        }

        private void downloadAttachments()
        {

        }

        #endregion

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

        private void addAttachment(string fileName = null!)
        {
            if (fileName == null)
                fileName = chooseAttachment();

            if (!string.IsNullOrEmpty(fileName))
            {
                attachmentsLb.Visible = true;
                builder.Attachments.Add(@fileName);
            }
        }

        private string chooseAttachment()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.InitialDirectory = "C:\\";
            //openFileDialog.Filter = "Image Files(*.BMP; *.JPG; *.GIF)| *.BMP; *.JPG; *.GIF | All files(*.*) | *.*";
            openFileDialog.FilterIndex = 0;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                attachmentsLabel.Visible = true;
                attachmentsLb.Visible = true;
                removeAttachmentBt.Visible = true;

                string fileName = openFileDialog.FileName;
                string fileNameShort = fileName.Substring(fileName.LastIndexOf('\\') + 1) + " ";

                attachmentsLb.Items.Add(fileNameShort);

                return fileName;
            }

            else
                return "";
        }

        #endregion

        private void sendBt_Click(object sender, EventArgs e)
        {

            var l = login.GetInstance;
            l.folderListenerBW.CancelAsync();


            this.Enabled = false;
            this.Cursor = Cursors.WaitCursor;

            message.From.Add(new MailboxAddress(Utility.username, Utility.username));

            addRecipient(message);

            addCcRecipients(message);

            addSubject(message);

            addBody(message);

            message.Body = builder.ToMessageBody();

            var client = Utility.establishConnectionSmtp();

            try
            {
                if (isDraft)
                {
                    Utility.deleteMessage(messageSender, true);
                }

                client.Send(message);

                Utility.logMessage("Message sent successfully!");

                messageSent = true;

                this.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            finally
            {
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                client.Disconnect(true);
                client.Dispose();


                l = login.GetInstance;
                l.folderListenerBW.RunWorkerAsync();
            }
        }

        private void exitBt_Click(object sender, EventArgs e)
        {
            exitFromBt = true;
            this.Close();
        }

        private IMailFolder getDraftFolder(ImapClient client, CancellationToken cancellationToken)
        {
            if ((client.Capabilities & (ImapCapabilities.SpecialUse | ImapCapabilities.XList)) != 0)
            {
                var trashFolder = client.GetFolder(SpecialFolder.Drafts);
                return trashFolder;
            }

            else
            {
                var personal = client.GetFolder(client.PersonalNamespaces[0]);

                foreach (var folder in personal.GetSubfolders(false, cancellationToken))
                {
                    foreach (var name in DraftFolderNames)
                    {
                        if (folder.Name == name)
                            return folder;
                    }
                }
            }

            return null;
        }

        private void buildDraftMessage(MimeMessage mg)
        {
            BodyBuilder b = new BodyBuilder();

            mg.Subject = subjectTb.Text;

            if (string.IsNullOrEmpty(messageBodyTb.Text))
            {

                b.TextBody = @"";

            }
            else
            {
                b.TextBody = messageBodyTb.Text;
            }

            if (!string.IsNullOrEmpty(recipientsTb.Text))
            {
                string[] recipients = recipientsTb.Text.Split(",");

                foreach (var rec in recipients)
                {
                    mg.To.Add(MailboxAddress.Parse(rec));
                }
            }

            if (!string.IsNullOrEmpty(ccRecipientsTb.Text))
            {
                string[] recipients = ccRecipientsTb.Text.Split(",");

                foreach (var rec in recipients)
                {
                    mg.Cc.Add(MailboxAddress.Parse(rec));
                }
            }

            if (attachmentsLb.Items.Count > 0)
            {
                foreach (var item in builder.Attachments)
                {
                    b.Attachments.Add(item);
                }
            }

            mg.Body = b.ToMessageBody();
        }

        private async void saveAsDraft()
        {
            try
            {
                this.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                if (isDraft)
                {
                    Utility.deleteMessage(messageSender, true);
                }

                MimeMessage mg = new MimeMessage();

                buildDraftMessage(mg);

                ImapClient client = await Utility.establishConnectionImap();
                IMailFolder draftsFolder = getDraftFolder(client, CancellationToken.None);

                await draftsFolder.OpenAsync(FolderAccess.ReadWrite);
                await draftsFolder.AppendAsync(mg, MessageFlags.Draft);

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            finally
            {
                this.Close();
                this.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void newEmail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (messageSent)
            {
                return;
            }

            else if(isDraft)
            {
                saveAsDraft();
            }

            else if (!allEmpty() || !exitFromBt)
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
            addAttachment();
        }

        private void removeAttachmentBt_Click(object sender, EventArgs e)
        {
            try
            {
                //remove item from the listbox
                var attachmentIndex = attachmentsLb.SelectedIndex;
                attachmentsLb.Items.RemoveAt(attachmentIndex);
                //remove from message
                builder.Attachments.RemoveAt(attachmentIndex);
            }

            catch
            {
                MessageBox.Show("No attachment selected!");
            }

            if (builder.Attachments.Count <= 0)
            {
                removeAttachmentBt.Visible = false;
                attachmentsLb.Visible = false;
                attachmentsLabel.Visible = false;
            }
        }

        private void recipientsTb_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string[] recipients = recipientsTb.Text.Split(",");
            bool valid = validateStringList(recipients);
            sendBt.Enabled = valid;

            if (!valid)
            {
                recipientsTb.ForeColor = Color.Red;

            }

            else
            {
                recipientsTb.ForeColor = Color.Blue;
            }
        }

        private bool validateStringList(string[] stringIn)
        {
            bool valid = false;

            foreach (string r in stringIn)
            {
                Debug.WriteLine(r);
                string rec = r.Replace(",", "");

                rec = rec.Trim();
                Debug.WriteLine(rec);
                valid = EmailValidator.Validate(rec);
            }

            return valid;
        }

        private void ccRecipientsTb_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string[] ccRecipients = ccRecipientsTb.Text.Split(",");
            bool valid = validateStringList(ccRecipients);

            if (string.IsNullOrEmpty(recipientsTb.Text)  || !string.IsNullOrEmpty(ccRecipientsTb.Text))
            {
                sendBt.Enabled = valid;
            }


            if (!valid)
            {
                ccRecipientsTb.ForeColor = Color.Red;
            }

            else
            {
                ccRecipientsTb.ForeColor = Color.Blue;
            }
        }

        private void recipientsTb_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();

            toolTip.InitialDelay = 100;
            string text = "Separate multiple recipients with ,";

            toolTip.SetToolTip(recipientsTb, text);
        }
    }
}
