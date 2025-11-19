using SysPlanner.Infrastructure.Persistance;

namespace SysPlanner.Services.Interfaces
{
    public interface ILocalizacaoService
    {
        Task<Localizacao> SalvarAsync(Localizacao localizacao);
        Task<IEnumerable<Localizacao>> ListarPorUsuarioAsync(Guid usuarioId, int limit = 50);
    }
}
