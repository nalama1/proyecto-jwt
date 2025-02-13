using System.ComponentModel.DataAnnotations;

namespace MSPersona.Dominio.Models
{
    public class Usuario
    {
        public int? ID { get; set; }

        [Required]
        public string  UserName { get; set; } = string.Empty;
        [Required]
        public string  Password { get; set; } = string.Empty;
        public DateTime? FechaCreacion { get; set; }

        public int? PersonaID { get; set; }

        public string? Email { get; set; }

    }
}
