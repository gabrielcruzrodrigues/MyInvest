using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Api;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

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
            {
                _logger.LogError("The purse not found");
                return null;
            }

            Active active = activeViewModel.CreateActive(purse);

            try
            {
                _context.Actives.Add(active);
                await _context.SaveChangesAsync();
                return active;
            }
            catch(Exception ex)
            {
                _logger.LogError($"An Error occured when tryning create active! err: {ex.Message}");
                return null;
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
            return await _context.Actives
                .AsNoTracking()
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));
        }
        public async Task<Active> GetByIdWithPursesAsync(int id)
        {
            return await _context.Actives
                .AsNoTracking()
                .Include(p => p.Purses)
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));
        }
        public async Task<bool> UpdateAsync(int id, UpdateActiveViewModel updateActiveViewModel)
        {
            var activeVerify = await _context.Actives.FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (activeVerify is null)
            {
                _logger.LogError($"Active with id: {id} not found!");
                return false;
            }

            var active = updateActiveViewModel.UpdateActive(activeVerify);

            try
            {
                _context.Entry(active).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured when tryning to update the user! err: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var active = await _context.Actives
                .FirstOrDefaultAsync(active => active.Active_Id.Equals(id));

            if (active is null)
            {
                _logger.LogError($"Active with id {id} not found");
                return false;
            }

            try
            {
                _context.Actives.Remove(active);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured when tryning to delete the user! err: {ex.Message}");
                return false;
            }
        }
        public async Task<ActiveReturn> SearchActiveAsync(string active)
        {
            try
            {
                var resultActive = await YahooFinanceApiClient.GetActive(active);
                
                if (resultActive is null)
                {
                    _logger.LogError("The active not found!");
                    return null;
                }

                return resultActive;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Houve um problema interno ao tentar buscar o ativo: {ex.Message}");
                return null;
            }
        }

        public async Task<Purse> GetActivesByPurseId(int purseId)
        {
            var actives = await _context.Purses
                .Include(p => p.Actives)
                .FirstOrDefaultAsync(p => p.Purse_Id == purseId);

            if (actives is null || !actives.Actives.Any())
            {
                _logger.LogError("No actives found for the given purse ID.");
                return null;
            }

            return actives;
        }
    }
}
