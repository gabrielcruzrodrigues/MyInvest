using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Domain;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.Services.Interfaces;
using MyInvestAPI.ViewModels;
using System.Security.Claims;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService _repository;

        public UserController(IUserService userService)
        {
            _repository = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _repository.GetAllUsersAsync();
        }

        [Authorize]
        [HttpGet("purses")]
        public async Task<IEnumerable<User>> GetAllUsersWithPurses()
        {
            return await _repository.GetAllUsersWithPursesAsync();
        }

        [Authorize]
        [HttpGet("purses/actives")]
        public async Task<IEnumerable<User>> GetAllUsersWithPursesAndActives()
        {
            return await _repository.GetAllUsersWithPursesAndActivesAsync();
        }

        [Authorize]
        [HttpGet("{userId}", Name ="GetUser")]
        public async Task<ActionResult<User>> GetById(string userId)
        {
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Você não pode buscar outro usuário por id."});
            }

            return Ok(await _repository.GetByIdAsync(userId));
        }

        [Authorize]
        [HttpGet("{userId}/purses")]
        public async Task<ActionResult<User>> GetUserWithAllPursesById(string userId)
        {
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Você não pode buscar outro usuário por id." });
            }

            return Ok(await _repository.GetUserWithAllPursesByIdAsync(userId));
        }

        [Authorize]
        [HttpGet("{userId}/purses/actives")]
        public async Task<ActionResult<User>> GetUserWithAllPursesAndActivesById(string userId)
        {
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Você não pode buscar outro usuário por id." });
            }

            return Ok(await _repository.GetUserWithAllPursesAndActivesByIdAsync(userId));
        }

        [Authorize]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateAsync(string userId, CreateUserViewModel userViewModel)
        {
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Você não pode atualizar outro usuário que não seja você." });
            }

            if (userViewModel is null)
                return BadRequest("The data for update must not be null.");

            await _repository.Update(userId, userViewModel);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != userId)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Você não pode deletar outro usuário que não seja você." });
            }

            await _repository.Delete(userId);
            return NoContent();
        }
    }
}
