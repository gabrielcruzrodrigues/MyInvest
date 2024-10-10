using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Extensions;
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

        public async Task<User> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(400, "O id não pode ser nulo ou vazio!");

            var user = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == id.ToString());

            if (user is null)
                throw new HttpResponseException(404, $"O usuário com o ID {id} não foi encontrado!");

            return user;
        }

        public async Task<User> GetUserWithAllPursesByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(400, "O id não pode ser nulo ou vazio!");

            var user = await _context.Users
                .AsNoTracking()
                .Include(user => user.Purses)
                .FirstOrDefaultAsync(user => user.Id == id.ToString());

            if (user is null)
                throw new HttpResponseException(404, $"O usuário com o ID {id} não foi encontrado!");

            return user;
        }

        public async Task<User> GetUserWithAllPursesAndActivesByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(400, "O id não pode ser nulo ou vazio!");

            var user = await _context.Users
                .Include(user => user.Purses)
                .ThenInclude(purse => purse.Actives)
                .AsNoTracking()
                .FirstOrDefaultAsync(user => user.Id == id.ToString());

            if (user is null)
                throw new HttpResponseException(404, $"O usuário com o ID {id} não foi encontrado!");

            return user;
        }

        public void Update(string id, CreateUserViewModel userViewModel)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(400, "O id não pode ser nulo ou vazio!");

            User userVerify = _context.Users.FirstOrDefault(user => user.Id == id.ToString());

            if (userVerify == null)
                throw new HttpResponseException(404, $"O usuário com o ID {id} não foi encontrado!");

            User user = userViewModel.UpdateUser(userVerify);

            try
            {
                _context.Entry(userVerify).State = EntityState.Modified;
                _context.SaveChangesAsync();
            }
            catch (Exception err)
            {
                _logger.LogError($"========= Ocorreu um erro ao tentar atualizar o usuário! err: {err.Message}");
                throw new HttpResponseException(500, "Ocorreu um erro ao tentar atualizar o usuário!");
            }
        }

        public void Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(400, "O id não pode ser nulo ou vazio!");

            User user = _context.Users.FirstOrDefault(user => user.Id == id.ToString());

            if (user == null)
                throw new HttpResponseException(404, $"O Usuário com o ID {id} não foi encontrado!");

            try
            {
                _context.Users.Remove(user);
                _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Ocorreu um erro ao tentar deletar o usuário! err: {ex.Message}");
                throw new HttpResponseException(500, "Ocorreu um erro ao tentar deletar o usuário!");
            }
        }
    }
}
