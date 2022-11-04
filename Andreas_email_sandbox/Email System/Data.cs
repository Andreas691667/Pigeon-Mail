using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace Email_System
{
    internal class Data
    {

        public struct msg
        {
            public UniqueId Id { get; set; }
            public string folder{ get; set; }
        }

        //public for data,
        //checks for new stuff
        public static ImapClient client = new ImapClient();

        public static int msgCount = 0;


        public static List<string> folderList = new List<string>();

        public static List<msg> messageList = new List<msg>();

        public static List<msg> existingMessages = new List<msg>();

        public static List<MimeMessage> messages = new List<MimeMessage>();



        public static bool exit = false;


        public static List<UniqueId> uids = new List<UniqueId>();


        public static void newMessages()
        {
            var inboxFolder = client.Inbox;
            
            
            //check for new messages, i.e. messageCount changed event.
            //update messagelist
        }

        public static void loadExistingMessages()
        {
            string filename = "MimeMessages.json";

            string jsonString = File.ReadAllText(filename);
            var data = JsonSerializer.Deserialize<List<UniqueId>>(jsonString);

            Debug.WriteLine("existing messaes loaded");
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

            Debug.WriteLine("folders loaded");

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

                    var messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId);

                    foreach (var messageSummary in messageSummaries)
                    {
                        Debug.WriteLine(messageSummary.UniqueId.Id);

                        //it doesn't work for mimemessages
                        UniqueId uid = messageSummary.UniqueId;
                        MimeMessage msg = await folder.GetMessageAsync(uid);
                        messages.Add(msg);                        

                        msg.WriteTo(uid.Id.ToString());
                    }

                }

                //saveMessages();

                if (exit)
                    break;
            }

            Debug.WriteLine("messages loaded");
        }
        private static void saveFolders()
        {
            File.WriteAllLines("folders.txt", folderList);            
            exit = true;
        }

        private static void saveMessages()
        {
            var json = JsonSerializer.Serialize(messages);
            File.WriteAllText("MimeMessages.json", json);
        }
    }
}
