using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories;

public interface IUserRepository
{
    Task<User> CreateAsync(User user);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<User>> GetAllUsersWithPursesAsync();
    Task<IEnumerable<User>> GetAllUsersWithPursesAndActivesAsync();
    Task<User> GetByIdAsync(int id);
    Task<User> GetUserWithAllPursesByIdAsync(int id);
    Task<User> GetUserWithAllPursesAndActivesByIdAsync(int id);
    Task<bool> UpdateAsync(int id, CreateUserViewModel userViewModel);
    Task<bool> DeleteAsync(int id);
}
