using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
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

        public async Task<User> CreateAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersWithPursesAsync()
        {
            return await _context.Users
                .Include(user => user.Purses)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersWithPursesAndActivesAsync()
        {
            return await _context.Users
                .Include(user => user.Purses)
                    .ThenInclude(purse => purse.Actives)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.User_Id == id);
        }

        public async Task<User> GetUserWithAllPursesByIdAsync(int id)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(user => user.Purses)
                .FirstOrDefaultAsync(user => user.User_Id == id);
        }

        public async Task<User> GetUserWithAllPursesAndActivesByIdAsync(int id)
        {
            return await _context.Users
                .Include(user => user.Purses)
                .ThenInclude(purse => purse.Actives)
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.User_Id == id);
        }

        public async Task<bool> UpdateAsync(int id, CreateUserViewModel userViewModel)
        {
            var userVerify = await _context.Users.FirstOrDefaultAsync(user => user.User_Id == id);

            if (userVerify == null)
                return false;

            User user = userViewModel.UpdateUser(userVerify);

            try
            {
                _context.Entry(userVerify).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception err)
            {
                _logger.LogError($"========= Ocorreu um erro ao tentar atualizar o usuário! err: {err.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.User_Id == id);

            if (user == null)
                return false;

            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex)
            {
                _logger.LogError($"========= Ocorreu um erro ao tentar deletar o usuário! err: {ex.Message}");
                return false;
            }
        }
    }
}
