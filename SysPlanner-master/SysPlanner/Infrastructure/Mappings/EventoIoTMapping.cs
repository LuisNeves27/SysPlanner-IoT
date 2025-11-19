using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SysPlanner.Infrastructure.Persistance;

namespace SysPlanner.Infrastructure.Mappings
{
    public class EventoIoTMapping : IEntityTypeConfiguration<EventoIoT>
    {
        public void Configure(EntityTypeBuilder<EventoIoT> builder)
        {
            builder.ToTable("EventosIoT");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Area).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Evento).HasMaxLength(50).IsRequired();
            builder.Property(e => e.Latitude);
            builder.Property(e => e.Longitude);
            builder.Property(e => e.Timestamp).IsRequired();

            builder.HasIndex(e => e.UsuarioId);
        }
    }
}
