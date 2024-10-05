using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyInvestAPI.Domain;
using MyInvestAPI.Services;
using MyInvestAPI.ViewModels.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MyInvestAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService, UserManager<User> userManager,
                          RoleManager<IdentityRole> roleManager, IConfiguration configuration,
                          ILogger<AuthController> logger)
    {
        _tokenService = tokenService;
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginViewModel request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);

        if (user is null || !await _userManager.CheckPasswordAsync(user, request.Password!))
        {
            return Unauthorized();
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

        await _userManager.UpdateAsync(user);

        return Ok(new
        {
            userId = user.Id,
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken,
            Expiration = token.ValidTo
        });
    }

    [HttpPost("register")]
    public async Task<ActionResult> register(RegisterViewModel request)
    {
        var EmailVerify = await _userManager.FindByEmailAsync(request.Email!);

        if (EmailVerify is not null)
        {
            return BadRequest(new {message = "Email já cadastrado no banco de dados" });
        }

        User user = new()
        {
            Email = request.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = request.Username,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = DateTime.UtcNow,
            Purses = new List<Purse>()
        };

        var result = await _userManager.CreateAsync(user, request.Password!);

        if (!result.Succeeded)
        {
            _logger.LogError($"A criação do usuário falhou! - err: {result.Errors}");
            return BadRequest(new { message = result.Errors.First().Description});
        }

        return Created("/users/{userId}", new { message = "Usuário criado com sucesso!" });
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult> RefreshToken(TokenViewModel tokenViewModel)
    {
        if (tokenViewModel is null)
        {
            return BadRequest("O corpo da requisição não pode ser nula!");
        }

        string? accessToken = tokenViewModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenViewModel));

        string? refreshToken = tokenViewModel.RefreshToken ?? throw new ArgumentNullException(nameof(tokenViewModel));

        var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

        if (principal is null)
        {
            return BadRequest("Access/Refresh token inválido");
        }

        var user = await _userManager.FindByNameAsync(principal.Identity.Name);

        if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            return BadRequest("Access/Refresh token inválido");
        }

        var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _userManager.UpdateAsync(user);

        var response = new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            refreshToken = newRefreshToken
        };

        return Ok(response);
    }

    [HttpPost("revoke/{username}")]
    public async Task<ActionResult> Revoke(string username)
    {
        var user = await _userManager.FindByNameAsync(username);

        if (user is null) return BadRequest("O nome do usuário é inválido ou não existe!");

        user.RefreshToken = null;

        await _userManager.UpdateAsync(user);
        return NoContent();
    }
}
