using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels.Auth;

namespace MyInvestAPI.Repositories;

public interface IAuthRepository
{
    Task<ResponseLoginViewModel> Login(LoginRequestViewModel request);
    Task<ResponseLoginViewModel> register(RegisterViewModel request);
}
