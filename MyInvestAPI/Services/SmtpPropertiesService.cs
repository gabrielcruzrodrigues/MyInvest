using MyInvestAPI.Domain;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.Services.Interfaces;
using MyInvestAPI.ViewModels;
using MyInvestAPI.Extensions;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Services
{
    public class SmtpPropertiesService : ISmtpPropertiesService
    {
        private readonly ISmtpPropertiesRepository _repository;

        public SmtpPropertiesService(ISmtpPropertiesRepository repository)
        {
            _repository = repository;
        }

        public async Task<SmtpProperties> Create(CreateSmtpPropertiesViewModel request)
        {
            SmtpProperties properties = request.CreateNewSmtpProperties();
            return await _repository.Create(properties);
        }

        public async Task Delete(int smtpPropertiesId)
        {
            if (smtpPropertiesId <= 0)
            {
                throw new HttpResponseException(400, "O id não pode ser menor ou igual a zero");
            }

            await _repository.Delete(smtpPropertiesId);
        }

        public async Task<IEnumerable<SmtpProperties>> GetAll()
        {
            return await _repository.GetAll();
        }

        public async Task<SmtpProperties> GetById(int smtpId)
        {
            return await _repository.GetById(smtpId);
        }

        public async Task Update(int id, UpdateSmtpPropertiesViewModel updateData)
        {
            await _repository.Update(id, updateData);
        }
    }
}
