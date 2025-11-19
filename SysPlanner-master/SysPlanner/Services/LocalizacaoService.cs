using Microsoft.EntityFrameworkCore;
using SysPlanner.Infrastructure.Contexts;
using SysPlanner.Infrastructure.Persistance;
using SysPlanner.Services.Interfaces;

namespace SysPlanner.Services
{
    public class LocalizacaoService : ILocalizacaoService
    {
        private readonly SysPlannerDbContext _db;
        public LocalizacaoService(SysPlannerDbContext db)
        {
            _db = db;
        }

        public async Task<Localizacao> SalvarAsync(Localizacao localizacao)
        {
            localizacao.Id = Guid.NewGuid();
            localizacao.Timestamp = DateTime.UtcNow;
            _db.Localizacoes.Add(localizacao);
            await _db.SaveChangesAsync();
            return localizacao;
        }

        public async Task<IEnumerable<Localizacao>> ListarPorUsuarioAsync(Guid usuarioId, int limit = 50)
        {
            return await _db.Localizacoes
                .Where(l => l.UsuarioId == usuarioId)
                .OrderByDescending(l => l.Timestamp)
                .Take(limit)
                .ToListAsync();
        }
    }
}
