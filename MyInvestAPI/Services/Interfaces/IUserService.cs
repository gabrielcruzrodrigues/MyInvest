using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetAllUsersWithPursesAsync();
        Task<IEnumerable<User>> GetAllUsersWithPursesAndActivesAsync();
        Task<User> GetByIdAsync(string id);
        Task<User> GetUserWithAllPursesByIdAsync(string id);
        Task<User> GetUserWithAllPursesAndActivesByIdAsync(string id);
        Task Update(string id, CreateUserViewModel userViewModel);
        Task Delete(string id);
    }
}
