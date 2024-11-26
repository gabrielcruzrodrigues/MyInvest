using MyInvestAPI.Domain;

namespace MyInvestAPI.Repositories.Interfaces
{
    public interface IPasswordResetTokenRepository
    {
        public Task<PasswordResetToken> CreateAsync(PasswordResetToken passwordResetToken);
        public Task<PasswordResetToken> GetByTokenAsync(string userResetToken);
        Task DeleteResetTokenPasswordAsync(string token);
    }
}
