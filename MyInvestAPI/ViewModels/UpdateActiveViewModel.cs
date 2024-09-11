using MyInvestAPI.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels
{
    public class UpdateActiveViewModel
    {
        public string? Description { get; set; }
        public string? Type { get; set; }
        public float DyDesiredPercentage { get; set; }

        public Active UpdateActive(Active active)
        {
            active.Type = this.Type;
            active.LastUpdatedAt = DateTime.UtcNow;
            active.DYDesiredPercentage = DyDesiredPercentage;
            return active;
        }
    }

}
