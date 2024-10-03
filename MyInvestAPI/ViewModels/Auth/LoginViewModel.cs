using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels.Auth
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O Username é obrigatório")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        public string? Password { get; set; }
    }
}
