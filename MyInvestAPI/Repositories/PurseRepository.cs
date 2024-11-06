using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Domain.Enums;
using MyInvestAPI.Extensions;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories;

public class PurseRepository : IPurseRepository
{
    public readonly MyInvestContext _context;
    public readonly ILogger<PurseRepository> _logger;

    public PurseRepository(MyInvestContext context, ILogger<PurseRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Purse> CreateAsync(Purse purse)
    {
        try
        {
            await _context.Purses.AddAsync(purse);
            await _context.SaveChangesAsync();
            return purse;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured when tryning create user! ex: {ex.Message}");
            throw new HttpResponseException(500, "An error occured when tryning create purse");
        }
    }


    public async Task<IEnumerable<Purse>> GetAllAsync()
    {
        return await _context.Purses
            .AsNoTracking()
            .Where(p => p.Active == ActiveEnum.ACTIVE)
            .ToListAsync();
    }

    public async Task<IEnumerable<Purse>> GetAllWithActivesAsync()
    {
        return await _context.Purses
            .AsNoTracking()
            .Where(p => p.Active == ActiveEnum.ACTIVE)
            .Include(p => p.Actives)
            .ToListAsync();
    }

    public async Task<Purse> GetByIdAsync(int id)
    {
        var purse = await _context.Purses
                            .AsNoTracking()
                            .Where(p => p.Active == ActiveEnum.ACTIVE)
                            .FirstOrDefaultAsync(p => p.Purse_Id == id);

        if (purse is null)
            throw new HttpResponseException(404, $"The purse with id {id} not found!");

        return purse;
    }

    public async Task<Purse> GetByIdWithActivesAsync(int id)
    {
        var purse = await _context.Purses
            .Include(p => p.Actives)
            .AsNoTracking()
            .Where(p => p.Active == ActiveEnum.ACTIVE)
            .FirstOrDefaultAsync(p => p.Purse_Id == id);

        if (purse is null)
            throw new HttpResponseException(404, $"The purse with id {id} not found!");

        return purse;
    }

    public async Task Update(Purse purse)
    {
        try
        {
            _context.Entry(purse).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"An Erro occured when tryning update purse! err: {ex.Message}");
            throw new HttpResponseException(500, "An Erro occured when tryning update purse!");
        }
    }

    public async Task Disable(int id)
    {
        try
        {
            var purse = await GetByIdAsync(id);
            purse.Active = Domain.Enums.ActiveEnum.DISABLE;
            _context.Entry(purse).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"An Erro occured when tryning delete purse! err: {ex.Message}");
            throw new HttpResponseException(500, "An Erro occured when tryning delete purse!");
        }
    }
}
