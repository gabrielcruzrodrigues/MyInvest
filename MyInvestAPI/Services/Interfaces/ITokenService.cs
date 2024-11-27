using MyInvestAPI.Domain;
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
        Task<string> GenerateAndReturnPasswordResetLinkAsync(User user);
        Task<string> GenerateSaveAndReturnRecoverCode(User user);
    }
}
