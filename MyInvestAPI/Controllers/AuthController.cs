using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyInvestAPI.Domain;
using MyInvestAPI.Repositories;
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
    private readonly IAuthRepository _authRepository;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(ITokenService tokenService,
                          RoleManager<IdentityRole> roleManager, IConfiguration configuration,
                          ILogger<AuthController> logger, IAuthRepository authRepository)
    {
        _tokenService = tokenService;
        _roleManager = roleManager;
        _configuration = configuration;
        _logger = logger;
        _authRepository = authRepository;
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequestViewModel request)
    {
        if (request is null)
        {
            BadRequest("O corpo da requisição não pode ser nulo");
        }

        var response = await _authRepository.Login(request!);

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<ActionResult> register(RegisterViewModel request)
    {
        var response = await _authRepository.register(request);
        return Created("/users/{userId}", response);
    }

    //[HttpPost("refresh-token")]
    //public async Task<ActionResult> RefreshToken(TokenViewModel tokenViewModel)
    //{
    //    if (tokenViewModel is null)
    //    {
    //        return BadRequest("O corpo da requisição não pode ser nula!");
    //    }

    //    string? accessToken = tokenViewModel.AccessToken ?? throw new ArgumentNullException(nameof(tokenViewModel));

    //    string? refreshToken = tokenViewModel.RefreshToken ?? throw new ArgumentNullException(nameof(tokenViewModel));

    //    var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken!, _configuration);

    //    if (principal is null)
    //    {
    //        return BadRequest("Access/Refresh token inválido");
    //    }

    //    var user = await _userManager.FindByNameAsync(principal.Identity.Name);

    //    if (user is null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
    //    {
    //        return BadRequest("Access/Refresh token inválido");
    //    }

    //    var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);
    //    var newRefreshToken = _tokenService.GenerateRefreshToken();

    //    user.RefreshToken = newRefreshToken;
    //    await _userManager.UpdateAsync(user);

    //    var response = new
    //    {
    //        accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
    //        refreshToken = newRefreshToken
    //    };

    //    return Ok(response);
    //}

    //[HttpPost("revoke/{username}")]
    //public async Task<ActionResult> Revoke(string username)
    //{
    //    var user = await _userManager.FindByNameAsync(username);

    //    if (user is null) return BadRequest("O nome do usuário é inválido ou não existe!");

    //    user.RefreshToken = null;

    //    await _userManager.UpdateAsync(user);
    //    return NoContent();
    //}
}
