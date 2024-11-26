using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MyInvestAPI.Repositories
{
    public class PasswordResetTokenRepository : IPasswordResetTokenRepository
    {
        private MyInvestContext _context;
        private ILogger<PasswordResetTokenRepository> _logger;

        public PasswordResetTokenRepository(MyInvestContext context, ILogger<PasswordResetTokenRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<PasswordResetToken> CreateAsync(PasswordResetToken passwordResetToken)
        {
            try
            {
                _ = await _context.PasswordResetTokens.AddAsync(passwordResetToken);
                await _context.SaveChangesAsync();
                return passwordResetToken;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro ao tentar criar um PasswordResetToken, Error: {ex.Message}");
                throw new HttpResponseException(400, "Erro ao tentar criar um novo PasswordResetToken");
            }
        }

        public async Task<PasswordResetToken> GetByTokenAsync(string userResetToken)
        {
            if (string.IsNullOrEmpty(userResetToken))
                throw new HttpResponseException(400, "O token é obrigatório");

            var passwordResetToken = await _context.PasswordResetTokens
                                    .Where(p => p.Token.Equals(userResetToken))
                                    .FirstOrDefaultAsync();

            if (passwordResetToken == null)
                throw new HttpResponseException(404, "Token invalido ou não existente");

            return passwordResetToken;
        }
    }
}
