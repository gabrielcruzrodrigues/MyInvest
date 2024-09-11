using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Api;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Repositories;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActiveController : ControllerBase
    {
        public readonly IActiveRepository _repository;

        public ActiveController(IActiveRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<Action>> Create(CreateActiveViewModel activeViewModel)
        {
            if (activeViewModel is null)
                return BadRequest("The body for create a new active must not be null.");

            Active activeCreated = await _repository.CreateAsync(activeViewModel);
            
            if (activeCreated is null)
                return BadRequest("An error occured when tryning to create the user");

            return new CreatedAtRouteResult("SearchActive", new { id = activeCreated.Active_Id }, activeCreated);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Active>>> GetAll()
        {
            var result = await _repository.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("purses")]
        public async Task<ActionResult<IEnumerable<Active>>> GetAllWithPurses()
        {
            var result = await _repository.GetAllWithPursesAsync();
            return Ok(result);
        }

        [HttpGet("{id}", Name ="SearchActive")]
        public async Task<ActionResult<Active>> GetById(int id)
        {
            var ActiveVerify = await _repository.GetByIdAsync(id);

            if (ActiveVerify is null)
                return NotFound("Active not found.");

            return Ok(ActiveVerify);
        }

        [HttpGet("{id}/purses")]
        public async Task<ActionResult<Active>> GetByIdWithPurses(int id)
        {
            var ActiveVerify = await _repository.GetByIdWithPursesAsync(id);

            if (ActiveVerify is null)
                return NotFound("Active not found.");

            return Ok(ActiveVerify);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateActiveViewModel activeViewModel)
        {
            var result = await _repository.UpdateAsync(id, activeViewModel);

            if (!result)
                return BadRequest("An error occured when tryning to update the user");

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);

            if (!result)
                return BadRequest("An error occured when tryning to delete the user");

            return NoContent();
        }

        [HttpGet("/search-active/{active}")]
        public async Task<ActionResult<ActiveReturn>> SearchActive(string active)
        {
            var result = await _repository.SearchActiveAsync(active);

            if (result is null)
                return StatusCode(500, "Un error occured when tryning search actives");

            return Ok(result);
        }

        [HttpGet("/get-actives/{purseId}")]
        public async Task<ActionResult> GetActivesByPurseId(int purseId)
        {
            var result = await _repository.GetActivesByPurseId(purseId);

            if (result is null)
                return StatusCode(500, "Un error occured when tryning search actives");

            return Ok(result);
        }
    }
}
