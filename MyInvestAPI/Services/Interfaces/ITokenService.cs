using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyInvestAPI.Services.Interfaces
{
    public interface ITokenService
    {
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config);
        string GeneratePasswordResetToken();
        string GeneratePasswordResetLink(string userEmail, string token);
        Task SaveResetTokenToDatabase(string userEmail, string token);
    }
}
