using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Cms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Email_System
{
    public partial class readMessage : Form
    {
        string username;
        string password;
        string bodyText = null!;

        IMessageSummary message;

        public readMessage(IMessageSummary m, string user, string pass)
        {
            InitializeComponent();
            message = m;
            username = user;
            password = pass;

            initializeMessage();
        }

        private void initializeMessage()
        {
            fromTb.Text = message.Envelope.From.ToString();

            subjectTb.Text = message.Envelope.Subject.ToString();

            ccRecipientsTb.Text = message.Envelope.Cc.ToString();

            using var client = new ImapClient();
            {
                client.Connect("imap.gmail.com", 993, true);
                client.Authenticate(username, password);

                var bodyPart = message.TextBody;

                client.Inbox.Open(FolderAccess.ReadOnly);

                var body = (TextPart)client.Inbox.GetBodyPart(message.UniqueId, bodyPart);

                var text = body.Text;
                bodyText = text;

                bodyRtb.Text = text;
            }
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
            new newEmail(username, password, 3, message, bodyText).Show();
        }

        private void replyAllBt_Click(object sender, EventArgs e)
        {
            new newEmail(username, password, 2, message).Show();
        }

        private void deleteMessageBt_Click(object sender, EventArgs e)
        {

            DialogResult result = MessageBox.Show("The message will be deleted permanently without being moved to trash. Do you wish to continue? The action cannot be undone.", "Continue?", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }

            else if (result == DialogResult.Yes)
            {
                bool messageDeleted = false;

                while (!messageDeleted)
                {
                    this.Cursor = Cursors.WaitCursor;
                    using var client = new ImapClient();
                    {
                        client.Connect("imap.gmail.com", 993, true);
                        client.Authenticate(username, password);

                        var trashFolder = client.GetFolder(SpecialFolder.Trash);

                        client.Inbox.Open(FolderAccess.ReadWrite);

                        client.Inbox.AddFlags(message.UniqueId, MessageFlags.Deleted, true);
                        client.Inbox.Expunge();
                    }

                    this.Cursor = Cursors.Default;
                    messageDeleted = true;
                    this.Close();
                }

                MessageBox.Show("The message has been deleted successfully!");
            }

        }

        private void moveToTrashBT_Click(object sender, EventArgs e)
        {
            
            DialogResult result = MessageBox.Show("The message will be moved to trash. Do you wish to continue?", "Continue?", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }

            else if (result == DialogResult.Yes)
            {
                bool messageMoved = false;

                while (!messageMoved)
                {
                    this.Cursor = Cursors.WaitCursor;
                    using var client = new ImapClient();
                    {
                        client.Connect("imap.gmail.com", 993, true);
                        client.Authenticate(username, password);


                        var trashFolder = client.GetFolder(SpecialFolder.Trash);
                        trashFolder.Open(FolderAccess.ReadWrite);
                        client.Inbox.Open(FolderAccess.ReadWrite);

                        client.Inbox.MoveTo(message.UniqueId, trashFolder);
                    }

                    messageMoved = true;
                    this.Cursor = Cursors.Default;
                    this.Close();
                }

                MessageBox.Show("The message has been moved to trash succesfully!");
            }

        }
    }
}
