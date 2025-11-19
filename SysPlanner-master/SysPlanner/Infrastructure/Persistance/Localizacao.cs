using System.ComponentModel.DataAnnotations;

namespace SysPlanner.Infrastructure.Persistance
{
    public class Localizacao
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UsuarioId { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
