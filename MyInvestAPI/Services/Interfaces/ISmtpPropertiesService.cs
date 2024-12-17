using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Services.Interfaces
{
    public interface ISmtpPropertiesService
    {
        Task<SmtpProperties> Create(CreateSmtpPropertiesViewModel data);
        Task Update(int id, UpdateSmtpPropertiesViewModel updateData);
        Task Delete(int smtpPropertiesId);
        Task<IEnumerable<SmtpProperties>> GetAll();
        Task<SmtpProperties> GetById(int smtpId);
    }
}
 