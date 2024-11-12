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
            _logger.LogError($"Um erro ocorreu ao tentar criar a carteira! ex: {ex.Message}");
            throw new HttpResponseException(500, "Um erro ocorreu ao tentar criar a carteira!");
        }
    }


    public async Task<IEnumerable<Purse>> GetAllAsync()
    {
        return await _context.Purses
            .AsNoTracking()
            .Where(p => p.Enable == ActiveEnum.ACTIVE)
            .ToListAsync();
    }

    public async Task<IEnumerable<Purse>> GetAllWithActivesAsync()
    {
        return await _context.Purses
            .AsNoTracking()
            .Where(p => p.Enable == ActiveEnum.ACTIVE)
            .Include(p => p.Actives)
            .ToListAsync();
    }

    public async Task<Purse> GetByIdAsync(int id)
    {
        var purse = await _context.Purses
                            .AsNoTracking()
                            .Where(p => p.Enable == ActiveEnum.ACTIVE)
                            .FirstOrDefaultAsync(p => p.Purse_Id == id);

        if (purse is null)
            throw new HttpResponseException(404, $"A carteira com o id {id} não foi encontrada!");

        return purse;
    }

    public async Task<Purse> GetByIdWithActivesAsync(int id)
    {
        var purse = await _context.Purses
            .Include(p => p.Actives)
            .AsNoTracking()
            .Where(p => p.Enable == ActiveEnum.ACTIVE)
            .FirstOrDefaultAsync(p => p.Purse_Id == id);

        if (purse is null)
            throw new HttpResponseException(404, $"A carteira com o id {id} não foi encontrada!");

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
            _logger.LogError($"Um erro ocorreu ao tentar atualizar a carteira! err: {ex.Message}");
            throw new HttpResponseException(500, "Um erro ocorreu ao tentar atualizar a carteira!!");
        }
    }

    public async Task Disable(int id)
    {
        try
        {
            var purse = await GetByIdAsync(id);
            purse.Enable = Domain.Enums.ActiveEnum.DISABLE;
            _context.Entry(purse).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Um erro ocorreu ao tentar desabilitar a carteira! err: {ex.Message}");
            throw new HttpResponseException(500, "Um erro ocorreu ao tentar desabilitar a carteira!!");
        }
    }
}
