using Microsoft.Extensions.Options;
using Sparq.DataAccess.Config;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Sparq.DataAccess.Services
{
    internal class SmtpEmailsService
    {
        private readonly EmailSettings _emailSettings;

        public SmtpEmailsService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            using MailMessage mail = new MailMessage();
            mail.From = new MailAddress(_emailSettings.FromEmail);
            mail.To.Add(toEmail);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            using SmtpClient smtp = new SmtpClient(_emailSettings.Host, _emailSettings.Port);
            smtp.Credentials = new NetworkCredential(_emailSettings.UserName, _emailSettings.Password);
            smtp.EnableSsl = _emailSettings.EnableSsl;
            await smtp.SendMailAsync(mail);
        }
    }
}
