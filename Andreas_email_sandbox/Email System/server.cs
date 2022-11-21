using MailKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Email_System
{
    internal class server
    {
        public static void killListeners()
        {
            var l = login.GetInstance;

            if (!l.folderListenerBW.CancellationPending || l.folderListenerBW.IsBusy)
            {
                l.folderListenerBW.CancelAsync();
                l.folderListenerBW.Dispose();
            }

            Thread.Sleep(400);
        }

        public static void startListeners()
        {
            var l = login.GetInstance;

            if(!l.folderListenerBW.IsBusy)
                l.folderListenerBW.RunWorkerAsync();
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

                    await folder.AddFlagsAsync(index, MessageFlags.Deleted, true);
                    await folder.ExpungeAsync();

                    Utility.logMessage("Message deleted");

                    Debug.WriteLine("msg deleted from server from folder: " + folder.FullName);

                    var task = client.DisconnectAsync(true);
                    task.Wait();
                }


                startListeners();

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static async void moveMsgTrashServer(Queue<Tuple<string, uint>> q)
        {
            var client = await Utility.establishConnectionImap();

            try
            {
                foreach (var item in q)
                {
                    var f = item.Item1;

                    var id = new UniqueId[] { new UniqueId(item.Item2) };
                    var trashFolder = await Data.GetTrashFolder(client);

                    var folder = await client.GetFolderAsync(f);

                    await folder.OpenAsync(FolderAccess.ReadWrite);

                    await folder.MoveToAsync(id, trashFolder);

                    Utility.logMessage("Message moved to trash");
                }

                startListeners();

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            finally
            {
                var task = client.DisconnectAsync(true);
            }
        }

        public static async void removeFlagServer(string folderIn, uint uid)
        {

            var client = await Utility.establishConnectionImap();         

            var folder = await client.GetFolderAsync(folderIn);

            var id = new UniqueId[] { new UniqueId(uid) };

            await folder.OpenAsync(FolderAccess.ReadWrite);
            await folder.RemoveFlagsAsync(id, MessageFlags.Flagged, true);

            await client.DisconnectAsync(true);

            Utility.logMessage("Message unflagged");

            Thread.Sleep(100);

            startListeners();
        }

        public static async void addFlagServer(string folderIn, int index, uint uid)
        {
            var client = await Utility.establishConnectionImap();

            //get source folder
            var folder = await client.GetFolderAsync(folderIn);
            await folder.OpenAsync(FolderAccess.ReadWrite);

            var id = new UniqueId[] { new UniqueId(uid) };

            //add flag and copy message
            await folder.AddFlagsAsync(id, MessageFlags.Flagged, true);

            await client.DisconnectAsync(true);

            Utility.logMessage("Message flagged on server");

            startListeners();
        }

        public static async void saveDraftServer()
        {
            //presumably not necessary
        }

        public static async void markMsgAsReadServer(string folderIn, uint uid)
        {
            var client = await Utility.establishConnectionImap();
            //add flag to message
            var folder = await client.GetFolderAsync(folderIn);
            await folder.OpenAsync(FolderAccess.ReadWrite);
            var id = new UniqueId[] { new UniqueId(uid) };
            await folder.AddFlagsAsync(id, MessageFlags.Seen, true);
            await client.DisconnectAsync(true);

            Utility.logMessage("Message read");

            startListeners();
        }
    }
}
