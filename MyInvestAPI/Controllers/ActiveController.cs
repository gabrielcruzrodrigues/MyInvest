using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyInvestAPI.Domain;
using MyInvestAPI.Domain.DTO;
using MyInvestAPI.Services.Interfaces;
using MyInvestAPI.ViewModels;
using System;
using System.Security.Claims;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ActiveController : ControllerBase
    {
        public readonly IActiveService _service;
        public readonly IPurseService _purseService;

        public ActiveController(IActiveService service, IPurseService purseService)
        {
            _service = service;
            _purseService = purseService;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Active>> Create(CreateActiveViewModel activeViewModel)
        {
            if (activeViewModel is null)
                return BadRequest("O body para criar um novo ativo não deve ser nulo.");

            var purse = await _purseService.GetByIdAsync(activeViewModel.Purse_Id);
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != purse.User_Id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Você não pode criar um ativo na carteira de outro usuário." });
            }

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
            var active = await _service.GetByIdAsync(id);

            var purse = await _purseService.GetByIdAsync(active.PurseId);
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != purse.User_Id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Você não pode buscar o ativo de outro usuário." });
            }

            return Ok(active);
        }

        [HttpGet("{id}/purses")]
        [Authorize]
        public async Task<ActionResult<Active>> GetByIdWithPurses(int id)
        {
            var ActiveVerify = await _service.GetByIdWithPursesAsync(id);

            if (ActiveVerify is null)
                return NotFound("Active not found.");

            var purse = await _purseService.GetByIdAsync(ActiveVerify.PurseId);
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != purse.User_Id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Você não pode buscar o ativo de outro usuário." });
            }

            return Ok(ActiveVerify);
        }

        [HttpPut("{activeId}")]
        [Authorize]
        public async Task<IActionResult> Update(int activeId, UpdateActiveViewModel activeViewModel)
        {
            var active = await _service.GetByIdAsync(activeId);

            var purse = await _purseService.GetByIdAsync(active.PurseId);
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != purse.User_Id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Você não pode atualizar o ativo de outro usuário." });
            }

            await _service.Update(activeId, activeViewModel);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var active = await _service.GetByIdAsync(id);

            var purse = await _purseService.GetByIdAsync(active.PurseId);
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != purse.User_Id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Você não pode deletar o ativo de outro usuário." });
            }

            await _service.Delete(id);
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
            var purse = await _purseService.GetByIdAsync(purseId);
            var userIdFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdFromToken != purse.User_Id)
            {
                return StatusCode(StatusCodes.Status403Forbidden, new { message = "Você não pode buscar os ativos de outros usuário." });
            }

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
