using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;
using MyInvestAPI.ViewModels.Auth;

namespace MyInvestAPI.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<ResponseLoginViewModel> Login(LoginRequestViewModel request);
    Task<ResponseLoginViewModel> Register(RegisterViewModel request);
    Task<object> GetNewTokenUsingRefreshToken(TokenViewModel tokenViewModel);
    Task RequestRecoverPassword(string userEmail);
    Task RecoverPassword(RecoverPasswordViewModel request);
    Task<string> SaveTokenAndPrepareMessageForSendToUser(string userEmail);
}
