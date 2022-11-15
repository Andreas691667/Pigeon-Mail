using MailKit;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            Thread.Sleep(100);
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

        public static async void moveMsgTrashServer(Queue<Tuple<string, int>> q)
        {
            try
            {
                foreach (var item in q)
                {
                    var f = item.Item1;
                    var index = item.Item2;

                    var client = await Utility.establishConnectionImap();

                    var trashFolder = await Data.GetTrashFolder(client);

                    var folder = await client.GetFolderAsync(f);

                    await folder.OpenAsync(FolderAccess.ReadWrite);

                    await folder.MoveToAsync(index, trashFolder);

                    Utility.logMessage("Message moved to trash");

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

        public static async void removeFlagSever()
        {

        }

        public static async void addFlagServer()
        {

        }

        public static async void saveDraftServer()
        {

        }
    }
}
