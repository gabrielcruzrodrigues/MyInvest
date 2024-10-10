using MyInvestAPI.Domain;
using System.ComponentModel.DataAnnotations;

namespace MyInvestAPI.ViewModels
{
    public class CreateUserViewModel
    {
        public string? Name { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public User CreateUser()
        {
            return new User(username: Name, password: Password, email: Email, phone: Phone);
        }

        public User UpdateUser(User user)
        {
            user.UserName = this.Name;
            user.Email = this.Email;
            user.PhoneNumber = this.Phone;
            user.LastUpdatedAt = DateTime.UtcNow;
            return user;
        }
    }
}
