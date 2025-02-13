using System.ComponentModel.DataAnnotations;

namespace MSPersona.Dominio.Models
{
    public class Persona
    {
        public int Id { get; set; }
        [Required]
        public string Nombres { get; set; } = string.Empty;
        [Required]
        public string Apellidos { get; set; } = string.Empty;
        [Required]
        public string NumeroIdentificacion { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        
        public string TipoIdentificacion { get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }

        public int TipoIdentificacionID { get; set; }
        public string? Eliminado { get; set; }
        
    }
}


