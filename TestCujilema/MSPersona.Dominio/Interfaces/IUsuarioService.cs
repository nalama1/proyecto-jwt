using MSPersona.Dominio.Models;
using MSPersona.Dominio.Models.Request;
using MSPersona.Dominio.Models.Response;

namespace MSPersona.Dominio.Interfaces
{
    public interface IUsuarioService
    {
        UserResponse Auth(AuthRequest model);
        Task<bool> GrabarUsuario(Usuario usuario);
        Task<Usuario> ExisteUsuarioParaPersona(int personaID);
    }
}
