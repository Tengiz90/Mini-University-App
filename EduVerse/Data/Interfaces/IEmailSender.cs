using System;
using System.Collections.Generic;
using System.Text;

namespace EduVerse.Data.Interfaces
{
    public interface IEmailSender
    {
        void Send(string to, string subject, string body);
    }
}
