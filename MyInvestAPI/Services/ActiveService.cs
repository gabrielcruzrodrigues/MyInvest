using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Domain;
using MyInvestAPI.Domain.DTO;
using MyInvestAPI.Extensions;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.Services.Interfaces;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Services
{
    public class ActiveService : IActiveService
    {
        private readonly IPurseService _purseService;
        private readonly IActiveRepository _activeRepository;

        public ActiveService(IPurseService purseService, IActiveRepository activeRepository)
        {
            _purseService = purseService;
            _activeRepository = activeRepository;
        }  

        public async Task<Active> CreateAsync(CreateActiveViewModel activeViewModel)
        {
            Purse purse = await _purseService.GetByIdAsync(activeViewModel.Purse_Id);

            Active active = activeViewModel.CreateActive(purse);
            return await _activeRepository.CreateAsync(active);
        }

        public async Task Delete(int id)
        {
            await _activeRepository.DisableAsync(id);
        }

        public async Task<Purse> GetActivesByPurseId(int purseId)
        {
            return await _activeRepository.GetActivesByPurseId(purseId);
        }

        public async Task<IEnumerable<ActiveReturnForPurseDetailsDTO>> GetActivesForShowInPurseDetails(int purseId)
        {
            var purse = await _activeRepository.GetActivesByPurseId(purseId);
            return await _activeRepository.GetActivesForShowInPurseDetails(purse);
        }

        public async Task<IEnumerable<Active>> GetAllAsync()
        {
            return await _activeRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Active>> GetAllWithPursesAsync()
        {
            return await _activeRepository.GetAllWithPursesAsync();
        }

        public async Task<Active> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "O id deve ser maior que 0!");

            return await _activeRepository.GetByIdAsync(id);
        }

        public async Task<Active> GetByIdWithPursesAsync(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "O id deve ser maior que 0!");

            return await _activeRepository.GetByIdWithPursesAsync(id);
        }

        public async Task<ActiveReturn> SearchActiveAsync(string active, string dYDesiredPercentage)
        {
            return await _activeRepository.SearchActiveAsync(active, dYDesiredPercentage);
        }

        public async Task Update(int id, UpdateActiveViewModel updateActiveViewModel)
        {
            var activeVerify = await _activeRepository.GetByIdAsync(id);
            Active active = updateActiveViewModel.UpdateActive(activeVerify);
            await _activeRepository.UpdateAsync(active);
        }
    }
}
