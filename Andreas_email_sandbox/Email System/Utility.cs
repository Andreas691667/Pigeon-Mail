using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Encodings;
using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;
using static Email_System.Data;

namespace Email_System
{
    internal class Utility
    {
        //variables to save the credentials of current client
        //these are NOT saved over different sessions
        public static string username = null!;
        public static string password = null!;

        //maybe add even more names?
        static string[] TrashFolderNames = { "Deleted", "Trash", "Papirkurv"};

        //get the trashFolder and return the Imailfolder
        static IMailFolder GetTrashFolder(ImapClient client, CancellationToken cancellationToken)
        {
            if ((client.Capabilities & (ImapCapabilities.SpecialUse | ImapCapabilities.XList)) != 0)
            {
                var trashFolder = client.GetFolder(SpecialFolder.Trash);
                return trashFolder;
            }

            else
            {
                var personal = client.GetFolder(client.PersonalNamespaces[0]);

                foreach (var folder in personal.GetSubfolders(false, cancellationToken))
                {
                    foreach (var name in TrashFolderNames)
                    {
                        if (folder.Name == name)
                            return folder;
                    }
                }
            }

            return null;
        }

        //establish an imap connection and return the client
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

        //establoish an smtp connection and return the client
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


        //refreshes the current open folder in Mailbox
        public static void refreshCurrentFolder()
        {
            Mailbox.refreshCurrentFolder();
        }

        //stops listening on folders and deletes message locally
        public static void deleteMsg(uint uid, string sub)
        {
            var l = login.GetInstance;


            foreach (var list in Data.existingMessages)
            {
                var folderIndex = Data.existingMessages.IndexOf(list);

                if(folderIndex == 0)
                {
                    l.inboxBackgroundWorker.CancelAsync();
                }

                else
                {
                    l.allFoldersbackgroundWorker.CancelAsync();
                }


                var items1 = list.FindAll(x => x.uid == uid);
                var items = items1.FindAll(x => x.subject == sub);

                foreach (var msg in items)
                {
                    Debug.WriteLine(msg.uid);

                    Debug.WriteLine(msg.subject + msg.folder);

                    int i = list.IndexOf(msg);

                    deleteMsgServer(msg.folder, i, folderIndex);

                    list.Remove(msg);
                    refreshCurrentFolder();
                }
            }
        }
        //deletes message from server and starts listening on golders again
        public static async void deleteMsgServer(string f, int index, int folderInd)
        {
            try
            {
                var client = await Utility.establishConnectionImap();
                var folder = await client.GetFolderAsync(f);
                await folder.OpenAsync(FolderAccess.ReadWrite);
                await folder.AddFlagsAsync(index, MessageFlags.Deleted, true);
                await folder.ExpungeAsync();
                await client.DisconnectAsync(true);

                Debug.WriteLine("msg deleted from server");
            }

            catch
            {

            }

            finally
            {
                //begin listening on inbox again
                var l = login.GetInstance;

                if (folderInd == 0)
                {
                    l.inboxBackgroundWorker.RunWorkerAsync();
                }

                else
                {
                    l.allFoldersbackgroundWorker.RunWorkerAsync();
                }
            }
        }




        #region old methods

        //DON'T USE
        public static async void moveMessageToTrash(IMessageSummary mg)
        {
            DialogResult result = MessageBox.Show("The message will be moved to trash. Do you wish to continue?", "Continue?", MessageBoxButtons.YesNo);

            if (result == DialogResult.No)
            {
                return;
            }


            else if (result == DialogResult.Yes)
            {
                try
                {
                    var client = await Utility.establishConnectionImap();
                    var folder = await client.GetFolderAsync(mg.Folder.ToString());

                    IMailFolder trashFolder = GetTrashFolder(client, CancellationToken.None);

                    await trashFolder.OpenAsync(FolderAccess.ReadWrite);
                    await folder.OpenAsync(FolderAccess.ReadWrite);

                    await folder.MoveToAsync(mg.UniqueId, trashFolder);

                    Utility.refreshCurrentFolder();

                    await client.DisconnectAsync(true);

                    MessageBox.Show("The message has been moved to trash succesfully!");
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }




        }

        //DON'T USE
        public static async void deleteMessage(IMessageSummary mg, bool avoidChoice = false)
        {
            if (avoidChoice == false)
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

            else
            {
                var client = await Utility.establishConnectionImap();
                var folder = await client.GetFolderAsync(mg.Folder.ToString());

                await folder.OpenAsync(FolderAccess.ReadWrite);
                await folder.AddFlagsAsync(mg.UniqueId, MessageFlags.Deleted, true);

                await folder.ExpungeAsync();
                Utility.refreshCurrentFolder();

                await client.DisconnectAsync(true);
            }
        }

        #endregion
    }
}
