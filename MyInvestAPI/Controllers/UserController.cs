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

            User user = userViewModel.CreateUser();

            try
            {
                var userCreated = await _repository.CreateAsync(user);
                return new CreatedAtRouteResult("GetUser", new { id = userCreated.User_Id }, userCreated);
            }
            catch(Exception)
            {
                return StatusCode(500, "An error occured when tryning to create the user");
            }
        }

        [HttpGet]
        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _repository.GetAllUsersAsync();
        }

        [HttpGet("purses")]
        public async Task<IEnumerable<User>> GetAllUsersWithPurses()
        {
            return await _repository.GetAllUsersWithPursesAsync();
        }

        [HttpGet("purses/actives")]
        public async Task<IEnumerable<User>> GetAllUsersWithPursesAndActives()
        {
            return await _repository.GetAllUsersWithPursesAndActivesAsync();
        }

        [HttpGet("{id:int}", Name ="GetUser")]
        public async Task<ActionResult<User>> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("The ID is not valid for search!");

            var user = await _repository.GetByIdAsync(id);

            if (user is null)
                return BadRequest("User not found.");

            return Ok(user);
        }

        [HttpGet("{id:int}/purses")]
        public async Task<ActionResult<User>> GetUserWithAllPursesById(int id)
        {
            if (id <= 0)
                return BadRequest("The ID is not valid for search!");

            var user = await _repository.GetUserWithAllPursesByIdAsync(id);

            if (user is null)
                return NotFound("User not found.");

            return Ok(user);
        }

        [HttpGet("{id:int}/purses/actives")]
        public async Task<ActionResult<User>> GetUserWithAllPursesAndActivesById(int id)
        {
            if (id <= 0)
                return BadRequest("The ID is not valid for search!");

            var user = await _repository.GetUserWithAllPursesAndActivesByIdAsync(id);

            if (user is null)
                return NotFound("User not found.");

            return Ok(user);
        }


        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CreateUserViewModel userViewModel)
        {
            if (userViewModel is null)
                return BadRequest("The data for update must not be null.");

            bool result = await _repository.UpdateAsync(id, userViewModel);
            
            if (result)
            {
                return NoContent();
            }
            else
            {
                return BadRequest("An error occured when tryning to update the user");  
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);

            if (!result)
                return BadRequest("An error occured when tryning to delete the user");

            return NoContent();
        }
    }
}
