using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyInvestAPI.Domain;
using MyInvestAPI.Services.Interfaces;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PurseController : ControllerBase
    {
        public readonly IPurseService _service;

        public PurseController(IPurseService IPurseService)
        {
            _service = IPurseService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Purse>> Create(CreatePurseViewModel purseViewModel)
        {
            if (purseViewModel is null)
                return BadRequest("The body for create purse must not be null.");

            var purseCreated = await _service.CreateAsync(purseViewModel);
                
            return new CreatedAtRouteResult("GetPurse", new { id = purseCreated.Purse_Id }, purseCreated);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Purse>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpGet("actives")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Purse>>> GetAllWithActives()
        {
            return Ok(await _service.GetAllWithActivesAsync());
        }

        [HttpGet("{id:int}", Name = "GetPurse")]
        [Authorize]
        public async Task<ActionResult<Purse>> getById(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpGet("{id:int}/actives")]
        [Authorize]
        public async Task<ActionResult<Purse>> getByIdWithActives(int id)
        {
            return Ok(await _service.GetByIdWithActivesAsync(id));
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdatePurseViewModel purseViewModel)
        {
            if (purseViewModel is null)
                return BadRequest("The body for update purse must not be null.");

            await _service.Update(id, purseViewModel);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
