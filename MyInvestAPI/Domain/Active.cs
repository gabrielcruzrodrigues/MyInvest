using MyInvestAPI.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyInvestAPI.Domain
{
    public class Active
    {
        [Key]
        public int Active_Id { get; set; }

        [Required]
        public string? Code { get; set; }

        [Required]
        public string? Type { get; set; }

        [Required]
        public float DYDesiredPercentage { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }
        [Required]
        public ActiveEnum Enable { get; set; }
        [Required]
        public int PurseId { get; set; }

        public Purse Purse { get; set; }
    } 
}
