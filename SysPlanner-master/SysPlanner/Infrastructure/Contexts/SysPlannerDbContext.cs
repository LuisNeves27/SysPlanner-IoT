using Microsoft.EntityFrameworkCore;
using SysPlanner.Infrastructure.Persistance;
using SysPlanner.Infrastructure.Mappings;

namespace SysPlanner.Infrastructure.Contexts
{
    public class SysPlannerDbContext : DbContext
    {
        public SysPlannerDbContext(DbContextOptions<SysPlannerDbContext> options)
            : base(options)
        { }

        public DbSet<Lembrete> Lembretes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // novos DbSets
        public DbSet<Localizacao> Localizacoes { get; set; }
        public DbSet<EventoIoT> EventosIoT { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LembreteMapping());
            modelBuilder.ApplyConfiguration(new UsuarioMapping());

            // novos mappings
            modelBuilder.ApplyConfiguration(new LocalizacaoMapping());
            modelBuilder.ApplyConfiguration(new EventoIoTMapping());
        }
    }
}
