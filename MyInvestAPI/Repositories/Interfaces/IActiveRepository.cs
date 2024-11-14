using MyInvestAPI.Domain;
using MyInvestAPI.Domain.DTO;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories.Interfaces
{
    public interface IActiveRepository
    {
        Task<Active> CreateAsync(Active active);
        Task<IEnumerable<Active>> GetAllAsync();
        Task<IEnumerable<Active>> GetAllWithPursesAsync();
        Task<Active> GetByIdAsync(int id);
        Task<Active> GetByIdWithPursesAsync(int id);
        Task UpdateAsync(Active active);
        Task DisableAsync(Active active);
        Task<ActiveReturn> SearchActiveAsync(string active, string dYDesiredPercentage);
        Task<Purse> GetActivesByPurseId(int purseId);
        Task<IEnumerable<ActiveReturnForPurseDetailsDTO>> GetActivesForShowInPurseDetails(Purse purse);
    }
}
