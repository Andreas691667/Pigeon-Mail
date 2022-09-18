using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit; //allow us to use mime messages

namespace mailApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //create a new mime message
            MimeMessage message = new MimeMessage();

            message.From.Add(new MailboxAddress("Sandbox afsender", "sandboxsandkasse@gmail.com"));

            message.To.Add(MailboxAddress.Parse("idablauenfeldtdam@gmail.com"));

            message.Subject = "This is the header!";

            message.Body = new TextPart("plain")  
               {
                Text = @"Hej smukke, elsker dig<3;"
            };

            Console.WriteLine("Email: ");
            string emailaddress = Console.ReadLine();
            Console.WriteLine("Password: ");
            string password = Console.ReadLine();

            SmtpClient client = new SmtpClient();

            try
            {
                client.Connect("smtp.gmail.com", 465, true);
                client.Authenticate(emailaddress, password);
                client.Send(message);

                Console.WriteLine("Message sent!");

            }

            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }



        }
    }
}