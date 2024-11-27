using MyInvestAPI.Domain;
using MyInvestAPI.Domain.Enums;
using MyInvestAPI.ViewModels;
using MyInvestAPI.ViewModels.Auth;

namespace MyInvestAPI.Repositories.Interfaces;

public interface IAuthRepository
{
    Task<ResponseLoginViewModel> Login(LoginRequestViewModel request);
    Task<ResponseLoginViewModel> Register(RegisterViewModel request);
    Task<object> GetNewTokenUsingRefreshToken(TokenViewModel tokenViewModel);
    Task RequestRecoverPassword(string userId);
    Task RecoverPassword(RecoverPasswordViewModel request);
    Task RequestCodeForForgottenPassword(string userId);
    Task<string> SaveTokenOrCodeAndPrepareMessageForSendToUser(User user, EntityOptionEnum option);
}
