using MailKit;
using MimeKit;

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

        private async void getTextBody()
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

        private void initializeMessage()
        {
            fromTb.Text = message.Envelope.From.ToString();

            //NEED TO CHECK IF THIS IS NULL
            subjectTb.Text = message.Envelope.Subject.ToString();

            ccRecipientsTb.Text = message.Envelope.Cc.ToString();
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
    }
}
