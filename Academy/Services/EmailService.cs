using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Academy.Services.Interfaces;

namespace Academy.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var host = _config["SmtpSettings:Host"];
            var port = int.Parse(_config["SmtpSettings:Port"] ?? "587");
            var senderEmail = _config["SmtpSettings:SenderEmail"];
            var password = _config["SmtpSettings:Password"];
            var enableSsl = bool.Parse(_config["SmtpSettings:EnableSsl"] ?? "true");

            using var smtpClient = new SmtpClient(host, port)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = enableSsl
            };

            var mailMessage = new MailMessage(senderEmail, to, subject, body)
            {
                IsBodyHtml = true
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}