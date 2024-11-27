using MyInvestAPI.Domain;

namespace MyInvestAPI.Repositories.Interfaces
{
    public interface IPasswordResetCodeRepository
    {
        Task<PasswordResetCode> CreateAsync(PasswordResetCode passwordResetCode);
        Task<PasswordResetCode> GetByCodeAsync(string passwordResetCode);
        Task DeletePasswordResetCodeAsync(string code);
    }
}
