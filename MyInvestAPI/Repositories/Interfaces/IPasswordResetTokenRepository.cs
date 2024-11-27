using MyInvestAPI.Domain;

namespace MyInvestAPI.Repositories.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        Task<PasswordResetToken> CreateAsync(PasswordResetToken passwordResetToken);
        Task<PasswordResetToken> GetByTokenAsync(string userResetToken);
        Task DeletePasswordResetTokenAsync(string token);
    }
}
