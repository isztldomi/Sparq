using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IEmailsService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
