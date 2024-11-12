using MyInvestAPI.Domain;
using MyInvestAPI.Domain.DTO;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories.Interfaces
{
    public interface IActiveRepository
    {
        Task<Active> CreateAsync(CreateActiveViewModel activeViewModel);
        Task<IEnumerable<Active>> GetAllAsync();
        Task<IEnumerable<Active>> GetAllWithPursesAsync();
        Task<Active> GetByIdAsync(int id);
        Task<Active> GetByIdWithPursesAsync(int id);
        Task Update(int id, UpdateActiveViewModel updateActiveViewModel);
        Task Delete(int id);
        Task<ActiveReturn> SearchActiveAsync(string active, string dYDesiredPercentage);
        Task<Purse> GetActivesByPurseId(int purseId);
        Task<IEnumerable<ActiveReturnForPurseDetailsDTO>> GetActivesForShowInPurseDetails(int purseId);
    }
}
