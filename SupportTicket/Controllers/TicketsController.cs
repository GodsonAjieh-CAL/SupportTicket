using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using SupportTicket.Services;

namespace SupportTicket.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet(Name = "GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _ticketService.GetAll();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _ticketService.GetById(id);

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        
        [HttpPost(Name = "Create")]
        public async Task<IActionResult> Create(string title)
        {
            var result = await _ticketService.Create(title);

            if (result == null)
            {
                return Conflict();
            }
            return Ok(result);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, string status)
        {
            var result = await _ticketService.Update(id, status);

            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _ticketService.Delete(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}
