using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SysPlanner.Infrastructure.Persistance;

namespace SysPlanner.Infrastructure.Mappings
{
    public class LocalizacaoMapping : IEntityTypeConfiguration<Localizacao>
    {
        public void Configure(EntityTypeBuilder<Localizacao> builder)
        {
            builder.ToTable("Localizacoes");

            builder.HasKey(l => l.Id);

            builder.Property(l => l.Latitude).IsRequired();
            builder.Property(l => l.Longitude).IsRequired();
            builder.Property(l => l.Timestamp).IsRequired();

            builder.HasIndex(l => l.UsuarioId);
        }
    }
}
