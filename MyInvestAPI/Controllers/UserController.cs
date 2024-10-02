using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Domain;
using MyInvestAPI.Repositories;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserRepository _repository;

        public UserController(IUserRepository IUserRepository)
        {
            _repository = IUserRepository;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateUserViewModel userViewModel)
        {
            if (userViewModel is null)
                return BadRequest("The User body must not be null.");

            var userCreated = await _repository.CreateAsync(userViewModel);
            return new CreatedAtRouteResult("GetUser", new { id = userCreated.User_Id }, userCreated);
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
        [HttpGet("{id:int}", Name ="GetUser")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            return Ok(await _repository.GetByIdAsync(id));
        }

        [Authorize]
        [HttpGet("{id:int}/purses")]
        public async Task<ActionResult<User>> GetUserWithAllPursesById(int id)
        {
            return Ok(await _repository.GetUserWithAllPursesByIdAsync(id));
        }

        [Authorize]
        [HttpGet("{id:int}/purses/actives")]
        public async Task<ActionResult<User>> GetUserWithAllPursesAndActivesById(int id)
        {
            return Ok(await _repository.GetUserWithAllPursesAndActivesByIdAsync(id));
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CreateUserViewModel userViewModel)
        {
            if (userViewModel is null)
                return BadRequest("The data for update must not be null.");

            _repository.Update(id, userViewModel);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }
    }
}
