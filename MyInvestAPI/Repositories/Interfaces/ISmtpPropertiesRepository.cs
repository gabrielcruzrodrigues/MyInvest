using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories.Interfaces
{
    public interface ISmtpPropertiesRepository
    {
        Task<SmtpProperties> Create(SmtpProperties data);
        Task Update(int id, UpdateSmtpPropertiesViewModel updateData);
        Task Delete(int SmtpPropertiesId);
        Task<IEnumerable<SmtpProperties>> GetAll();
        Task<SmtpProperties> GetById(int smtpId);
    }
}
