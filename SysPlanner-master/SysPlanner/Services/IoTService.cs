using Microsoft.EntityFrameworkCore;
using SysPlanner.Infrastructure.Contexts;
using SysPlanner.Infrastructure.Persistance;
using SysPlanner.Services.Interfaces;

namespace SysPlanner.Services
{
    public class IoTService : IIoTService
    {
        private readonly SysPlannerDbContext _db;
        public IoTService(SysPlannerDbContext db)
        {
            _db = db;
        }

        public async Task<EventoIoT> CriarEventoAsync(EventoIoT evento)
        {
            evento.Id = Guid.NewGuid();
            evento.Timestamp = DateTime.UtcNow;
            _db.EventosIoT.Add(evento);
            await _db.SaveChangesAsync();
            return evento;
        }

        public async Task<IEnumerable<EventoIoT>> ListarEventosUsuarioAsync(Guid usuarioId, int limit = 50)
        {
            return await _db.EventosIoT
                .Where(e => e.UsuarioId == usuarioId)
                .OrderByDescending(e => e.Timestamp)
                .Take(limit)
                .ToListAsync();
        }
    }
}
