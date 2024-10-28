using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Domain;
using MyInvestAPI.Extensions;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.Services.Interfaces;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _repository.GetAllUsersAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersWithPursesAsync()
        {
            return await _repository.GetAllUsersWithPursesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersWithPursesAndActivesAsync()
        {
            return await _repository.GetAllUsersWithPursesAndActivesAsync();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(400, "O id não pode ser nulo ou vazio!");

            return await _repository.GetByIdAsync(id);
        }

        public async Task<User> GetUserWithAllPursesByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(400, "O id não pode ser nulo ou vazio!");

            return await _repository.GetUserWithAllPursesByIdAsync(id);
        }

        public async Task<User> GetUserWithAllPursesAndActivesByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(400, "O id não pode ser nulo ou vazio!");

            return await _repository.GetUserWithAllPursesAndActivesByIdAsync(id);
        }

        public async Task Update(string id, CreateUserViewModel userViewModel)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(400, "O id não pode ser nulo ou vazio!");

            User userVerify = await _repository.GetByIdAsync(id);
            User user = userViewModel.UpdateUser(userVerify);
            await _repository.Update(user);
        }

        public async Task Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new HttpResponseException(400, "O id não pode ser nulo ou vazio!");

            User user = await _repository.GetByIdAsync(id);
            await _repository.Delete(user);
        }
    }
}
