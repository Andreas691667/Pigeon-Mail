using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Email_System
{
    internal class Utility
    {
        public static string username = null!;
        public static string password = null!;

        public static async Task<ImapClient>  establishConnectionImap()
        {
            var client = new ImapClient();

            await client.ConnectAsync("imap.gmail.com", 993, true);
            await client.AuthenticateAsync(username, password);

            return client;
        }

        public static async Task<SmtpClient> establishConnectionSmtp()
        {
            var client = new SmtpClient();

            await client.ConnectAsync("imap.gmail.com", 993, true);
            await client.AuthenticateAsync(username, password);

            return client;
        }
    }
}
