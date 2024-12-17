using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyInvestAPI.Domain;
using MyInvestAPI.Services.Interfaces;
using MyInvestAPI.ViewModels;

namespace MyInvestAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SmtpPropertiesController : ControllerBase
    {
        private readonly ISmtpPropertiesService _service;

        public SmtpPropertiesController(ISmtpPropertiesService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<SmtpProperties>> Create(CreateSmtpPropertiesViewModel request)
        {
            SmtpProperties smtpPropertiesResponse = await _service.Create(request);
            return StatusCode(201, smtpPropertiesResponse);
        }

        [Authorize]
        [HttpPut("id:int")]
        public async Task<IActionResult> Update(int id, UpdateSmtpPropertiesViewModel request)
        {
            await _service.Update(id, request);
            return NoContent();
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SmtpProperties>>> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [Authorize]
        [HttpGet("id:int")]
        public async Task<ActionResult<SmtpProperties>> GetById(int id)
        {
            if (id <= 0)
            {
                return BadRequest("O id deve ser maior que zero!");
            }

            return Ok(await _service.GetById(id));
        }

        [Authorize]
        [HttpDelete("id:int")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}
