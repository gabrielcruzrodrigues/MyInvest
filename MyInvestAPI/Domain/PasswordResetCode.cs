using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyInvestAPI.Domain
{
    public class PasswordResetCode
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(10)]
        public required string Code { get; set; }
        [Required]
        [StringLength(50)]
        public required string UserId { get; set; }
        [Required]
        public DateTime ExpirationTime { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
