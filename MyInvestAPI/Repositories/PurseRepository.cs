using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
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

    public async Task<Purse> CreateAsync(CreatePurseViewModel purseViewModel)
    {
        var userVerify = await _context.Users.FirstOrDefaultAsync(u => u.User_Id == purseViewModel.User_Id);
        if (userVerify is null)
            return null;

        Purse purse = purseViewModel.CreatePurse();

        try
        {
            await _context.Purses.AddAsync(purse);
            await _context.SaveChangesAsync();
            return purse;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occured when tryning create user! ex: {ex.Message}");
            return null;
        }
    }


    public async Task<IEnumerable<Purse>> GetAllAsync()
    {
        return await _context.Purses
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Purse>> GetAllWithActivesAsync()
    {
        return await _context.Purses
            .AsNoTracking()
            .Include(p => p.Actives)
            .ToListAsync();
    }

    public async Task<Purse> GetByIdAsync(int id)
    {
        return await _context.Purses
                            .AsNoTracking()
                            .FirstOrDefaultAsync(p => p.Purse_Id == id);
    }

    public async Task<Purse> GetByIdWithActivesAsync(int id)
    {
        return await _context.Purses
            .Include(p => p.Actives)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Purse_Id == id);
    }

    public async Task<bool> UpdateAsync(int id, UpdatePurseViewModel updatePurseViewModel)
    {
        var purseVerify = await _context.Purses.FirstOrDefaultAsync(p => p.Purse_Id == id);

        if (purseVerify is null)
            return false;

        Purse purse = updatePurseViewModel.UpdatePurse(purseVerify);

        try
        {
            _context.Entry(purse).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An Erro occured when tryning update purse! err: {ex.Message}");
            return false;
        }

    }
    public async Task<bool> DeleteAsync(int id)
    {
        var purse = await _context.Purses.FirstOrDefaultAsync(p => p.Purse_Id == id);

        if (purse is null)
            return false;

        try
        {
            _context.Purses.Remove(purse);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An Erro occured when tryning delete purse! err: {ex.Message}");
            return false;
        }
    }
}
