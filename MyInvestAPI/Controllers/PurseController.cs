using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Repositories;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurseController : ControllerBase
    {
        public readonly IPurseRepository _repository;

        public PurseController(IPurseRepository IPurseRepository)
        {
            _repository = IPurseRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Purse>> Create(CreatePurseViewModel purseViewModel)
        {
            if (purseViewModel is null)
                return BadRequest("The body for create purse must not be null.");

            var purseCreated = await _repository.CreateAsync(purseViewModel);

            if (purseCreated is null)
                return BadRequest("User not found!");
                
            return new CreatedAtRouteResult("GetPurse", new { id = purseCreated.Purse_Id }, purseCreated);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purse>>> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("actives")]
        public async Task<ActionResult<IEnumerable<Purse>>> GetAllWithActives()
        {
            var result = _repository.GetAllWithActivesAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}", Name = "GetPurse")]
        public async Task<ActionResult<Purse>> getById(int id)
        {
            if (id <= 0)
                return BadRequest("The ID is invalid for search!");

            var purse = await _repository.GetByIdAsync(id);

            if (purse is null)
                return NotFound($"Purse with id {id} not found.");

            return Ok(purse);
        }

        [HttpGet("{id:int}/actives")]
        public async Task<ActionResult<Purse>> getByIdWithActives(int id)
        {
            if (id <= 0)
                return BadRequest("The ID is invalid for search!");

            var purse = await _repository.GetByIdWithActivesAsync(id);

            if (purse is null)
                return NotFound($"Purse with id {id} not found.");

            return Ok(purse);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UpdatePurseViewModel purseViewModel)
        {
            if (purseViewModel is null)
                return BadRequest("The body for update purse must not be null.");

            var result = await _repository.UpdateAsync(id, purseViewModel);

            if (result)
                return BadRequest("An error occured when tryning to update the purse.");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);

            if (result is false)
                return StatusCode(500, "An error occured when tryning to delete the user");

            return NoContent();
        }
    }
}
