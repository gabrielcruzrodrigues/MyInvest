using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyInvestAPI.Domain;
using MyInvestAPI.Extensions;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.Services.Interfaces;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Services
{
    public class PurseService : IPurseService
    {
        private readonly IPurseRepository _purseRepository;
        private readonly IUserService _userService;

        public PurseService(IPurseRepository repository, IUserService userService)
        {
            _purseRepository = repository;
            _userService = userService;
        }

        public async Task<Purse> CreateAsync(CreatePurseViewModel purseViewModel)
        {
            var userVerify = await _userService.GetByIdAsync(purseViewModel.User_Id);

            if (userVerify is null)
                throw new HttpResponseException(404, $"The user with ID {purseViewModel.User_Id} not found!");

            Purse purse = purseViewModel.CreatePurse();
            return await _purseRepository.CreateAsync(purse);
        }

        public async Task Delete(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "O id não pode ser menor ou igual a zero!");

            await _purseRepository.Disable(id);
        }

        public async Task<IEnumerable<Purse>> GetAllAsync()
        {
            return await _purseRepository.GetAllAsync();
        }

        public async Task<IEnumerable<Purse>> GetAllWithActivesAsync()
        {
            return await _purseRepository.GetAllWithActivesAsync();
        }

        public async Task<Purse> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "O id não pode ser menor ou igual a zero!");

            return await _purseRepository.GetByIdAsync(id);
        }

        public async Task<Purse> GetByIdWithActivesAsync(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "O id não pode ser menor ou igual a zero!");

            return await _purseRepository.GetByIdWithActivesAsync(id);
        }

        public async Task Update(int id, UpdatePurseViewModel updatePurseViewModel)
        {
            var purseVerify = await _purseRepository.GetByIdAsync(id);
            Purse purse = updatePurseViewModel.UpdatePurse(purseVerify);
            await _purseRepository.Update(purse);
        }
    }
}
