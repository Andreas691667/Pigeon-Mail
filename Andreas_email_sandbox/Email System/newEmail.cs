using EmailValidation;
using MailKit;
using MimeKit; //allow us to use mime messages
using System.Diagnostics;

namespace Email_System
{
    public partial class newEmail : Form
    {
        MimeMessage message = new MimeMessage();
        BodyBuilder builder = new BodyBuilder();

        //Dictionary<string, string> attachments = new Dictionary<string, string>();
        Data.msg msg = new Data.msg();




        bool stopSend = false;
        bool isDraft = false;
        bool exitFromBt = false;
        bool messageSent = false;

        Dictionary<string, double> attachments = new Dictionary<string, double>();
        double collectedFileSize = 0;

        //type keys:
        // 0: blank email
        // 1: reply
        // 2: reply all
        // 3: forward
        // 4: drafts
        public newEmail(int flag, string body = null!, string subject = null!, string rec = null!, string from = null!, string ccRec = null!, string attachments = null!, string folder = null!, uint uid = 0, string flags = null!, string sender = null!, string date = null!)
        {
            InitializeComponent();

            msg.from = from;
            msg.subject = subject;
            msg.to = rec;
            msg.body = body;
            msg.attachments = attachments;
            msg.folder = folder;
            msg.cc = ccRec;
            msg.uid = uid;
            msg.sender = sender;
            msg.flags = flags;
            msg.date = date;

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

            if (!Utility.connectedToInternet())
                sendBt.Enabled = false;

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


            messageBodyTb.AppendText(Environment.NewLine);
            messageBodyTb.AppendText("---------------------------------");
            messageBodyTb.AppendText(Environment.NewLine);
            messageBodyTb.AppendText("At " + msg.date + " " + msg.from + " wrote:");
            messageBodyTb.AppendText(Environment.NewLine);

            if (!string.IsNullOrEmpty(msg.body))
                messageBodyTb.AppendText(msg.body);
        }

        private void flagReplyAll()
        {
            subjectTb.Text = "Re: " + msg.subject;

            string recipient = (msg.from).Substring(msg.from.LastIndexOf("<"));
            recipient = recipient.Replace('<', ' ');
            recipient = recipient.Replace('>', ' ');
            recipient.Trim();

            recipientsTb.Text = recipient;

            try
            {
                if (!string.IsNullOrEmpty(msg.cc))
                {
                    string[] ccRecipients = msg.cc.Split(",");

                    foreach (var rec in ccRecipients)
                    {
                        ccRecipientsTb.AppendText(rec + ", ");
                    }

                    ccRecipientsTb.Text = ccRecipientsTb.Text.Remove(ccRecipientsTb.Text.Length - 2);
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            messageBodyTb.AppendText(Environment.NewLine);
            messageBodyTb.AppendText("---------------------------------");
            messageBodyTb.AppendText(Environment.NewLine);
            messageBodyTb.AppendText("At " + msg.date + " " + msg.from + " wrote:");
            messageBodyTb.AppendText(Environment.NewLine);

            if (!string.IsNullOrEmpty(msg.body))
                messageBodyTb.AppendText(msg.body);
        }

        private async void flagForward()
        {
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

            //get attachments from message
            if (msg.attachments != null && Utility.connectedToInternet())
            {
                string[] attachments = msg.attachments.Split(';');

                var client = await Utility.establishConnectionImap();
                var f = client.GetFolder(msg.folder);
                f.Open(FolderAccess.ReadWrite);

                var id = new UniqueId[] { new UniqueId(msg.uid) };

                var items = f.Fetch(id, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure);

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

                        MimeEntity entity = f.GetBodyPart(item.UniqueId, attachment);

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

        private async void flagDraft()
        {
            isDraft = true;
            draftBt.Enabled = false;

            subjectTb.Text = msg.subject;
            recipientsTb.Text = msg.to;
            ccRecipientsTb.Text = msg.cc;
            messageBodyTb.AppendText(msg.body);

            //get attachments from message
            if (msg.attachments != null && Utility.connectedToInternet())
            {
                string[] attachments = msg.attachments.Split(';');

                var client = await Utility.establishConnectionImap();
                var f = client.GetFolder(msg.folder);
                f.Open(FolderAccess.ReadWrite);

                var id = new UniqueId[] { new UniqueId(msg.uid) };

                var items = f.Fetch(id, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure);

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

                        MimeEntity entity = f.GetBodyPart(item.UniqueId, attachment);

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


        #endregion

        #region Build message

        private void addRecipient(MimeMessage message)
        {
            if (string.IsNullOrEmpty(recipientsTb.Text))
            {
                MessageBox.Show("No recipient!");
                stopSend = true;
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
                    stopSend = true;
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
                    stopSend = true;
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

                var size = (double)new FileInfo(openFileDialog.FileName).Length;
                size *= 0.000001;

                collectedFileSize += size;
                collectedFileSize = Math.Round(collectedFileSize, 2);

                sizeLabel.Visible = true;
                sizeLabel.Text = "(" + collectedFileSize.ToString() + "MB" + " )";

                string fileName = openFileDialog.FileName;
                string fileNameShort = fileName.Substring(fileName.LastIndexOf('\\') + 1) + " ";


                if (attachments.ContainsKey(fileNameShort))
                {
                    MessageBox.Show("Attachment with such a name already exists. Please select another or rename the file.");
                    return "";
                }

                //add to dictionary
                attachments.Add(fileNameShort, size);
                attachmentsLb.Items.Add(fileNameShort);

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

            if (!stopSend)
            {

                var client = Utility.establishConnectionSmtp();

                server.killListeners();

                messageSent = true;
                this.Close();

                Utility.logMessage("Sending message", 3000);

                try
                {
                    client.Send(message);

                    //if the message was a draft, we should delete it from the draft folder!
                    if (isDraft)
                    {
                        Utility.deleteMsg(msg.uid, msg.folder);
                    }


                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                finally
                {

                    client.Disconnect(true);
                    client.Dispose();

                    if (!isDraft)
                        server.startListeners();

                    Utility.logMessage("Message sent successfully!", 3000);
                }
            }

            else
                return;
        }

        private void exitBt_Click(object sender, EventArgs e)
        {
            exitFromBt = true;
            this.Close();
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

        private void updateLocalDraftMessage()
        {
            msg.cc = ccRecipientsTb.Text;
            msg.from = Utility.username;
            msg.to = recipientsTb.Text;
            msg.subject = subjectTb.Text;
            //msg.attachments
            msg.body = messageBodyTb.Text;
            msg.date = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            msg.date += " temp";

            for (int i = 0; i < Data.UIMessages.Count; i++)
            {
                for (int j = 0; j < Data.UIMessages[i].Count; j++)
                {
                    if (msg.uid == Data.UIMessages[i][j].uid)
                        Data.UIMessages[i][j] = msg;
                }
            }
        }

        private async void saveAsDraft()
        {
            try
            {
                MimeMessage mg = new MimeMessage();
                buildDraftMessage(mg);

                var draftsFolder = await Data.GetDraftFolder();
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
            }
        }

        private void newEmail_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (messageSent || exitFromBt)
            {
                return;
            }

            else if (isDraft)
            {
                updateLocalDraftMessage();
                Utility.refreshCurrentFolder();
            }

            else if (!allEmpty())
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
            this.Close();
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
                string attachmentName = attachmentsLb.SelectedItem.ToString();

                attachmentsLb.Items.RemoveAt(attachmentIndex);
                //remove from message
                builder.Attachments.RemoveAt(attachmentIndex);


                double size = attachments[attachmentName];
                collectedFileSize -= size;

                collectedFileSize = Math.Round(collectedFileSize, 2);
                sizeLabel.Visible = true;
                sizeLabel.Text = "(" + collectedFileSize.ToString() + "MB" + " )";
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
                sizeLabel.Visible = false;
                collectedFileSize = 0;
                attachments.Clear();
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

            if (string.IsNullOrEmpty(recipientsTb.Text) || !string.IsNullOrEmpty(ccRecipientsTb.Text))
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
