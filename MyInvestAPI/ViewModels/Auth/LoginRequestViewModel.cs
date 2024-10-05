using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels.Auth;

public class LoginRequestViewModel
{
    [Required(ErrorMessage = "O Username é obrigatório")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A senha é obrigatória")]
    public string? Password { get; set; }

    public LoginRequestViewModel(string email, string password)
    {
        Email = email;
        Password = password;
    }
}
