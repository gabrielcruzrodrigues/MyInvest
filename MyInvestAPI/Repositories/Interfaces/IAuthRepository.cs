using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels.Auth;

namespace MyInvestAPI.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<ResponseLoginViewModel> Login(LoginRequestViewModel request);
    Task<ResponseLoginViewModel> Register(RegisterViewModel request);
    Task<object> GetNewTokenUsingRefreshToken(TokenViewModel tokenViewModel);
    Task RecoverPassword(string userId);
}
