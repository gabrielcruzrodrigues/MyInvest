using Microsoft.AspNetCore.Identity;
using MyInvestAPI.Domain;
using MyInvestAPI.ViewModels.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using MyInvestAPI.Extensions;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.Services.Interfaces;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IEmailSender _emailSender;
        private readonly IPasswordResetTokenRepository _passwordResetTokenRepository;

        public AuthRepository(ITokenService tokenService, UserManager<User> userManager,
                              RoleManager<IdentityRole> roleManager, IConfiguration configuration,
                              ILogger<AuthRepository> logger, IEmailSender emailSender, 
                              IPasswordResetTokenRepository passwordResetTokenRepository)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
            _emailSender = emailSender;
            _passwordResetTokenRepository = passwordResetTokenRepository;
        }

        public async Task<ResponseLoginViewModel> Login(LoginRequestViewModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email!);

            if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password!))
            {
                throw new HttpResponseException(401, "Credenciais incorretas!");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

            var refreshToken = _tokenService.GenerateRefreshToken();

            _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(refreshTokenValidityInMinutes);

            try
            {
                await _userManager.UpdateAsync(user);
            }
            catch(Exception ex)
            {
                throw new HttpResponseException(400, ex.Message);
            }

            return new ResponseLoginViewModel
            (
                user.Id,
                new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken,
                token.ValidTo
            );
        }

        public async Task<ResponseLoginViewModel> Register(RegisterViewModel request)
        {
            var EmailVerify = await _userManager.FindByEmailAsync(request.Email!);

            if (EmailVerify is not null)
            {
                throw new HttpResponseException(400, "Usuário já cadastrado no banco de dados!");
            }

            User user = new()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Username,
                CreatedAt = DateTime.UtcNow,
                LastUpdatedAt = DateTime.UtcNow,
                Purses = new List<Purse>(),
                Enable = Domain.Enums.ActiveEnum.ACTIVE
            };

            var result = await _userManager.CreateAsync(user, request.Password!);

            if (!result.Succeeded)
            {
                _logger.LogError($"A criação do usuário falhou! - err: {result.Errors}");
                throw new HttpResponseException(400, result.Errors.First().Description);
            }

            var credentials = new LoginRequestViewModel(request.Email!, request.Password!);
            return await Login(credentials);
        }

        public async Task<object> GetNewTokenUsingRefreshToken(TokenViewModel tokenViewModel)
        {
            string? accessToken = tokenViewModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenViewModel));

            string? refreshToken = tokenViewModel.RefreshToken ?? throw new ArgumentNullException(nameof(tokenViewModel));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

            if (principal is null)
            {
                throw new HttpResponseException(400, "Access/Refresh token inválido");
            }

            var user = await _userManager.FindByNameAsync(principal.Identity.Name);

            if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new HttpResponseException(400, "Access/Refresh token inválido");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return new
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken)
            };
        }

        public async Task RequestRecoverPassword(string userEmail)
        {
            var completeLink = await SaveTokenAndPrepareMessageForSendToUser(userEmail);
            string toEmail = userEmail;
            string subject = "MyInvest: Email de recuperação de senha";
            string message = $"Siga o link abaixo para recuperar a sua conta: {completeLink}";
            await _emailSender.SendEmailAsync(toEmail, subject, message);
        }

        public async Task RecoverPassword(RecoverPasswordViewModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.UserEmail);
            if (user is null)
            {
                throw new HttpResponseException(404, "Usuário não encontrado!");
            }

            var tokenVerify = await _passwordResetTokenRepository.GetByTokenAsync(request.Token);
            if (tokenVerify is null)
            {
                throw new HttpResponseException(404, "Token inválido ou inexistente!");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (string.IsNullOrEmpty(resetToken))
            {
                throw new HttpResponseException(400, "Falha ao gerar o token de redefinição de senha.");
            }

            var removePasswordResult = await _userManager.ResetPasswordAsync(user, resetToken, request.NewPassword);
            if (!removePasswordResult.Succeeded)
            {
                _logger.LogError("Erro ao tentar atualizar a senha do usuário!", removePasswordResult.Errors);
                throw new HttpResponseException(400, $"Erro ao tentar atualizar a senha do usuário! Ex: {removePasswordResult.Errors.First().Description}");
            }

            await _passwordResetTokenRepository.DeleteResetTokenPasswordAsync(request.Token);
        }

        public async Task<string> SaveTokenAndPrepareMessageForSendToUser(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user is null)
            {
                throw new HttpResponseException(404, "Usuário não encontrado!");
            }

            return await _tokenService.GenerateAndReturnPasswordResetLinkAsync(user);
        }
    }
}
