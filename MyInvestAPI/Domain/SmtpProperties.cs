using MyInvestAPI.Extensions;
using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.Domain
{
    public class SmtpProperties
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string SmtpClient { get; set; }
        
        [Required]
        public required string Port
        {
            get => _port;
            set
            {
                if (!string.IsNullOrEmpty(value) && !value.All(char.IsDigit))
                {
                    throw new HttpResponseException(422, "A porta deve conter apenas caracteres numéricos");
                }
                _port = value;
            }

        }
        private string? _port;

        [Required]
        public required string SenderEmail { get; set; }

        [Required]
        public required string PasswordSenderEmail { get; set; }

        [Required]
        public required bool Active { get; set; }

        [Required]
        public required bool Operation { get; set; }
    }
}
