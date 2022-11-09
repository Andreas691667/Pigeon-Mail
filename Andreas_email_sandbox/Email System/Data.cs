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
using Org.BouncyCastle.Asn1.X509;

namespace Email_System
{
    internal class Data
    {
        public struct msg
        {
            public uint uid { get; set; }
            public string body { get; set; }
            public string from { get; set; }
            public string to { get; set; }
            public string cc { get; set; }
            public string date { get; set; }
            public string subject { get; set; }
            public string attachments { get; set; }
            public string flags { get; set; }
            public string folder { get; set; }

        }

        //public for data,
        //checks for new stuff
        public static ImapClient client = new ImapClient();

        public static int msgCount = 0;

        public static List<string> folderList = new List<string>();

        public static List<string> existingFolders = new List<string>();

        public static List<List<msg>> existingMessages = new List<List<msg>>();

        public static bool exit = false;

        public static List<List<msg>> msgs = new List<List<msg>>();

        public static bool stop = false;



        public static Semaphore s;


        public static async Task listenInboxFolder()
        {
            var client = await Utility.establishConnectionImap();
            var inboxFolder = client.Inbox;

            var i = login.GetInstance;
            var bw = i.inboxBackgroundWorker;

            while (!bw.CancellationPending)
            {
                int currentCount = existingMessages[0].Count;

                inboxFolder.Open(FolderAccess.ReadOnly);
                int newCount = inboxFolder.Count;

                if (newCount != currentCount)
                {
                    if (newCount > currentCount)
                    {
                        var Task = addNewMessage(inboxFolder.FullName, newCount - currentCount);
                        Task.Wait();                        
                    }

                    if(newCount < currentCount)
                    {
                        //not implemented
                    }                    
                }
            }

            client.Disconnect(true);
        }

        public static async Task addNewMessage(string folder, int N)
        {
            var client = await Utility.establishConnectionImap();
            var f = await client.GetFolderAsync(folder);
            f.Open(FolderAccess.ReadOnly);

            //var message = f.GetMessage(f.Count - 1);

            var messages = f.Fetch(f.Count - N, f.Count, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);


            foreach(var item in messages)
            {
                msg message = new msg();
                message = buildMessage(message, item, folder);

                var bodyPart = item.TextBody;

                if (bodyPart != null)
                {
                    var body = (TextPart)f.GetBodyPart(item.UniqueId, bodyPart);
                    var bodyText = body.Text;
                    message.body = bodyText;
                }

                else
                {
                    message.body = "";
                }

                int i = existingFolders.IndexOf(folder);

                existingMessages[i].Add(message);

                Debug.WriteLine("new message added");
            }

            client.Disconnect(true);


            //Debug.WriteLine(message.Subject);
        }

        public static async void listenAllFolders()
        {
            var client = await Utility.establishConnectionImap();

            var i = login.GetInstance;
            var bw = i.allFoldersbackgroundWorker;

            while (!bw.CancellationPending)
            {

                foreach (string folder in existingFolders)
                {
                    IMailFolder f = client.GetFolder(folder);
                    f.Open(FolderAccess.ReadOnly);

                    int index = existingFolders.IndexOf(folder);

                    int currentCount = existingMessages[index].Count;
                    int newCount = f.Count;

                    if (newCount != currentCount)
                    {
                        if (newCount > currentCount)
                        {
                            var Task = addNewMessage(folder, newCount - currentCount);
                            Task.Wait();
                        }

                        if (newCount < currentCount)
                        {
                            //not implemented
                        }
                    }
                }
            }

            client.Disconnect(true);
        }

        public static void loadExistingMessages()
        {
            string filename = "messages.json";

            while(!File.Exists(filename))
            {
            }

            string jsonString = File.ReadAllText(filename);

            var data = JsonSerializer.Deserialize<List<List<msg>>>(jsonString);

            Debug.WriteLine("existing messaes loaded");

            existingMessages = data!;

            Debug.WriteLine(existingMessages.Count);
        }

        public static void loadExistingFolders()
        {
            string filename = "folders.json";

            while (!File.Exists(filename))
            {
            }

            string jsonString = File.ReadAllText(filename);

            var data = JsonSerializer.Deserialize<List<string>>(jsonString);

            Debug.WriteLine("existing folders loaded");

            existingFolders= data!;

            Debug.WriteLine(existingFolders.Count);
        }

        public static async Task loadFolders(BackgroundWorker bw)
        {

            if (!File.Exists("folders.json"))
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
                    client.Disconnect(true);
                    client.Dispose();

                    if (exit)
                        break;
                }
            }

            
            //loadMessages(bw);

            loadExistingFolders();

            //loadMessages(bw);
            Debug.WriteLine("folders loaded");

        }

        public static async Task loadMessages(BackgroundWorker bw)
        {

            if (!File.Exists("messages.json"))
            {
                while (!bw.CancellationPending)
                {
                    var client = await Utility.establishConnectionImap();

                    foreach (string folderName in existingFolders)
                    {
                        var folder = await client.GetFolderAsync(folderName);

                        await folder.OpenAsync(FolderAccess.ReadOnly);

                        var messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                        List<msg> messages = new List<msg>();

                        foreach (var messageSummary in messageSummaries)
                        {
                            Debug.WriteLine(messageSummary.UniqueId.Id);

                            msg message = new msg();

                            message = buildMessage(message, messageSummary, folderName);

                            var bodyPart = messageSummary.TextBody;

                            if (bodyPart != null)
                            {
                                var body = (TextPart)folder.GetBodyPart(messageSummary.UniqueId, bodyPart);
                                var bodyText = body.Text;
                                message.body = bodyText;
                            }

                            else
                            {
                                message.body = "";
                            }

                            Debug.WriteLine(message.flags);

                            messages.Add(message);

                        }

                        msgs.Add(messages);

                    }

                    saveMessages(msgs);
                    break;

                    if (exit)
                        break;
                }                
            }

            Debug.WriteLine("messages loaded");
            loadExistingMessages();
        }

        private static msg buildMessage(msg message, IMessageSummary messageSummary, string folderName)
        {

            message.folder = folderName;

            if (!string.IsNullOrEmpty(messageSummary.Envelope.Subject))
            {
                message.subject = messageSummary.Envelope.Subject.ToString();
            }
            else
            {
                message.subject = "<no subject>";
            }


            if (messageSummary.Envelope.From != null)
            {
                message.from = messageSummary.Envelope.From.ToString();
            }
            else
            {
                message.from = "";
            }


            if (messageSummary.Envelope.To != null)
            {
                message.to = messageSummary.Envelope.To.ToString();
            }
            else
            {
                message.to = "";
            }

            if (messageSummary.Envelope.Cc != null)
            {
                message.cc = messageSummary.Envelope.Cc.ToString();
            }
            else
            {
                message.cc = "";
            }

            if (messageSummary.Envelope.Date != null)
            {
                message.date = messageSummary.Envelope.Date.ToString();
            }
            else
            {
                message.date = "";
            }

            if (messageSummary.Envelope.Subject != null)
            {
                message.subject = messageSummary.Envelope.Subject;
            }
            else
            {
                message.subject = "";
            }

            message.uid = messageSummary.UniqueId.Id;


            if (messageSummary.Attachments != null)
            {
                foreach (var attachment in messageSummary.Attachments)
                {
                    message.attachments += attachment.FileName + ";";
                }
            }

            message.flags = messageSummary.Flags.ToString();

            return message;
        }


        private static void saveFolders()
        {
            var json = JsonSerializer.Serialize(folderList);
            File.WriteAllText("folders.json", json);            
            exit = true;
        }
        public static void saveMessages(List<List<msg>> list)
        {
            var json = JsonSerializer.Serialize(list);
            File.WriteAllText("messages.json", json);
            Debug.WriteLine("all messages saved");
        }
    }
}
