using System;
using System.Threading.Tasks;

namespace SysPlanner.Services.IA
{
    public class IAService
    {
        public IAService()
        {
        }

        public async Task<string> GerarLembreteIA(Guid userId, string? descricao)
        {
            await Task.Delay(200); // simula processamento da IA

            if (string.IsNullOrWhiteSpace(descricao))
                descricao = "Lembrete gerado automaticamente pela IA.";

            return $"Lembrete IA para o usuário {userId}: {descricao}";
        }
    }
}
