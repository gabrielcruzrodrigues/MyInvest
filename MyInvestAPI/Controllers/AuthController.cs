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
    public async Task<ActionResult> Register(RegisterViewModel request)
    {
        var response = await _authRepository.Register(request);
        return Created("/users/{userId}", response);
    }

    [HttpPost("new-access-token")]
    public async Task<ActionResult> GetNewAccessToken(TokenViewModel tokenViewModel)
    {
        if (tokenViewModel is null)
        {
            return BadRequest("O corpo da requisição não pode ser nula!");
        }

        var response = await _authRepository.GetNewTokenUsingRefreshToken(tokenViewModel);
        return Ok(response);
    }
}
