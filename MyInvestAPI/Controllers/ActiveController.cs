using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Api;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Domain.DTO;
using MyInvestAPI.Repositories.Interfaces;
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
        [Authorize]
        public async Task<ActionResult<Active>> Create(CreateActiveViewModel activeViewModel)
        {
            if (activeViewModel is null)
                return BadRequest("O body para criar um novo ativo não deve ser nulo.");

            Active activeCreated = await _repository.CreateAsync(activeViewModel);

            return new CreatedAtRouteResult("SearchActive", new { id = activeCreated.Active_Id }, activeCreated);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Active>>> GetAll()
        {
            return Ok(await _repository.GetAllAsync());
        }

        [HttpGet("purses")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Active>>> GetAllWithPurses()
        {
            return Ok(await _repository.GetAllWithPursesAsync());
        }

        [HttpGet("{id}", Name = "SearchActive")]
        [Authorize]
        public async Task<ActionResult<Active>> GetById(int id)
        {
            return Ok(await _repository.GetByIdAsync(id));
        }

        [HttpGet("{id}/purses")]
        [Authorize]
        public async Task<ActionResult<Active>> GetByIdWithPurses(int id)
        {
            var ActiveVerify = await _repository.GetByIdWithPursesAsync(id);

            if (ActiveVerify is null)
                return NotFound("Active not found.");

            return Ok(ActiveVerify);
        }

        [HttpPut("{activeId}")]
        [Authorize]
        public async Task<IActionResult> Update(int activeId, UpdateActiveViewModel activeViewModel)
        {
            _repository.Update(activeId, activeViewModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }

        [HttpGet("/search-active/{active}/{dYDesiredPercentage}")]
        public async Task<ActionResult<ActiveReturn>> SearchActive(string active, string dYDesiredPercentage)
        {
            return Ok(await _repository.SearchActiveAsync(active, dYDesiredPercentage));
        }

        [HttpGet("/search-active-purse-details/{purseId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ActiveReturnForPurseDetailsDTO>>> searchActivesForPurseDetails(int purseId)
        {
            return Ok(await _repository.GetActivesForShowInPurseDetails(purseId));
        }

        [HttpGet("/get-actives/{purseId}")]
        [Authorize]
        public async Task<ActionResult> GetActivesByPurseId(int purseId)
        {
            return Ok(await _repository.GetActivesByPurseId(purseId));
        }
    }
}
