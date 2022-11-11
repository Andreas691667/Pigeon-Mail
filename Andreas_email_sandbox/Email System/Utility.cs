using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Encodings;
using Org.BouncyCastle.Asn1.X509;
using System.Collections.Generic;
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
        public static void deleteMsg(uint uid, string sub, string folder)
        {
            //we should search other folders in case the message is in several folders
            //perhaps just traverse the entire list 'existingMessages'? shouldn't be that slow

            var l = login.GetInstance;

            var folderIndex = Data.existingFolders.IndexOf(folder);

            if (!l.folderListenerBW.CancellationPending)
            {
                l.folderListenerBW.CancelAsync();
                l.folderListenerBW.Dispose();
            }
            Thread.Sleep(500);
            Queue<Tuple<string, int>> deleteQueue = new Queue<Tuple<string, int>>();

            foreach (var f in Data.existingMessages.ToList())
            {
                foreach (var m in f.ToList())
                {
                    if (m.uid == uid && m.subject == sub)
                    {
                        Debug.WriteLine(m.uid);

                        Debug.WriteLine(m.subject + m.folder);

                        int i = Data.existingMessages[folderIndex].IndexOf(m);
                        Data.existingMessages[folderIndex].Remove(m);

                        Tuple<string, int> t = new Tuple<string,int>(m.folder, i);

                        deleteQueue.Enqueue(t);

                        refreshCurrentFolder();
                    }
                }
            }

            deleteMsgServer(deleteQueue);
        }

        //deletes message from server and starts listening on folders again
        public static async void deleteMsgServer(Queue<Tuple<string, int>> q)
        {
            try
            {
                foreach (var item in q)
                {
                    var f = item.Item1;
                    var index = item.Item2;

                    var client = await Utility.establishConnectionImap();
                    var folder = await client.GetFolderAsync(f);
                    await folder.OpenAsync(FolderAccess.ReadWrite);

                    int oldCount = folder.Count;

                    await folder.AddFlagsAsync(index, MessageFlags.Deleted, true);
                    await folder.ExpungeAsync();

                    Debug.WriteLine("msg deleted from server from folder: " + folder.FullName);

                    var task = client.DisconnectAsync(true);
                    task.Wait();
                }


                var l = login.GetInstance;

                l.folderListenerBW.RunWorkerAsync();
                

            }

            catch
            {

            }

            finally
            {
                //begin listening on inbox again
/*                var l = login.GetInstance;

                if (!l.folderListenerBW.IsBusy)
                {
                    l.folderListenerBW.RunWorkerAsync();
                }*/
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
