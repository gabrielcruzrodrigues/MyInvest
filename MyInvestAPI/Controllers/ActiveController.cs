using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyInvestAPI.Api;
using MyInvestAPI.Data;
using MyInvestAPI.Domain;
using MyInvestAPI.Domain.DTO;
using MyInvestAPI.Repositories.Interfaces;
using MyInvestAPI.Services.Interfaces;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActiveController : ControllerBase
    {
        public readonly IActiveService _service;

        public ActiveController(IActiveService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Active>> Create(CreateActiveViewModel activeViewModel)
        {
            if (activeViewModel is null)
                return BadRequest("O body para criar um novo ativo não deve ser nulo.");

            Active activeCreated = await _service.CreateAsync(activeViewModel);

            return new CreatedAtRouteResult("SearchActive", new { id = activeCreated.Active_Id }, activeCreated);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Active>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("purses")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Active>>> GetAllWithPurses()
        {
            return Ok(await _service.GetAllWithPursesAsync());
        }

        [HttpGet("{id}", Name = "SearchActive")]
        [Authorize]
        public async Task<ActionResult<Active>> GetById(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("{id}/purses")]
        [Authorize]
        public async Task<ActionResult<Active>> GetByIdWithPurses(int id)
        {
            var ActiveVerify = await _service.GetByIdWithPursesAsync(id);

            if (ActiveVerify is null)
                return NotFound("Active not found.");

            return Ok(ActiveVerify);
        }

        [HttpPut("{activeId}")]
        [Authorize]
        public async Task<IActionResult> Update(int activeId, UpdateActiveViewModel activeViewModel)
        {
            _service.Update(activeId, activeViewModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            _service.Delete(id);
            return NoContent();
        }

        [HttpGet("/search-active/{active}/{dYDesiredPercentage}")]
        public async Task<ActionResult<ActiveReturn>> SearchActive(string active, string dYDesiredPercentage)
        {
            return Ok(await _service.SearchActiveAsync(active, dYDesiredPercentage));
        }

        [HttpGet("/search-active-purse-details/{purseId}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ActiveReturnForPurseDetailsDTO>>> searchActivesForPurseDetails(int purseId)
        {
            return Ok(await _service.GetActivesForShowInPurseDetails(purseId));
        }

        [HttpGet("/get-actives/{purseId}")]
        [Authorize]
        public async Task<ActionResult> GetActivesByPurseId(int purseId)
        {
            return Ok(await _service.GetActivesByPurseId(purseId));
        }
    }
}
