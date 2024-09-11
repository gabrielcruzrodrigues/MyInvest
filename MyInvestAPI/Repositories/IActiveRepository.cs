using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories
{
    public interface IActiveRepository
    {
        Task<Active> CreateAsync(CreateActiveViewModel activeViewModel);
        Task<IEnumerable<Active>> GetAllAsync();
        Task<IEnumerable<Active>> GetAllWithPursesAsync();
        Task<Active> GetByIdAsync(int id);
        Task<Active> GetByIdWithPursesAsync(int id);
        Task<bool> UpdateAsync(int id, UpdateActiveViewModel updateActiveViewModel);
        Task<bool> DeleteAsync(int id);
        Task<ActiveReturn> SearchActiveAsync(string active);
        Task<Purse> GetActivesByPurseId(int purseId);
    }
}
