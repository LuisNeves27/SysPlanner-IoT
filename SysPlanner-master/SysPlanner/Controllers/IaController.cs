using Microsoft.AspNetCore.Mvc;
using SysPlanner.Infrastructure.Persistance;

namespace SysPlanner.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class IaController : ControllerBase
    {
        private readonly ILogger<IaController> _logger;

        public IaController(ILogger<IaController> logger)
        {
            _logger = logger;
        }

        [HttpPost("generate-reminder")]
        public async Task<IActionResult> GenerateReminder([FromBody] GenerateReminderRequest req)
        {
            // POC: geramos um texto simples que simula a IA.
            // Em produção, aqui você chamaria um serviço de LLM / OpenAI ou modelo local.
            await Task.Yield();

            var text = $"Olá! Lembrete gerado automaticamente para o usuário {req.UserId}. " +
                       $"Resumo do compromisso: {req.Compromisso ?? "Sem detalhe"}. " +
                       $"Sugestão: checar detalhes e confirmar horário.";

            return Ok(new { reminder = text });
        }

        public class GenerateReminderRequest
        {
            public Guid UserId { get; set; }
            public string? Compromisso { get; set; }
        }
    }
}
