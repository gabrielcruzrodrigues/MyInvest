using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Services.Interfaces
{
    public interface IPurseService
    {
        Task<Purse> CreateAsync(CreatePurseViewModel purseViewModel);
        Task<IEnumerable<Purse>> GetAllAsync();
        Task<IEnumerable<Purse>> GetAllWithActivesAsync();
        Task<Purse> GetByIdAsync(int id);
        Task<Purse> GetByIdWithActivesAsync(int id);
        Task Update(int id, UpdatePurseViewModel updatePurseViewModel);
        Task Delete(int id);
    }
}
