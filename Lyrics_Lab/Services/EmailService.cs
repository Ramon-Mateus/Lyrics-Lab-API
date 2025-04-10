using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Lyrics_Lab.Services
{
    public class EmailService : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            string smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST")!;
            string smtpPort = Environment.GetEnvironmentVariable("SMTP_PORT")!;
            string smtpUser = Environment.GetEnvironmentVariable("SMTP_USER")!;
            string smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD")!;

            var client = new SmtpClient(smtpHost)
            {
                Port = int.Parse(smtpPort),
                Credentials = new NetworkCredential(smtpUser, smtpPassword),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUser),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await client.SendMailAsync(mailMessage);
        }
    }
}