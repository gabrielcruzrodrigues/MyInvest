using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;
using System.Diagnostics.Contracts;

namespace MyInvestAPI.Repositories;

public interface IPurseRepository
{
    Task<Purse> CreateAsync(CreatePurseViewModel purseViewModel);
    Task<IEnumerable<Purse>> GetAllAsync();
    Task<IEnumerable<Purse>> GetAllWithActivesAsync();
    Task<Purse> GetByIdAsync(int id);
    Task<Purse> GetByIdWithActivesAsync(int id);
    Task<bool> UpdateAsync(int id, UpdatePurseViewModel updatePurseViewModel);
    Task<bool> DeleteAsync(int id); 
}
