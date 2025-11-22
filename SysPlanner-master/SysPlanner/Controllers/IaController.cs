using Microsoft.AspNetCore.Mvc;
using SysPlanner.Services;
using SysPlanner.Services.IA;

namespace SysPlanner.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class IaController : ControllerBase
    {
        private readonly IAService _iaService;

        public IaController(IAService iaService)
        {
            _iaService = iaService;
        }

        // ==========================================
        // 🔥 ROTA PRINCIPAL — GERA E SALVA LEMBRETE
        // ==========================================
        [HttpPost("generate-reminder")]
        public async Task<IActionResult> GenerateReminder([FromBody] IARequest req)
        {
            if (req.UserId == Guid.Empty)
                return BadRequest("UserId inválido.");

            var reminder = await _iaService.GerarLembreteIA(req.UserId, req.Descricao);

            return Ok(new
            {
                message = "Lembrete IA criado e salvo com sucesso!",
                lembrete = reminder
            });
        }

        public class IARequest
        {
            public Guid UserId { get; set; }
            public string? Descricao { get; set; }
        }
    }
}

