using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Extensions;
using MyInvestAPI.Repositories.Interfaces;

namespace MyInvestAPI.Repositories
{
    public class PasswordResetCodeRepository : IPasswordResetCodeRepository
    {
        private MyInvestContext _context;
        private ILogger<PasswordResetCodeRepository> _logger;

        public PasswordResetCodeRepository(MyInvestContext context, ILogger<PasswordResetCodeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }   

        public async Task<PasswordResetCode> CreateAsync(PasswordResetCode passwordResetCode)
        {
            try
            {
                _ = await _context.PasswordResetCodes.AddAsync(passwordResetCode);
                await _context.SaveChangesAsync();
                return passwordResetCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro ao tentar criar um PasswordResetCode, Error: {ex.Message}");
                throw new HttpResponseException(400, "Erro ao tentar criar um novo PasswordResetCode");
            }
        }

        public async Task DeletePasswordResetCodeAsync(string code)
        {
            var codeForDelete = await _context.PasswordResetCodes
                        .Where(p => p.Code.Equals(code))
                        .FirstOrDefaultAsync();

            if (codeForDelete is null)
            {
                throw new HttpResponseException(404, "Código inválido ou inexistente!");
            }

            try
            {
                _context.PasswordResetCodes.Remove(codeForDelete);
                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao tentar deletar PasswordResetCode!", ex.Message);
                throw new HttpResponseException(400, "Erro ao tentar deletar PasswordResetCode");
            }
        }

        public async Task<PasswordResetCode> GetByCodeAsync(string Code)
        {
            if (string.IsNullOrEmpty(Code))
                throw new HttpResponseException(400, "O código é obrigatório");

            var passwordResetCode = await _context.PasswordResetCodes
                                    .Where(p => p.Code.Equals(Code))
                                    .FirstOrDefaultAsync();

            return passwordResetCode;
        }
    }
}
