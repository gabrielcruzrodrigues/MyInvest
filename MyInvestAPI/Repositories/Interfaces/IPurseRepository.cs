using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;
using System.Diagnostics.Contracts;

namespace MyInvestAPI.Repositories.Interfaces;

public interface IPurseRepository
{
    Task<Purse> CreateAsync(Purse purse);
    Task<IEnumerable<Purse>> GetAllAsync();
    Task<IEnumerable<Purse>> GetAllWithActivesAsync();
    Task<Purse> GetByIdAsync(int id);
    Task<Purse> GetByIdWithActivesAsync(int id);
    Task Update(Purse purse);
    Task Disable(int id);
}
