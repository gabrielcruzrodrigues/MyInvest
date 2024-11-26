using Microsoft.IdentityModel.Tokens;
using MyInvestAPI.Domain;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MyInvestAPI.Services
{
    public class TokenService : ITokenService
    {
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;

        public TokenService(IPasswordResetTokenRepository passwordResetTokenRepository)
        {
            _passwordResetTokenRepository = passwordResetTokenRepository;
        }

        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration _config)
        {
            var key = _config.GetSection("JWT").GetValue<string>("SecretKey") ?? 
                throw new InvalidOperationException("Senha secreta inválida");

            var privateKey = Encoding.UTF8.GetBytes(key);

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_config.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),
                Audience = _config.GetSection("JWT").GetValue<string>("ValidAudience"),
                Issuer = _config.GetSection("JWT").GetValue<string>("ValidIssuer"),
                SigningCredentials = signingCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            return token;
        }

        private string GeneratePasswordResetToken()
        {
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        public async Task<string> GenerateAndReturnPasswordResetLinkAsync(User user)
        {
            var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");
            if (string.IsNullOrEmpty(frontendUrl))
            {
                throw new Exception("FRONTEND_URL não configurado no ambiente.");
            }

            string tokenForSave;
            do
            {
                tokenForSave = GeneratePasswordResetToken();
            }
            while (await _passwordResetTokenRepository.GetByTokenAsync(tokenForSave) is not null);

            var passwordResetTokenForSave = new PasswordResetToken()
            {
                UserId = user.Id,
                Token = tokenForSave,
                ExpirationTime = DateTime.UtcNow.AddMinutes(30),
            };

            var generatedPasswordResetToken = await _passwordResetTokenRepository.CreateAsync(passwordResetTokenForSave);

            return $"{frontendUrl}/reset-password?email={Uri.EscapeDataString(user.Email)}&token={generatedPasswordResetToken.Token}";
        }

        public string GenerateRefreshToken()
        {
            var secureRandomBytes = new byte[128];

            using var randomNumberGenerator = RandomNumberGenerator.Create();

            randomNumberGenerator.GetBytes(secureRandomBytes);

            var refreshToken = Convert.ToBase64String(secureRandomBytes);
            return refreshToken;
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration _config)
        {
            var secretKey = _config["JWT:SecretKey"] ?? throw new InvalidOperationException("Chave secreta inválida");

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Token inválido!");
            }

            return principal;
        }

        public async Task DeleteResetTokenPasswordAsync(string token)
        {
            try
            {
                
            }
            catch
            {

            }
        }
    }
}
