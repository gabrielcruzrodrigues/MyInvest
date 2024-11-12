using MyInvestAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyInvestAPI.Domain
{
    public class Purse
    {
        [Key]
        public int Purse_Id { get; set; }
        [Required]
        public string? Name { get; set; }

        [Required]
        [StringLength(300)]
        public string? Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        [Required]
        public ActiveEnum Enable { get; set; }

        [ForeignKey("User")]
        public string User_Id { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        public ICollection<Active> Actives { get; set; } = new List<Active>();

        public Purse()
        { }

        public Purse(string name, string description, string user_id)
        {
            Description = description;
            Name = name;
            CreatedAt = DateTime.UtcNow;
            LastUpdatedAt = DateTime.UtcNow;
            User_Id = user_id;
            Actives = new List<Active>();
            Enable = ActiveEnum.ACTIVE;
        }
    }
}
