using Microsoft.Extensions.Options;
using MyInvestAPI.Services.Interfaces;
using System.Net;
using System.Net.Mail;
using MyInvestAPI.Extensions;

namespace MyInvestAPI.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        public readonly AuthMessageSenderCredentials Credentials;

        public EmailSender(IOptions<AuthMessageSenderCredentials> credentials, ILogger<EmailSender> logger)
        {
            _logger = logger;
            Credentials = credentials.Value ?? throw new ArgumentNullException(nameof(credentials));
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (string.IsNullOrEmpty(Credentials.SenderEmail) || string.IsNullOrEmpty(Credentials.PasswordSenderEmail))
            {
                throw new Exception("Null credentials");
            }

            await Execute(subject, message, toEmail);
        }

        public async Task Execute(string subject, string message, string toEmail)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(Credentials.SenderEmail, Credentials.PasswordSenderEmail),
                EnableSsl = true
            };

            var body = new MailMessage(Credentials.SenderEmail, toEmail)
            {
                Subject = subject,
                Body = message
            };

            try
            {
                await client.SendMailAsync(body);
                _logger.LogInformation($"O email para {toEmail} foi enviado com sucesso!");
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(400, ex.Message);
            }
        }
    }
}
