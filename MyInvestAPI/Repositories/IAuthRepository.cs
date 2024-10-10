using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels.Auth;

namespace MyInvestAPI.Repositories;

public interface IAuthRepository
{
    Task<ResponseLoginViewModel> Login(LoginRequestViewModel request);
    Task<ResponseLoginViewModel> Register(RegisterViewModel request);
    Task<object> GetNewTokenUsingRefreshToken(TokenViewModel tokenViewModel);
}
