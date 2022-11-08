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
            public string uid { get; set; }
            public string body { get; set; }
            public string from { get; set; }
            public string to { get; set; }
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


        public static void newMessages()
        {
            var inboxFolder = client.Inbox;
            
            //check for new messages, i.e. messageCount changed event.
            //update messagelist
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



        public static async void loadFolders(BackgroundWorker bw)
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

        public static async void loadMessages(BackgroundWorker bw)
        {

            if (!File.Exists("messages.json"))
            {
                while (!bw.CancellationPending)
                {                    
                    var client = await Utility.establishConnectionImap();

                    foreach (string folderName in existingFolders)
                    {
                        Debug.WriteLine("we here");

                        var folder = await client.GetFolderAsync(folderName);

                        await folder.OpenAsync(FolderAccess.ReadOnly);

                        var messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                        List<msg> messages = new List<msg>();   

                        foreach (var messageSummary in messageSummaries)
                        {
                            Debug.WriteLine(messageSummary.UniqueId.Id);

                            msg message = new msg();

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

                            message.uid = messageSummary.UniqueId.ToString();


                            if (messageSummary.Attachments != null)
                            {
                                foreach (var attachment in messageSummary.Attachments)
                                {
                                    message.attachments += attachment.FileName + ";";
                                }
                            }


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

                            message.flags = messageSummary.Flags.ToString();
                            Debug.WriteLine(message.flags);

                            messages.Add(message);

                        }

                        msgs.Add(messages);

                    }                   

                    if (exit)
                        break;

                    saveMessages();
                }                
            }

            Debug.WriteLine("messages loaded");
            loadExistingMessages();
        }
        private static void saveFolders()
        {
            var json = JsonSerializer.Serialize(folderList);
            File.WriteAllText("folders.json", json);            
            exit = true;
        }
        private static void saveMessages()
        {
            var json = JsonSerializer.Serialize(msgs);
            File.WriteAllText("messages.json", json);
            Debug.WriteLine("all messages saved");
        }
    }
}
