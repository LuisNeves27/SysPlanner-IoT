using Microsoft.AspNetCore.Mvc;
using SysPlanner.Infrastructure.Persistance;
using SysPlanner.Services.Interfaces;

namespace SysPlanner.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class LocationController : ControllerBase
    {
        private readonly ILocalizacaoService _localService;
        private readonly ILogger<LocationController> _logger;

        public LocationController(ILocalizacaoService localService, ILogger<LocationController> logger)
        {
            _localService = localService;
            _logger = logger;
        }

        [HttpPost("save")]
        public async Task<IActionResult> Save([FromBody] Localizacao dto)
        {
            if (dto == null || dto.UsuarioId == Guid.Empty)
                return BadRequest("Dados inválidos.");

            var saved = await _localService.SalvarAsync(dto);
            return Ok(saved);
        }

        [HttpGet("user/{usuarioId:guid}")]
        public async Task<IActionResult> ByUser(Guid usuarioId)
        {
            var list = await _localService.ListarPorUsuarioAsync(usuarioId);
            return Ok(list);
        }
    }
}
