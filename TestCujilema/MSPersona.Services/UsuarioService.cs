using MSPersona.Dominio.Interfaces;
using MSPersona.Dominio.Models;
using MSPersona.Dominio.Models.Request;
using MSPersona.Dominio.Models.Response;

namespace MSPersona.Services
{
    public class UsuarioService : IUsuarioService
    {
        public readonly IUsuarioRepository _UsuarioRepository;
        public UsuarioService(IUsuarioRepository UsuarioRepository)
        {
            _UsuarioRepository = UsuarioRepository;
        }

        public UserResponse Auth(AuthRequest model)
        {
            return _UsuarioRepository.Auth(model);
        }

        public async Task<Usuario> ExisteUsuarioParaPersona(int personaID)
        {
            return await _UsuarioRepository.ExisteUsuarioParaPersona(personaID);
        }

        public async Task<bool> GrabarUsuario(Usuario usuario)
        {
            return await _UsuarioRepository.GrabarUsuario(usuario);
        }
     

    }
}

 