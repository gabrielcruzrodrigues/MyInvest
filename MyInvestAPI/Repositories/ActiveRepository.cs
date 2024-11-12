using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Api;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Domain.DTO;
using MyInvestAPI.Extensions;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.ViewModels;
using System;

namespace MyInvestAPI.Repositories
{
    public class ActiveRepository : IActiveRepository
    {
        public readonly MyInvestContext _context;
        public readonly ILogger<ActiveRepository> _logger;

        public ActiveRepository(MyInvestContext context, ILogger<ActiveRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Active> CreateAsync(CreateActiveViewModel activeViewModel)
        {
            var purse = await _context.Purses.FirstOrDefaultAsync(purse => purse.Purse_Id.Equals(activeViewModel.Purse_Id));

            if (purse is null)
                throw new HttpResponseException(404, $"The purse with id {activeViewModel.Purse_Id} not found!");

            Active active = activeViewModel.CreateActive(purse);

            try
            {
                _context.Actives.Add(active);
                await _context.SaveChangesAsync();
                return active;
            }
            catch(Exception ex)
            {
                _logger.LogError($"Um erro aconteceu ao tentar criar um ativo! err: {ex.Message}");
                throw new HttpResponseException(500, "Um erro aconteceu ao tentar criar um ativo!");
            }
        }

        public async Task<IEnumerable<Active>> GetAllAsync()
        {
            return await _context.Actives
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Active>> GetAllWithPursesAsync()
        {
            return await _context.Actives
                .AsNoTracking()
                .Include(p => p.Purses)
                .ToListAsync();
        }

        public async Task<Active> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "O id deve ser maior que 0!");

            var active = await _context.Actives
                .AsNoTracking()
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (active is null)
                throw new HttpResponseException(404, $"O id com o id {id} não foi encontrado!");

            return active;
        }

        public async Task<Active> GetByIdWithPursesAsync(int id)
        {
            if (id <= 0)
                throw new HttpResponseException(400, "O id deve ser maior que 0!");

            var active = await _context.Actives
                .AsNoTracking()
                .Include(p => p.Purses)
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (active is null)
                throw new HttpResponseException(404, $"O id com o id {id} não foi encontrado!");

            return active;
        }

        public async Task Update(int id, UpdateActiveViewModel updateActiveViewModel)
        {
            var activeVerify = _context.Actives.FirstOrDefault(active => active.Active_Id.Equals(id));

            if (activeVerify is null)
                throw new HttpResponseException(404, $"O id com o id {id} não foi encontrado!");

            var active = updateActiveViewModel.UpdateActive(activeVerify);

            try
            {
                _context.Entry(active).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Um erro aconteceu ao tentar atualizar um ativo! err: {ex.Message}");
                throw new HttpResponseException(500, "Um erro aconteceu ao tentar atualizar um ativo!");;
            }
        }

        public async Task Delete(int id)
        {
            Active active = _context.Actives
                .FirstOrDefault(active => active.Active_Id.Equals(id));

            if (active is null)
                throw new HttpResponseException(404, $"O ativo com o id {id} não foi encontrado!");

            try
            {
                _context.Actives.Remove(active);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Um erro aconteceu ao tentar desabilitar um ativo! err: {ex.Message}");
                throw new HttpResponseException(500, "Um erro aconteceu ao tentar desabilitar um ativo!"); ;
            }
        }

        public async Task<ActiveReturn> SearchActiveAsync(string active, string dYDesiredPercentage)
        {
            try
            {
                return await YahooFinanceApiClient.CreateActiveReturn(active, dYDesiredPercentage);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogError($"O ativo com o id {active} não foi encontrado! err: {ex.Message}");
                throw new HttpResponseException(404, $"O ativo com o id { active } não foi encontrado!");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Um erro ocorreu ao tentar buscar os ativos! err: {ex.Message}");
                throw new HttpResponseException(500, ex.Message);
            }
        }

        public async Task<Purse> GetActivesByPurseId(int purseId)
        {
            var Purse = await _context.Purses
                .Include(p => p.Actives)
                .FirstOrDefaultAsync(p => p.Purse_Id == purseId);

            if (Purse is null || !Purse.Actives.Any())
                throw new HttpResponseException(404, $"A carteira com o id {purseId} não foi encontrada!");

            return Purse;
        }

        public async Task<IEnumerable<ActiveReturnForPurseDetailsDTO>> GetActivesForShowInPurseDetails(int purseId)
        {
            try
            {
                var purse = await _context.Purses
                            .Include(p => p.Actives)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Purse_Id == purseId);

                if (purse is null)
                    throw new HttpResponseException(404, $"A carteira com o id {purseId} não foi encontrada!");

                List<ActiveReturnForPurseDetailsDTO> actives = new();
                foreach (var active in purse.Actives)
                {
                    actives.Add(await YahooFinanceApiClient.CreateActiveReturnForPurseDetails(active.Code, active.DYDesiredPercentage.ToString(), active.Active_Id));
                }

                return actives;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Um erro ocorreu ao tentar buscar os ativos! err: {ex.Message}");
                throw new HttpResponseException(500, ex.Message);
            }
        }
    }
}
