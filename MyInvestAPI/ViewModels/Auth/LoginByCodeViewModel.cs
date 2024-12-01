using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels.Auth
{
    public class LoginByCodeViewModel
    {
        [Required]
        public required string Code { get; set; }
    }
}
