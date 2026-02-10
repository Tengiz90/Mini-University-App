using EduVerse.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace EduVerse.Data.Implementations
{
    public class EmailSender : IEmailSender
    {
        public void Send(string to, string subject, string body)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = new NetworkCredential("gachechiladzetengiz8@gmail.com", "kmrt dgnv usav afcs");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("gachechiladzetengiz8@gmail.com");
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;

            smtpClient.Send(mail);
        }
    }
}
