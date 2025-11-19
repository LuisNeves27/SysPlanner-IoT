using Microsoft.AspNetCore.Mvc;
using SysPlanner.Services.Interfaces;

namespace SysPlanner.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReminderController : ControllerBase
    {
        private readonly ILembreteService _lembreteService;

        public ReminderController(ILembreteService lembreteService)
        {
            _lembreteService = lembreteService;
        }

        [HttpGet("list")]
        public async Task<IActionResult> List()
        {
            // POC: retorna os lembretes (primeira página grande)
            var (items, total) = await _lembreteService.ListarTodosAsync(1, 50);
            var simplified = items.Select(l => new { message = l.Descricao ?? l.Titulo, title = l.Titulo, id = l.Id });
            return Ok(simplified);
        }
    }
}
