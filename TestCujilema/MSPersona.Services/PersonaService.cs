using MSPersona.Dominio.Interfaces;
using MSPersona.Dominio.Models;

namespace MSPersona.Services
{
    public class PersonaService : IPersonaService
    {
        public readonly IPersonaRepository _PersonaRepository;
        public PersonaService(IPersonaRepository PersonaRepository)
        {
            _PersonaRepository = PersonaRepository;
        }
        public async Task<IEnumerable<Persona>> GetPersona()
        {
            return await _PersonaRepository.GetPersona();
        }
        public async Task<Persona> GetPersonaByID(int id)
        {
            return await _PersonaRepository.GetPersonaByID(id);
        }
        public async Task<int> GetPersonaID(string numeroIdentificacion, int codigoVerificacion)
        {
            return await _PersonaRepository.GetPersonaID(numeroIdentificacion, codigoVerificacion);
        }
        public async Task<Persona> GetCodigoVerificado(int personaID, int codigoVerificacion)
        {
            return await _PersonaRepository.GetCodigoVerificado(personaID,codigoVerificacion);
        }
        public async Task<Persona> GetPersonaByNumeroIdentificacion(string numeroIdentificacion)
        {
            return await _PersonaRepository.GetPersonaByNumeroIdentificacion(numeroIdentificacion);
        }
        public async Task<bool> DeletePersona(int id)
        {
            return await _PersonaRepository.DeletePersona(id);
        }
        public async Task<bool> ActualizaPersona(int id, Persona persona)
        {
            return await _PersonaRepository.ActualizaPersona(id, persona);
        }

        public async Task<(bool IsSuccess, int CodigoSecuencia)> GrabarPersona(Persona persona)
        {
            return await _PersonaRepository.GrabarPersona(persona);
        }

        
    }
}
