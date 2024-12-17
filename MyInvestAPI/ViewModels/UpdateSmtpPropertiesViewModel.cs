using System.ComponentModel.DataAnnotations;
using MyInvestAPI.Domain;

namespace MyInvestAPI.ViewModels
{
    public class UpdateSmtpPropertiesViewModel
    {
        public string? Name { get; set; }
        public string? SmtpClient { get; set; }

        public string? Port { get; set; }

        public string? SenderEmail { get; set; }

        public string? PasswordSenderEmail { get; set; }

        public SmtpProperties UpdateSmtpProperties(SmtpProperties smtpProperties)
        {
            if (Name is not null)
            {
                smtpProperties.Name = Name;
            }

            if (SmtpClient is not null)
            {
                smtpProperties.SmtpClient = SmtpClient;
            }

            if (Port is not null)
            {
                smtpProperties.Port = Port;
            }

            if (SenderEmail is not null)
            {
                smtpProperties.SenderEmail = SenderEmail;
            }

            if (PasswordSenderEmail is not null)
            {
                smtpProperties.PasswordSenderEmail = PasswordSenderEmail;
            }

            return smtpProperties;
        }
    }
}
