using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;


namespace ZeroGravity.Services
{
    public class EmailService : IEmailService
    {
        private readonly AppSettings _appSettings;

        public EmailService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public void Send(string to, string subject, string html, string from = null)
        {
            if (_appSettings.InTestMode)
            {
                return; 
            }

            var mail = new MailMessage();
            mail.From = new MailAddress(from ?? _appSettings.EmailFrom);
            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = html;
            mail.IsBodyHtml = true;


            var client = new SmtpClient
            {
                Host = _appSettings.SmtpHost,
                Port = _appSettings.SmtpPort,
                Credentials = new NetworkCredential(_appSettings.SmtpUser, _appSettings.SmtpPass),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            client.Send(mail);
        }
    }
}