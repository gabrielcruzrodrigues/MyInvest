using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyInvestAPI.Domain
{
    [Table("Users")]
    public class User : IdentityUser
    {
        [Required]
        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedAt { get; set; }

        public ICollection<Purse>? Purses { get; set; }

        public User()
        { }

        public User(string username, string password, string email, string phone)
        {
            this.Email = email;
            this.CreatedAt = DateTime.UtcNow;
            this.LastUpdatedAt = DateTime.UtcNow;
            this.Purses = new List<Purse>();
        }
    }
}
