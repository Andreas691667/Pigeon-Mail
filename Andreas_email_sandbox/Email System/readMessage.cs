using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System.Diagnostics;
using System.IO;

namespace Email_System
{
    public partial class readMessage : Form
    {
        string bodyText = null!;

        Data.msg msg = new Data.msg();

        IMessageSummary message = null!;

        public readMessage(string body, string from, string to, string date, string subject, string attachments, string folder)
        {
            Utility.refreshCurrentFolder();
            InitializeComponent();

            msg.from = from;
            msg.subject = subject;
            msg.date = date;
            msg.to = to;
            msg.body = body;
            msg.attachments = attachments;
            msg.folder = folder;

            initializeMessage();
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

            //var attachment = message.Attachments.ElementAt(attachmentIndex);

            //retrieve attachment from folder 
            var f = client.GetFolder(msg.folder);            

            f.Open(FolderAccess.ReadWrite);
            
            var query = SearchQuery.SubjectContains(msg.subject);

            var uids = f.Search(query);            

            var items = f.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.BodyStructure);

            foreach (var item in items)
            {
                //foreach (var attachment in item.Attachments)
                //{
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
        

/*        private async void getTextBody()
        {
            try
            {
                var client = await Utility.establishConnectionImap();

                var bodyPart = message.TextBody;

                var folder = await client.GetFolderAsync(message.Folder.ToString());
                await folder.OpenAsync(FolderAccess.ReadOnly);

                var body = (TextPart)folder.GetBodyPart(message.UniqueId, bodyPart);

                var text = body.Text;
                bodyText = text;

                bodyRtb.Text = text;

                await client.DisconnectAsync(true);
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }*/

        private void initializeMessage()
        {
            /*            fromTb.Text += message.Envelope.From.ToString();

                        toTb.Text += message.Envelope.To.ToString();

                        dateTb.Text +=  message.Envelope.Date.ToString();

                        //NEED TO CHECK IF THIS IS NULL
                        subjectTb.Text = message.Envelope.Subject.ToString();*/

            //ccRecipientsTb.Text = message.Envelope.Cc.ToString();

            fromTb.Text += msg.from;
            toTb.Text += msg.to;
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
            new newEmail(1, null!, msg.body, msg.subject, msg.to,msg.from, msg.cc, msg.attachments, msg.folder).Show();
        }

        private void forwardBt_Click(object sender, EventArgs e)
        {
            new newEmail(3, null!, msg.body, msg.subject, msg.to, msg.from, msg.cc, msg.attachments, msg.folder).Show();
        }

        private void replyAllBt_Click(object sender, EventArgs e)
        {
            new newEmail(2, null!, msg.body, msg.subject, msg.to, msg.from, msg.cc, msg.attachments, msg.folder).Show();
        }

        private void deleteMessageBt_Click(object sender, EventArgs e)
        {
         //   Utility.deleteMsg(msg.uid, msg.subject);



            this.Close();
        }

        private void moveToTrashBT_Click(object sender, EventArgs e)
        {
            Utility.moveMessageToTrash(message);
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

            catch(Exception ex)
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
