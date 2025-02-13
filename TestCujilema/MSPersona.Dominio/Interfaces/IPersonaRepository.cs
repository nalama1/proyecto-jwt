using MSPersona.Dominio.Models;

namespace MSPersona.Dominio.Interfaces
{
    public interface IPersonaRepository
    {
        Task<IEnumerable<Persona>> GetPersona();
        Task<Persona> GetPersonaByID(int id);
        Task<int> GetPersonaID(string numeroIdentificacion, int codigoVerificacion);
        Task<Persona> GetCodigoVerificado(int personaID, int codigoVerificacion);
        Task<Persona> GetPersonaByNumeroIdentificacion(string numeroIdentificacion);
        Task<bool> DeletePersona(int id);
        Task<bool> ActualizaPersona(int id, Persona persona);     
        Task<(bool IsSuccess, int CodigoSecuencia)> GrabarPersona(Persona persona);
        
    }
}

 