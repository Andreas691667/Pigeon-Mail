using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MailKit;

namespace Email_System
{
    internal class Data
    {

        public struct msg
        {
            public string Id { get; set; }
            public string subject { get; set; }
            public string body { get; set; }
        }

        public static List<string> folderList = new List<string>();

        public static List<msg> messageList = new List<msg>();

        public static List<msg> messageList2 = new List<msg>();

        public static bool exit = false;


        public static msg getMessageByID(UniqueId id)
        {
            int index = messageList2.FindIndex(msg => msg.Id == id.Id.ToString());
            return messageList2[index];
        }

        public static void loadExistingMessages()
        {
            string filename = "messagesJson.json";
            string jsonString = File.ReadAllText(filename);
            var data = JsonSerializer.Deserialize<List<msg>>(jsonString)!;

            messageList2 = data;
        }

        public static async void loadFolders(BackgroundWorker bw)
        {

            //File.Open("folders.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

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

                foreach (string folderName in folderList)
                {
                    var folder = await client.GetFolderAsync(folderName);

                    await folder.OpenAsync(FolderAccess.ReadOnly);

                    var messages = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.Flags);

                    foreach (var message in messages)
                    {
                        msg mg = new msg();

                        mg.subject = message.Envelope.Subject;
                        mg.Id = message.UniqueId.Id.ToString();
                        //mg.body = message.Body.ToString();

                        messageList.Add(mg);
                    }
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
            var json = JsonSerializer.Serialize(messageList);
            File.WriteAllText("messagesJson.json", json);
        }
    }
}
