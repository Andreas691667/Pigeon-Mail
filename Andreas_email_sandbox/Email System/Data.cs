using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;

namespace Email_System
{
    internal class Data
    {

        public static IList<string> folderList = new List<string>();

        public static IList<string> messageList = new List<string>();

        public static bool exit = false;

        public static async void loadFolders(BackgroundWorker bw)
        {
            while (!bw.CancellationPending)
            {

                var client = await Utility.establishConnectionImap();

                // get the folders from the server (to a List)
                var folders = await client.GetFoldersAsync(new FolderNamespace(';', ""));

                // get the messages from the folder and add them to the dictionary
                foreach (var item in folders)
                {
                    if (item.Exists)
                    {                        
                        folderList.Add(item.FullName);
                    }
                }

                saveFolders();

                //disconnect from the client
                await client.DisconnectAsync(true);

                if (exit)
                    break;
            }

            //string folderName = item.FullName.Substring(item.FullName.LastIndexOf('/') + 1);
        }


        public static async void loadMessages(BackgroundWorker bw)
        {
            while (!bw.CancellationPending)
            {
                var client = await Utility.establishConnectionImap();
                File.Open("folders.txt", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);


                foreach (string folderName in File.ReadLines("folders.txt"))
                {
                    var folder = await client.GetFolderAsync(folderName);

                    await folder.OpenAsync(FolderAccess.ReadOnly);

                    var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                }

                saveMessages();

                if (exit)
                    break;
            }
        }



        private static void saveFolders()
        {
            File.WriteAllLines("folders.txt", folderList);
            
            exit = true;
        }

        private static void saveMessages()
        {
            File.WriteAllLines("messages.txt", messageList);
            exit = true;
        }
    }
}
