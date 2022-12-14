using MailKit;
using System.Diagnostics;

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

            Thread.Sleep(1000);
        }

        public static void startListeners()
        {
            var l = login.GetInstance;

            if (!l.folderListenerBW.IsBusy) { }
                l.folderListenerBW.RunWorkerAsync();

        }

        //deletes message from server and starts listening on folders again
        public static async void deleteMsgServer(Queue<Tuple<string, uint>> q)
        {
            var client = await Utility.establishConnectionImap();

            try
            {
                foreach (var item in q)
                {
                    var f = item.Item1;
                    var id = new UniqueId[] { new UniqueId(item.Item2) };

                    var folder = await client.GetFolderAsync(f);
                    await folder.OpenAsync(FolderAccess.ReadWrite);

                    await folder.AddFlagsAsync(id, MessageFlags.Deleted, true);
                    await folder.ExpungeAsync();

                    Utility.logMessage("Message deleted", 3000);
                    Debug.WriteLine("msg deleted from server from folder: " + folder.FullName);
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        public static async void moveMsgTrashServer(Queue<Tuple<string, uint>> q)
        {
            var client = await Utility.establishConnectionImap();

            uint idToRemove = 0;

            try
            {
                foreach (var item in q)
                {
                    var f = item.Item1;

                    idToRemove = item.Item2;

                    var id = new UniqueId[] { new UniqueId(item.Item2) };

                    var trashFolder = await Data.GetTrashFolder(client);

                    var folder = await client.GetFolderAsync(f);

                    await folder.OpenAsync(FolderAccess.ReadWrite);

                    await folder.MoveToAsync(id, trashFolder);

                    Utility.logMessage("Message moved to trash", 3000);
                }

                //startListeners();

            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            finally
            {
                await client.DisconnectAsync(true);
                //await Task.Delay(5000);
                Data.changedUids.Remove(idToRemove);
            }
        }

        public static async Task moveMsgServer(Queue<Tuple<string, uint>> q, string moveToFolder)
        {
            var client = await Utility.establishConnectionImap();
            uint idToRemove = 0;

            try
            {
                foreach (var item in q)
                {
                    var f = item.Item1;

                    idToRemove = item.Item2;

                    var id = new UniqueId[] { new UniqueId(item.Item2) };

                    var destFolder = await client.GetFolderAsync(moveToFolder);

                    var folder = await client.GetFolderAsync(f);

                    await folder.OpenAsync(FolderAccess.ReadWrite);

                    var task = await folder.MoveToAsync(id, destFolder);

                    var destId = task[id[0]].Id;

                    int folderIndex = Data.existingFolders.IndexOf(f);
                    int messageIndex = Data.pendingMessages[folderIndex].FindIndex(x => x.uid == idToRemove);

                    var m = Data.pendingMessages[folderIndex][messageIndex];
                    m.uid = destId;
                    Data.pendingMessages[folderIndex][messageIndex] = m;

                    Debug.WriteLine("Message moved to: " + destFolder.FullName);

                    //Utility.logMessage("Message moved!", 3000);
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            finally
            {
                client.Disconnect(true);
                Data.changedUids.Remove(idToRemove);
            }
        }

        public static async void emptyFolderServer(string folder)
        {
            var client = await Utility.establishConnectionImap();

            List<uint> idsToRemove = new List<uint>();

            var f = await client.GetFolderAsync(folder);
            await f.OpenAsync(FolderAccess.ReadWrite);

            var allMessages = f.Fetch(0, -1, MessageSummaryItems.UniqueId);

            foreach (var msg in allMessages)
            {
                idsToRemove.Add(msg.UniqueId.Id);
                await f.AddFlagsAsync(msg.UniqueId, MessageFlags.Deleted, true);
            }

            await f.ExpungeAsync();
            await client.DisconnectAsync(true);

            foreach (uint id in idsToRemove)
            {
                Data.changedUids.Remove(id);
            }
        }

        public static async void removeFlagServer(string folderIn, uint uid)
        {
            uint idToRemove = uid;

            var client = await Utility.establishConnectionImap();

            var folder = await client.GetFolderAsync(folderIn);

            var id = new UniqueId[] { new UniqueId(uid) };

            await folder.OpenAsync(FolderAccess.ReadWrite);
            await folder.RemoveFlagsAsync(id, MessageFlags.Flagged, true);

            await client.DisconnectAsync(true);

            Utility.logMessage("Message unflagged", 3000);

            //Thread.Sleep(1000);

            await Task.Delay(5000);
            Data.changedUids.Remove(idToRemove);

            //startListeners();
        }

        public static async void addFlagServer(string folderIn, int index, uint uid)
        {
            var client = await Utility.establishConnectionImap();

            //get source folder
            var folder = await client.GetFolderAsync(folderIn);
            await folder.OpenAsync(FolderAccess.ReadWrite);


            var uids = folder.Search(MailKit.Search.SearchQuery.All);

            int uidIndex = 0;

            for (int i = 0; i < uids.Count; i++)
            {
                if (uids[i].Id == uid)
                {
                    uidIndex = i;
                }
            }

            var id = new UniqueId[] { new UniqueId(uid) };

            //add flag and copy message
            await folder.AddFlagsAsync(uids[uidIndex], MessageFlags.Flagged, true);
            await client.DisconnectAsync(true);
            Utility.logMessage("Message flagged on server", 3000);
            Utility.refreshCurrentFolder();

            await Task.Delay(5000);
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

            Utility.logMessage("Message read", 3000);

            //startListeners();
        }

        public static async void markMsgAsUnreadServer(string folderIn, uint uid)
        {
            var client = await Utility.establishConnectionImap();
            //remove flag to message
            var folder = await client.GetFolderAsync(folderIn);
            await folder.OpenAsync(FolderAccess.ReadWrite);
            var id = new UniqueId[] { new UniqueId(uid) };
            await folder.RemoveFlagsAsync(id, MessageFlags.Seen, true);
            await client.DisconnectAsync(true);

            Utility.logMessage("Message unread", 3000);

            //startListeners();
        }
    }
}
