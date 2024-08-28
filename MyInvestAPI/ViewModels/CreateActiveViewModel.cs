using MyInvestAPI.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels
{
    public class CreateActiveViewModel
    {
        public string? Description { get; set; }
        public TypeEnum type { get; set; }
        public int Purse_Id { get; set; }

        public Active CreateActive(Purse purse)
        {
            Active active = new();
            active.Code = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
            active.Description = this.Description;
            active.type = this.type;
            active.CreatedAt = DateTime.UtcNow;
            active.LastUpdatedAt = DateTime.UtcNow;
            active.Purses = new List<Purse>() { purse };
            return active;
        }
    }
    
}
