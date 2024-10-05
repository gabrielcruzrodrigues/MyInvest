using Newtonsoft.Json.Linq;

namespace MyInvestAPI.ViewModels.Auth
{
    public class ResponseLoginViewModel
    {
        public string? UserId { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime Expiration { get; set; }

        public ResponseLoginViewModel(string userId, string token, string refreshToken, DateTime expiration)
        {
            UserId = userId;
            Token = token;
            RefreshToken = refreshToken;
            Expiration = expiration;
        }
    }
}
