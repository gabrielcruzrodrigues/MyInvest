using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.Domain
{
    public class Active
    {
        [Key]
        public int Active_Id { get; set; }
        public string? Code { get; set; }
        public string? Description { get; set; }
        public TypeEnum type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public ICollection<Purse> Purses { get; set; } = new List<Purse>();
    } 
}
