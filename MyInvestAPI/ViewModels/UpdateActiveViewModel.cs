using MyInvestAPI.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels
{
    public class UpdateActiveViewModel
    {
        public string? Description { get; set; }
        public TypeEnum type { get; set; }

        public Active UpdateActive(Active active)
        {
            active.Description = this.Description;
            active.type = this.type;
            active.LastUpdatedAt = DateTime.UtcNow;
            return active;
        }
    }

}
