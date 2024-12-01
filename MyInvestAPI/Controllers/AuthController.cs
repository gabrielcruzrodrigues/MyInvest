using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyInvestAPI.Domain;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.Services.Interfaces;
using MyInvestAPI.ViewModels;
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

    [HttpPost("request-recover-password")]
    public async Task<ActionResult> RequestRecoverPassword(string userId)
    {
        await _authRepository.RequestRecoverPassword(userId);
        return Ok();
    }

    [HttpPost("recover-password")]
    public async Task<ActionResult> RecoverPassword(RecoverPasswordViewModel request)
    {
        await _authRepository.RecoverPassword(request);
        return Ok();
    }

    [HttpPost("request-login-by-code")]
    public async Task<ActionResult> RequestLoginByCode(RequestLoginByCodeViewModel request)
    {
        await _authRepository.RequestCodeForForgottenPassword(request.UserEmail);
        return Ok();
    }

    [HttpPost("login-by-code")]
    public async Task<ActionResult<ResponseLoginViewModel>> LoginByCode(LoginByCodeViewModel request)
    {
        return Ok(await _tokenService.VerifyPasswordResetCode(request.Code));
    }
}
