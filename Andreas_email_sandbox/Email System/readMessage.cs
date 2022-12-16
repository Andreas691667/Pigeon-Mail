using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;

namespace Email_System
{
    public partial class readMessage : Form
    {
        Data.msg msg = new Data.msg();

        public readMessage(string body, string from, string to, string cc, string date, string subject, string attachments, string folder, uint uid)
        {
            Utility.refreshCurrentFolder();
            InitializeComponent();

            msg.uid = uid;
            msg.from = from;
            msg.subject = subject;
            msg.date = date;
            msg.to = to;
            msg.body = body;
            msg.attachments = attachments;
            msg.folder = folder;
            msg.cc = cc;

            initializeMessage();

            if (!Utility.connectedToInternet())
            {
                replyAllBt.Enabled = false;
                replyBt.Enabled = false;
                forwardBt.Enabled = false;
                downloadAttachmentBt.Enabled = false;
                moveToTrashBT.Enabled = false;
                deleteMessageBt.Enabled = false;
            }

            if (msg.folder == Data.allFolderName)
            {
                moveToTrashBT.Enabled = false;
                deleteMessageBt.Enabled = false;
            }
        }

        //adds attachments from message to listbox
        private void getAttachments()
        {
            if (msg.attachments != null)
            {
                string[] attachments = msg.attachments.Split(';');

                foreach (var attachment in attachments)
                {
                    attachmentsPanel.Visible = true;
                    attachmentsLb.Items.Add(attachment);
                }
            }
        }


        //code from: http://www.mimekit.net/docs/html/P_MailKit_IMessageSummary_Attachments.htm
        //method downloads selected attachments and saves it on user's desktop
        private void downloadAttachment(ImapClient client)
        {
            //get attachment from listbox
            int attachmentIndex = attachmentsLb.SelectedIndex;

            //retrieve attachment from folder 
            var f = client.GetFolder(msg.folder);

            f.Open(FolderAccess.ReadWrite);

            var query = SearchQuery.SubjectContains(msg.subject);

            var uids = f.Search(query);

            var items = f.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure);

            foreach (var item in items)
            {

                if (item.UniqueId.Id == msg.uid)
                {
                    var attachment = item.Attachments.ElementAt(attachmentIndex);


                    MimeEntity entity = f.GetBodyPart(item.UniqueId, attachment);

                    //specify download path
                    string Desktopfolder = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

                    //download the attachment
                    // attachments can be either message/rfc822 parts or regular MIME parts
                    if (entity is MessagePart)
                    {
                        var rfc822 = (MessagePart)entity;
                        var path = Path.Combine(Desktopfolder, attachment.PartSpecifier + ".eml");
                        rfc822.Message.WriteTo(path);

                        //display the file in file explorer
                        string argument = "/select, \"" + path + "\"";
                        System.Diagnostics.Process.Start("explorer.exe", argument);
                    }
                    else
                    {
                        var part = (MimePart)entity;

                        // note: it's possible for this to be null, but most will specify a filename
                        //var fileName = part.FileName;
                        string fileName = attachment.FileName;

                        var path = Path.Combine(Desktopfolder, fileName);

                        // decode and save the content to a file
                        using (var stream = File.Create(path))
                            part.Content.DecodeTo(stream);

                        //display the file in file explorer
                        string argument = "/select, \"" + path + "\"";
                        System.Diagnostics.Process.Start("explorer.exe", argument);
                    }
                }
            }

        }


        private void initializeMessage()
        {
            fromTb.Text += msg.from;
            toTb.Text += msg.to;
            toTb.Text += ", " + msg.cc;
            dateTb.Text += msg.date;
            subjectTb.Text += msg.subject;
            bodyRtb.Text += msg.body;

            getAttachments();
        }
        private void closeBt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void replyBt_Click(object sender, EventArgs e)
        {
            new newEmail(1, msg.body, msg.subject, msg.to, msg.from, msg.cc, msg.attachments, msg.folder, msg.uid, msg.flags, msg.sender, msg.date).Show();
        }

        private void forwardBt_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(msg.attachments))
            {
                DialogResult d = MessageBox.Show("Do you wish to include attachments?", "Include attachments?", MessageBoxButtons.YesNo);

                if (d == DialogResult.Yes)
                    new newEmail(3, msg.body, msg.subject, msg.to, msg.from, msg.cc, msg.attachments, msg.folder, msg.uid, msg.flags, msg.sender, msg.date).Show();
            }

            else
                new newEmail(3, msg.body, msg.subject, msg.to, msg.from, msg.cc, null!, msg.folder, msg.uid, msg.flags, msg.sender, msg.date).Show();
        }

        private void replyAllBt_Click(object sender, EventArgs e)
        {
            new newEmail(2, msg.body, msg.subject, msg.to, msg.from, msg.cc, msg.attachments, msg.folder, msg.uid, msg.flags, msg.sender, msg.date).Show();
        }

        private void deleteMessageBt_Click(object sender, EventArgs e)
        {
            Utility.deleteMsg(msg.uid, msg.folder);
            this.Close();
        }

        private void moveToTrashBT_Click(object sender, EventArgs e)
        {
            Utility.moveMsgTrash(msg.uid, msg.subject, msg.folder);
            this.Close();
        }

        private async void downloadAttachmentBt_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                var client = await Utility.establishConnectionImap();
                downloadAttachment(client);
            }

            catch
            {
                MessageBox.Show("No attachment selected");
            }

            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }
}
