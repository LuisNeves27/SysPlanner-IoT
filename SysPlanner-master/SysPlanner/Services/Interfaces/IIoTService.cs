using SysPlanner.Infrastructure.Persistance;

namespace SysPlanner.Services.Interfaces
{
    public interface IIoTService
    {
        Task<EventoIoT> CriarEventoAsync(EventoIoT evento);
        Task<IEnumerable<EventoIoT>> ListarEventosUsuarioAsync(Guid usuarioId, int limit = 50);
    }
}
