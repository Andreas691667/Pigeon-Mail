using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Windows.Forms;

namespace Email_System
{
    internal class Utility
    {
        public static string username = null!;
        public static string password = null!;

        public static async Task<ImapClient>  establishConnectionImap()
        {
            try
            {
                ImapClient client = new ImapClient();

                await client.ConnectAsync(Properties.Settings.Default.ImapServer, Properties.Settings.Default.ImapPort, true);
                await client.AuthenticateAsync(username, password);

                return client;
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null!;
            }
        }

        public static SmtpClient establishConnectionSmtp()
        {
            try
            {
                SmtpClient client = new SmtpClient();

                string server = Properties.Settings.Default.SmtpServer;

                switch (server)
                {
                    case "smtp.gmail.com":
                        client.Connect(Properties.Settings.Default.SmtpServer, Properties.Settings.Default.SmtpPort, true);
                        break;

                    case "smtp.office365.com":
                        client.Connect(Properties.Settings.Default.SmtpServer, Properties.Settings.Default.SmtpPort, SecureSocketOptions.StartTls);
                        break;
                }

                
                client.Authenticate(username, password);

                return client;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null!;
            }
        }

        public static void refreshCurrentFolder()
        {
            Mailbox.refreshCurrentFolder();
        }

        public static async void deleteMessage(IMessageSummary mg)
        {
            DialogResult result = MessageBox.Show("The message will be deleted permanently without being moved to trash. Do you wish to continue? The action cannot be undone.", "Continue?", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }

            else if (result == DialogResult.Yes)
            {
                var client = await Utility.establishConnectionImap();

                var folder = await client.GetFolderAsync(mg.Folder.ToString());

                await folder.OpenAsync(FolderAccess.ReadWrite);

                await folder.AddFlagsAsync(mg.UniqueId, MessageFlags.Deleted, true);
                await folder.ExpungeAsync();

                Utility.refreshCurrentFolder();

                await client.DisconnectAsync(true);

                MessageBox.Show("The message has been deleted successfully!");
            }
        }
        public static async void moveMessageToTrash(IMessageSummary mg)
        {
            DialogResult result = MessageBox.Show("The message will be moved to trash. Do you wish to continue?", "Continue?", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                return;
            }

            else if (result == DialogResult.Yes)
            {
                var client = await Utility.establishConnectionImap();

                var folder = await client.GetFolderAsync(mg.Folder.ToString());
                var trashFolder = client.GetFolder(SpecialFolder.Trash);

                await trashFolder.OpenAsync(FolderAccess.ReadWrite);
                await folder.OpenAsync(FolderAccess.ReadWrite);

                await folder.MoveToAsync(mg.UniqueId, trashFolder);

                Utility.refreshCurrentFolder();

                await client.DisconnectAsync(true);

                MessageBox.Show("The message has been moved to trash succesfully!");
            }
        }
    }
}
