using System.ComponentModel.DataAnnotations;

namespace MSPersona.Dominio.Models.Request
{
    public class AuthRequest
    {
        [Required]
        public string Usuario { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
