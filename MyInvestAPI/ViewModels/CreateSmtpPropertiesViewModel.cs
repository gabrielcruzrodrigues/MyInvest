using System.ComponentModel.DataAnnotations;
using MyInvestAPI.Domain;

namespace MyInvestAPI.ViewModels
{
    public class CreateSmtpPropertiesViewModel
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        public required string SmtpClient { get; set; }

        [Required]
        public required string Port { get; set; }

        [Required]
        public required string SenderEmail { get; set; }

        [Required]
        public required string PasswordSenderEmail { get; set; }

        public SmtpProperties CreateNewSmtpProperties()
        {
            return new SmtpProperties()
            {
                Name = Name,
                SmtpClient = SmtpClient,
                Port = Port,
                SenderEmail = SenderEmail,
                PasswordSenderEmail = PasswordSenderEmail,
                Active = true,
                Operation = false
            };
        }
    }
}
