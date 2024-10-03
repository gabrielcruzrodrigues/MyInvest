using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels.Auth
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage ="O Username é obrigatório")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "O email é obrigatório")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        public string? Password { get; set; }

        [Required(ErrorMessage ="O número do usuário é obrigatório")]
        public string? PhoneNumber { get; set; }
    }
}
