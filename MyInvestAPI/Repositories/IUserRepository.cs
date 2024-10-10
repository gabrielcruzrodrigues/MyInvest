using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<IEnumerable<User>> GetAllUsersWithPursesAsync();
    Task<IEnumerable<User>> GetAllUsersWithPursesAndActivesAsync();
    Task<User> GetByIdAsync(string userId);
    Task<User> GetUserWithAllPursesByIdAsync(string userId);
    Task<User> GetUserWithAllPursesAndActivesByIdAsync(string userId);
    void Update(string userId, CreateUserViewModel userViewModel);
    void Delete(string userId);
}
