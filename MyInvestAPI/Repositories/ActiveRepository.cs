using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Api;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Domain.DTO;
using MyInvestAPI.Domain.Enums;
using MyInvestAPI.Extensions;
using MyInvestAPI.Repositories.Interfaces;
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

        public async Task<Active> CreateAsync(Active active)
        {
            try
            {
                await _context.Actives.AddAsync(active);
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
                .Where(c => c.Enable.Equals(ActiveEnum.ACTIVE))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Active>> GetAllWithPursesAsync()
        {
            return await _context.Actives
                .Include(p => p.Purse)
                .Where(c => c.Enable.Equals(ActiveEnum.ACTIVE))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Active> GetByIdAsync(int id)
        {
            var active = await _context.Actives
                .AsNoTracking()
                .Where(c => c.Enable.Equals(ActiveEnum.ACTIVE))
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (active is null)
                throw new HttpResponseException(404, $"O ativo com o id {id} não foi encontrado!");

            return active;
        }

        public async Task<Active> GetByIdWithPursesAsync(int id)
        {
            var active = await _context.Actives
                .AsNoTracking()
                .Include(p => p.Purse)
                .Where(c => c.Enable.Equals(ActiveEnum.ACTIVE))
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (active is null)
                throw new HttpResponseException(404, $"O ativo com o id {id} não foi encontrado!");

            return active;
        }

        public async Task UpdateAsync(Active active)
        {
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

        public async Task DisableAsync(int id)
        {
            Active active = await _context.Actives
                .FirstOrDefaultAsync(c => c.Active_Id.Equals(id));

            if (active is null)
            {
                throw new HttpResponseException(404, $"O ativo com o id {id} não foi encontrado");
            }

            active.Enable = Domain.Enums.ActiveEnum.DISABLE;

            try
            {
                _context.Actives.Update(active);
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
                .Where(c => c.Enable.Equals(ActiveEnum.ACTIVE))
                .FirstOrDefaultAsync(p => p.Purse_Id == purseId);

            if (Purse is null || !Purse.Actives.Any())
                throw new HttpResponseException(404, $"A carteira com o id {purseId} não foi encontrada!");

            return Purse;
        }

        public async Task<IEnumerable<ActiveReturnForPurseDetailsDTO>> GetActivesForShowInPurseDetails(Purse purse)
        {
            try
            { 
                List<ActiveReturnForPurseDetailsDTO> actives = new();
                foreach (var active in purse.Actives.Where(a => a.Enable.Equals(ActiveEnum.ACTIVE)))
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
