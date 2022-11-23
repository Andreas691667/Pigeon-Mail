using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
            public string sender { get; set; }
            public string to { get; set; }
            public string cc { get; set; }
            public string date { get; set; }
            public string subject { get; set; }
            public string attachments { get; set; }
            public string flags { get; set; }
            public string folder { get; set; }
        }


        public static string trashFolderName = "";
        public static string draftFolderName = "";
        public static string flaggedFolderName = "";
        public static string allFolderName = "";

        //list to load folders on login
        private static List<string> folderList = new List<string>();

        //list to store folders
        public static List<string> existingFolders = new List<string>();

        //list to store messages
        public static List<List<msg>> existingMessages = new List<List<msg>>();

        public static bool exit = false;

        //private list to load messages on login
        private static List<List<msg>> msgs = new List<List<msg>>();

        public static async void listenInboxFolder()
        {
            var client = await Utility.establishConnectionImap();
            var inboxFolder = client.Inbox;

            var i = login.GetInstance;
            var bw = i.folderListenerBW;
            
            while (!bw.CancellationPending)
            {
                int currentCount = existingMessages[0].Count; //local count
                inboxFolder.Open(FolderAccess.ReadOnly);
                int newCount = inboxFolder.Count; //server cocunt

                if (newCount != currentCount)
                {
                    //if there is more messages on the server than we have locally
                    if (newCount > currentCount)
                    {
                        //Debug.WriteLine("New count = " + newCount + " Old count: " + currentCount);

                       // var Task = addNewMessage(inboxFolder.FullName, newCount - currentCount);
                        //Task.Wait();
                    }

                    if(newCount < currentCount)
                    {
                        //this is when a message is deleted on server
                        //not implemented
                    }                    
                }
            }

            client.Disconnect(true);
        }

        private static async Task addNewMessage(string folder, IList<UniqueId> uids, List<uint> ids)
        {
            var client = await Utility.establishConnectionImap();

            try
            {
                var f = await client.GetFolderAsync(folder);
                f.Open(FolderAccess.ReadOnly);

                int folderIndex = existingFolders.IndexOf(folder);
                var messages = f.Fetch(uids, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                List<uint> uidsExisting = new List<uint>();

                foreach (var m in existingMessages[folderIndex])
                {
                    uidsExisting.Add(m.uid);
                }


                foreach (var item in messages)
                {
                    if (!uidsExisting.Contains(item.UniqueId.Id))
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

                        existingMessages[folderIndex].Add(message);

                        Debug.WriteLine("new message added to: " + folder + folderIndex);
                    }
                }
            }

            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

            finally
            {
                client.Disconnect(true);
            }
        }

        public static async void listenAllFolders()
        {
            var client = await Utility.establishConnectionImap();

            var i = login.GetInstance;
            var bw = i.folderListenerBW;

            while (!bw.CancellationPending)
            {
                foreach (string folder in existingFolders)
                {
                    IMailFolder f = client.GetFolder(folder);
                    f.Open(FolderAccess.ReadOnly);                    

                    int index = existingFolders.IndexOf(folder);

                    if (index != -1)
                    {
                        int currentCount = existingMessages[index].Count;
                        int newCount = f.Count;

                        if (newCount != currentCount)
                        {
                            var uids = f.Search(MailKit.Search.SearchQuery.All);
                            Debug.WriteLine("New count = " + newCount + " Old count: " + currentCount + " In folder: " + f.FullName);

                            List<uint> uidsId = new List<uint>();

                            foreach (var u in uids)
                            {
                                uidsId.Add(u.Id);
                            }

                            if (newCount > currentCount)
                            {
                               var Task = addNewMessage(f.FullName, uids, uidsId);
                               Task.Wait();
                            }

                            if (newCount < currentCount)
                            {
                                var summaries = f.Fetch(uids, MessageSummaryItems.Envelope);
                                
                                List<string> subjects = new List<string>();

                                foreach(var s in summaries)
                                {
                                    subjects.Add(s.Envelope.Subject);
                                }

                                var task = deleteMessage(uidsId, f.FullName);
                                Thread.Sleep(1000);
                                task.Wait();
                            }
                        }
                    }
                }
            }

            client.Disconnect(true);
        }

        private static async Task deleteMessage(List<uint> ids, string folder)
        {
            int folderIndex = existingFolders.IndexOf(folder);

            foreach (var m in existingMessages[folderIndex])
            {
                if (!ids.Contains(m.uid))
                {
                    existingMessages[folderIndex].Remove(m);
                    Debug.WriteLine("Message removed from folder: " + folder);

                    saveMessages(existingMessages);
                    Task task = loadExistingMessages();
                    task.Wait();
                    return;
                }
            }

        }

        public static async Task loadExistingMessages()
        {
            string filename = Utility.username + "messages.json";


            while (!File.Exists(filename))
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
            string filename = Utility.username + "folders.json";

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

            if (!File.Exists(Utility.username + "folders.json"))
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
            if (!File.Exists(Utility.username + "messages.json"))
            {
                while (!bw.CancellationPending)
                {
                    var client = await Utility.establishConnectionImap();

                    int i = 0;

                    foreach (string folderName in existingFolders)
                    {
                        List<msg> newlist = new List<msg>();
                        msgs.Add(newlist);

                        var folder = await client.GetFolderAsync(folderName);

                        await folder.OpenAsync(FolderAccess.ReadOnly);

                        var messageSummaries = await folder.FetchAsync(0, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure | MessageSummaryItems.Flags);

                        List<msg> messages = new List<msg>();

                        foreach (var messageSummary in messageSummaries.Reverse())
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

                            messages.Add(message);

                            if (messages.Count % 10 == 0 && messages.Count != 0)
                            {
                                msgs[i].AddRange(messages);
                                saveMessages(msgs);
                                Task t = loadExistingMessages();
                                t.Wait();
                                messages.Clear();
                            }
                        }

                        msgs[i].AddRange(messages);
                        saveMessages(msgs);
                        Task task = loadExistingMessages();
                        task.Wait();

                        i++;
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

            if(messageSummary.Envelope.Sender != null)
            {
                message.sender = messageSummary.Envelope.Sender.ToString();
            }

            else
            {
                message.sender = "";
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
            File.WriteAllText(Utility.username + "folders.json", json);            
            exit = true;
        }
        public static void saveMessages(List<List<msg>> list)
        {
            var json = JsonSerializer.Serialize(list);
//            File.WriteAllText("messages.json", json);
            File.WriteAllText(Utility.username + "messages.json", json);
            Debug.WriteLine("all messages saved");
        }


        //maybe add even more names?
        static string[] TrashFolderNames = { "Deleted", "Trash", "Papirkurv" };

        //get the trashFolder and return the Imailfolder
        public static async Task<IMailFolder> GetTrashFolder(ImapClient client = null!)
        {
            if (client == null)
            {
                client = await Utility.establishConnectionImap();
            }

            if ((client.Capabilities & (ImapCapabilities.SpecialUse | ImapCapabilities.XList)) != 0)
            {
                var trashFolder = client.GetFolder(SpecialFolder.Trash);
                trashFolderName = trashFolder.FullName;
                return trashFolder;
            }

            else
            {
                var personal = client.GetFolder(client.PersonalNamespaces[0]);

                foreach (var folder in personal.GetSubfolders(false, CancellationToken.None))
                {
                    foreach (var name in TrashFolderNames)
                    {
                        if (folder.Name == name)
                        {
                            trashFolderName = folder.FullName;
                            return folder;
                        }
                    }
                }
            }

            return null!;
        }

        static string[] DraftFolderNames = { "Drafts", "Kladder", "Draft" };
        public static async Task<IMailFolder> GetDraftFolder(ImapClient client = null!)
        {
            if (client == null)
            {
                client = await Utility.establishConnectionImap();
            }

            if ((client.Capabilities & (ImapCapabilities.SpecialUse | ImapCapabilities.XList)) != 0)
            {
                var draftFolder = client.GetFolder(SpecialFolder.Drafts);
                draftFolderName = draftFolder.FullName;
                return draftFolder;
            }

            else
            {
                var personal = client.GetFolder(client.PersonalNamespaces[0]);

                foreach (var folder in personal.GetSubfolders(false, CancellationToken.None))
                {
                    foreach (var name in DraftFolderNames)
                    {
                        if (folder.Name == name)
                        {
                            draftFolderName = folder.FullName;
                            return folder;
                        }
                    }
                }
            }

            return null!;
        }


        static string[] FlaggedFolderNames = { "Flagged", "Starred", "Important" };
        public static async Task<IMailFolder> GetFlaggedFolder(ImapClient client = null!)
        {
            if (client == null)
            {
                client = await Utility.establishConnectionImap();
            }

            if ((client.Capabilities & (ImapCapabilities.SpecialUse | ImapCapabilities.XList)) != 0)
            {
                var flaggedFolder = client.GetFolder(SpecialFolder.Flagged);
                flaggedFolderName = flaggedFolder.FullName;
                return flaggedFolder;
            }

            else
            {
                var personal = client.GetFolder(client.PersonalNamespaces[0]);

                foreach (var folder in personal.GetSubfolders(false, CancellationToken.None))
                {
                    foreach (var name in FlaggedFolderNames)
                    {
                        if (folder.Name == name)
                        {
                            flaggedFolderName = folder.FullName;
                            return folder;
                        }
                    }
                }
            }

            return null!;
        }


        static string[] AllFolderNames = { "All", "Alle mails", "Important" };
        public static async Task<IMailFolder> GetAllFolder(ImapClient client = null!)
        {
            if (client == null)
            {
                client = await Utility.establishConnectionImap();
            }

            if ((client.Capabilities & (ImapCapabilities.SpecialUse | ImapCapabilities.XList)) != 0)
            {
                var allFolder = client.GetFolder(SpecialFolder.All);
                allFolderName = allFolder.FullName;
                return allFolder;
            }

            else
            {
                var personal = client.GetFolder(client.PersonalNamespaces[0]);

                foreach (var folder in personal.GetSubfolders(false, CancellationToken.None))
                {
                    foreach (var name in AllFolderNames)
                    {
                        if (folder.Name == name)
                        {
                            allFolderName = folder.FullName;
                            return folder;
                        }
                    }
                }
            }

            return null!;
        }


        public static void deleteFiles()
        {
            File.Delete(Utility.username + "messages.json");
            File.Delete(Utility.username + "folders.json");
        }
    }
}
