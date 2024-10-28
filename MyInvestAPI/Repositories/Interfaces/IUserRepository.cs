using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<User>> GetAllUsersWithPursesAsync();
    Task<IEnumerable<User>> GetAllUsersWithPursesAndActivesAsync();
    Task<User> GetByIdAsync(string userId);
    Task<User> GetUserWithAllPursesByIdAsync(string userId);
    Task<User> GetUserWithAllPursesAndActivesByIdAsync(string userId);
    Task Update(User user);
    Task Delete(User user);
}
