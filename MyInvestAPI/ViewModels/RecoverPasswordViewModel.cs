using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels
{
    public class RecoverPasswordViewModel
    {
        [Required]
        public required string Token { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "O e-mail deve ser válido.")]
        public required string UserEmail { get; set; }
        [StringLength(30, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 30 caracteres.")]
        public required string NewPassword { get; set; }
    }
}
