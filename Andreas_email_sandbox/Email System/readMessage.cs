using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System.IO;

namespace Email_System
{
    public partial class readMessage : Form
    {
        string bodyText = null!;

        IMessageSummary message;

        public readMessage(IMessageSummary m)
        {
            Utility.refreshCurrentFolder();
            InitializeComponent();

            message = m;
            getTextBody();
            initializeMessage();
        }

        //adds attachments from message to listbox
        private void getAttachments()
        {
            foreach(var attachment in message.Attachments)
            {
                attachmentsPanel.Visible = true;
                attachmentsLb.Items.Add(attachment.FileName);
            }
        }


        //code from: http://www.mimekit.net/docs/html/P_MailKit_IMessageSummary_Attachments.htm
        //method downloads selected attachments and saves it on user's desktop

        private void downloadAttachment(ImapClient client)
        {
            //get attachment from listbox
            int attachmentIndex = attachmentsLb.SelectedIndex;
            var attachment = message.Attachments.ElementAt(attachmentIndex);

            //retrieve attachment from folder 
            var f = client.GetFolder(message.Folder.ToString());
            f.Open(FolderAccess.ReadWrite);

            MimeEntity entity = f.GetBodyPart(message.UniqueId, attachment);

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
                var fileName = part.FileName;

                var path = Path.Combine(Desktopfolder, fileName);

                // decode and save the content to a file
                using (var stream = File.Create(path))
                    part.Content.DecodeTo(stream);

                //display the file in file explorer
                string argument = "/select, \"" + path + "\"";
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
        }

        private async void getTextBody()
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
        }

        private void initializeMessage()
        {
            fromTb.Text += message.Envelope.From.ToString();

            toTb.Text += message.Envelope.To.ToString();

            dateTb.Text +=  message.Envelope.Date.ToString();

            //NEED TO CHECK IF THIS IS NULL
            subjectTb.Text = message.Envelope.Subject.ToString();

            //ccRecipientsTb.Text = message.Envelope.Cc.ToString();

            getAttachments(); 
        }
        private void closeBt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void replyBt_Click(object sender, EventArgs e)
        {
            string rec = fromTb.Text.Substring(fromTb.Text.IndexOf("<"));
            new newEmail(1, message).Show();
        }

        private void forwardBt_Click(object sender, EventArgs e)
        {
            new newEmail(3, message, bodyText).Show();
        }

        private void replyAllBt_Click(object sender, EventArgs e)
        {
            new newEmail(2, message).Show();
        }

        private void deleteMessageBt_Click(object sender, EventArgs e)
        {
            Utility.deleteMessage(message);
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
