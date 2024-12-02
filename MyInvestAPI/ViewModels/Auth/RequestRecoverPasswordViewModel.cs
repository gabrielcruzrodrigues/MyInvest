using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels.Auth
{
    public class RequestRecoverPasswordViewModel
    {
        [Required]
        public required string UserId { get; set; }
    }
}
