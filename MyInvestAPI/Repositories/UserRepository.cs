using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Domain.Enums;
using MyInvestAPI.Extensions;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        public readonly MyInvestContext _context;
        public readonly ILogger<UserRepository> _logger;

        public UserRepository(MyInvestContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .Where(u => u.Active == ActiveEnum.ACTIVE)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersWithPursesAsync()
        {
            return await _context.Users
                .Include(user => user.Purses)
                .Where(u => u.Active == ActiveEnum.ACTIVE)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersWithPursesAndActivesAsync()
        {
            return await _context.Users
                .Where(u => u.Active == ActiveEnum.ACTIVE)
                .Include(user => user.Purses)
                    .ThenInclude(purse => purse.Actives)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Active == ActiveEnum.ACTIVE)
                .FirstOrDefaultAsync(user => user.Id == id.ToString());

            if (user is null)
                throw new HttpResponseException(404, $"O usuário com o ID {id} não foi encontrado!");

            return user;
        }

        public async Task<User> GetUserWithAllPursesByIdAsync(string id)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Include(user => user.Purses)
                .Where(u => u.Active == ActiveEnum.ACTIVE)
                .FirstOrDefaultAsync(user => user.Id == id.ToString());

            if (user is null)
                throw new HttpResponseException(404, $"O usuário com o ID {id} não foi encontrado!");

            return user;
        }

        public async Task<User> GetUserWithAllPursesAndActivesByIdAsync(string id)
        {
            var user = await _context.Users
                .Include(user => user.Purses)
                    .ThenInclude(purse => purse.Actives)
                    .AsNoTracking()
                .Where(u => u.Active == ActiveEnum.ACTIVE)
                .FirstOrDefaultAsync(user => user.Id == id.ToString());

            if (user is null)
                throw new HttpResponseException(404, $"O usuário com o ID {id} não foi encontrado!");

            return user;
        }

        public async Task Update(User user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception err)
            {
                _logger.LogError($"========= Ocorreu um erro ao tentar atualizar o usuário! err: {err.Message}");
                throw new HttpResponseException(500, "Ocorreu um erro ao tentar atualizar o usuário!");
            }
        }

        public async Task Disable(string userId)
        {
            try
            {
                var user = await GetByIdAsync(userId);
                user.Active = ActiveEnum.DISABLE;
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Ocorreu um erro ao tentar desativar o usuário! err: {ex.Message}");
                throw new HttpResponseException(500, "Ocorreu um erro ao tentar desativar o usuário!");
            }
        }
    }
}
