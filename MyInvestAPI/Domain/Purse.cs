using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyInvestAPI.Domain
{
    public class Purse
    {
        [Key]
        public int Purse_Id { get; set; }

        [Required]
        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public int User_Id { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public ICollection<Active> Actives { get; set; } = new List<Active>();
    }
}
