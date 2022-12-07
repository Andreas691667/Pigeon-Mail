using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit.Encodings;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Windows.Forms;
using static Email_System.Data;

namespace Email_System
{
    internal class Utility
    {
        // variables to save the credentials of current client
        // these are NOT saved over different sessions
        public static string username = null!;
        public static string password = null!;

        // ----- Getter and setter functions to access username and password -----
        public string getUsername ()
        {
            return username;
        }

        public static string getPassword ()
        {
            return password;
        }

        public static void setUsername (string username_in)
        {
            username = username_in;
        }

        public static void setPassword (string password_in)
        {
            password = password_in;
        }

        // ------ Other stuff -----
        // establish an imap connection and return the client

        public static bool connectedToInternet()
        {
            try
            {
                Ping myPing = new Ping();
                String host = "google.com";
                byte[] buffer = new byte[32];
                int timeout = 1000;
                PingOptions pingOptions = new PingOptions();
                PingReply reply = myPing.Send(host, timeout, buffer, pingOptions);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
            
        }

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
            var m = Mailbox.GetInstance;
            m.BeginInvoke(new Action(() => Mailbox.refreshCurrentFolder()));
        }

        //finds all messages matching the uid and deletes them locally
        //adds the uid to a queue and deletes the messages on server afterwards.
        public static void moveMsgTrash(uint uid, string sub, string folder)
        {
            //server.killListeners();

            var trashFolderIndex = Data.existingFolders.IndexOf(Data.trashFolderName);
            var folderIndex = Data.existingFolders.IndexOf(folder);

            //check we are not already in the trash folder
            if (trashFolderIndex == folderIndex)
            {
                Utility.logMessage("Message is already in trash!");
                return;
            }

            Queue<Tuple<string, uint>> trashQueue = new Queue<Tuple<string, uint>>();

            for(int f = 0; f < Data.UIMessages.Count; f++)
            {
                for(int i = 0; i<Data.UIMessages[f].Count;i++)
                {
                    Data.msg m = Data.UIMessages[f][i];

                    if (f != trashFolderIndex)
                    {
                        if (m.uid == uid)
                        {
                            Data.UIMessages[f].RemoveAt(i);
                            Data.pendingMessages[f].RemoveAt(i);

                            //should only be moved to trash once
                            if (trashQueue.Count <= 0)
                            {
                                Data.UIMessages[trashFolderIndex].Add(m);
                                Data.pendingMessages[trashFolderIndex].Add(m);
                            }

                            //move to trash on server here
                            Tuple<string, uint> t = new Tuple<string, uint>(m.folder, m.uid);
                            trashQueue.Enqueue(t);

                            refreshCurrentFolder();
                        }
                    }
                }
            }


            Debug.WriteLine("Found " + trashQueue.Count + " messages to move to trash");
            server.moveMsgTrashServer(trashQueue);
        }



        //stops listening on folders and deletes message locally
        public static void deleteMsg(uint uid, string sub, string folder)
        {
            server.killListeners();

            var folderIndex = Data.existingFolders.IndexOf(folder);

            Queue<Tuple<string, uint>> deleteQueue = new Queue<Tuple<string, uint>>();

            for(int f = 0; f < Data.UIMessages.Count; f++)
            {
                for (int i = 0; i < Data.UIMessages[f].Count; i++)
                {
                    Data.msg m = Data.UIMessages[f][i];

                    if (m.uid == uid)
                    {
                        Debug.WriteLine(m.uid);
                        Debug.WriteLine(m.subject + m.folder);

                        Debug.WriteLine("Deleted: " + m.subject + "from: " + m.folder);

                        Data.UIMessages[f].RemoveAt(i);
                        Data.pendingMessages[f].RemoveAt(i);

                        Tuple<string, uint> t = new Tuple<string, uint>(m.folder, m.uid);

                        deleteQueue.Enqueue(t);

                        refreshCurrentFolder();
                    }
                }
            }

            server.deleteMsgServer(deleteQueue);
        }

        public static void logMessage(string message)
        {
            Mailbox.setText(message);

            var t = new System.Windows.Forms.Timer();

            t.Interval = 3000; // it will Tick in 3 seconds

            t.Tick += (s, e) =>
            {
                Mailbox.setText("");
                t.Stop();
            };

            t.Start();             
        }

        public static void restartApplication()
        {
            Application.Restart();
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
/*                    var client = await Utility.establishConnectionImap();
                    var folder = await client.GetFolderAsync(mg.Folder.ToString());

                    IMailFolder trashFolder = GetTrashFolder(client, CancellationToken.None);

                    await trashFolder.OpenAsync(FolderAccess.ReadWrite);
                    await folder.OpenAsync(FolderAccess.ReadWrite);

                    await folder.MoveToAsync(mg.UniqueId, trashFolder);

                    Utility.refreshCurrentFolder();

                    await client.DisconnectAsync(true);

                    MessageBox.Show("The message has been moved to trash succesfully!");*/
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
