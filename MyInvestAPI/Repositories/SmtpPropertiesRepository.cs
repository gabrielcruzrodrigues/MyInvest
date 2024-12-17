using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Extensions;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories
{
    public class SmtpPropertiesRepository : ISmtpPropertiesRepository
    {
        private readonly MyInvestContext _context;
        private readonly ILogger<SmtpPropertiesRepository> _logger;

        public SmtpPropertiesRepository(MyInvestContext context, ILogger<SmtpPropertiesRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<SmtpProperties> Create(SmtpProperties data)
        {
            try
            {
                await _context.SmtpProperties.AddAsync(data);
                await _context.SaveChangesAsync();
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Ocorreu um erro ao tentar criar um perfil smtp! err: {ex.Message}");
                throw new HttpResponseException(500, "Ocorreu um erro ao tentar criar um perfil smtp!");
            }
        }

        public async Task Delete(int smtpPropertiesId)
        {
            SmtpProperties smptProperties = await GetById(smtpPropertiesId);
            try
            {
                smptProperties.Active = false;
                _context.Entry(smptProperties).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Ocorreu um erro ao tentar deletar o SmtpPropertie! err: {ex.Message}");
                throw new HttpResponseException(500, "Aconteceu um erro ao tentar deletar o SmtpPropertie");
            }
        }

        public async Task<IEnumerable<SmtpProperties>> GetAll()
        {
            return await _context.SmtpProperties
                .AsNoTracking()
                .Where(s => s.Active == true)
                .ToListAsync();
        }

        public async Task<SmtpProperties> GetById(int smtpId)
        {
            var SmtpProperties = await _context.SmtpProperties
                .Where(s => s.Id.Equals(smtpId))
                .Where(s => s.Active.Equals(true))
                .FirstOrDefaultAsync();

            if (SmtpProperties is null)
            {
                throw new HttpResponseException(404, "SmtpPropertie não encontrado!");
            }

            return SmtpProperties;
        }

        public async Task Update(int id, UpdateSmtpPropertiesViewModel updateData)
        {
            SmtpProperties smtpProperties = await GetById(id);
            SmtpProperties smptPropertiesUpdated = updateData.UpdateSmtpProperties(smtpProperties);

            try
            {
                _context.Entry(smptPropertiesUpdated).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Ocorreu um erro ao tentar atualizar um perfil smtp! err: {ex.Message}");
                throw new HttpResponseException(500, "Ocorreu um erro ao tentar atualizar um perfil smtp!");
            }
        }
    }
}
