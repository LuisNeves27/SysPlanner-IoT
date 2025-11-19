using System.ComponentModel.DataAnnotations;

namespace SysPlanner.Infrastructure.Persistance
{
    public class EventoIoT
    {
        public Guid Id { get; set; }

        [Required]
        public Guid UsuarioId { get; set; }

        [Required]
        [StringLength(100)]
        public string Area { get; set; } = string.Empty; // ex: "Casa"

        [Required]
        [StringLength(50)]
        public string Evento { get; set; } = string.Empty; // ex: "entered" / "exited"

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
