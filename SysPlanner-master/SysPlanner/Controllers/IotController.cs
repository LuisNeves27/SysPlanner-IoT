using Microsoft.AspNetCore.Mvc;
using SysPlanner.Infrastructure.Persistance;
using SysPlanner.Services.Interfaces;

namespace SysPlanner.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class IotController : ControllerBase
    {
        private readonly IIoTService _iotService;
        private readonly ILembreteService _lembreteService;
        private readonly ILogger<IotController> _logger;

        public IotController(
            IIoTService iotService,
            ILembreteService lembreteService,
            ILogger<IotController> logger)
        {
            _iotService = iotService;
            _lembreteService = lembreteService;
            _logger = logger;
        }

        // Simulação rápida — o dashboard chama este endpoint
        [HttpPost("simulate-home")]
        public async Task<IActionResult> SimulateHome([FromBody] SimulateHomeRequest req)
        {
            if (req == null || req.UserId == Guid.Empty)
                return BadRequest("userId required.");

            var evento = new EventoIoT
            {
                UsuarioId = req.UserId,
                Area = "Casa",
                Evento = "entered",
                Latitude = req.Latitude,
                Longitude = req.Longitude
            };

            var saved = await _iotService.CriarEventoAsync(evento);

            // Lógica POC: quando entrar em 'Casa', gerar lembrete via IA endpoint local
            // Neste POC, vamos criar um lembrete simples automaticamente (ex: "Lembrete gerado automaticamente")
            try
            {
                var lembrete = new Lembrete
                {
                    Titulo = "Lembrete Automático: Chegou em Casa",
                    Descricao = $"Detectado chegada em {saved.Area} às {saved.Timestamp:O}",
                    Data = DateTime.UtcNow.AddHours(1),
                    Prioridade = SysPlanner.Infrastructure.Persistance.Enums.Prioridade.Media,
                    Categoria = SysPlanner.Infrastructure.Persistance.Enums.Categoria.Pessoal,
                    UsuarioId = req.UserId
                };

                var created = await _lembreteService.CriarAsync(lembrete);

                return Ok(new { message = "Evento criado e lembrete gerado.", evento = saved, lembreteId = created.Id });
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Erro ao criar lembrete automático: {err}", ex.Message);
                return Ok(new { message = "Evento criado, falha ao gerar lembrete automático.", evento = saved });
            }
        }

        public class SimulateHomeRequest
        {
            public Guid UserId { get; set; }
            public double? Latitude { get; set; }
            public double? Longitude { get; set; }
        }
    }
}
