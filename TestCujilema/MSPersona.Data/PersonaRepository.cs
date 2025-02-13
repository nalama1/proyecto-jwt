using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using MSPersona.Dominio.Interfaces;
using MSPersona.Dominio.Models;
using System.Data;

namespace MSPersona.Data
{
    public class PersonaRepository : IPersonaRepository
    {

        private readonly string _connectionString;
        private readonly Serilog.ILogger _logger;

        public PersonaRepository(IConfiguration configuration, Serilog.ILogger logger)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DBConnection");

            if (string.IsNullOrEmpty(_connectionString))
            {
                _logger.Error("La cadena de conexión 'DBConnection' no está configurada.");
                throw new InvalidOperationException("Cadena de conexión no configurada.");
            }
        }

        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_connectionString);
            }
        }


        /// <summary>
        /// Obtener Listados de Personas
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Persona>> GetPersona()
        {
            using var con = Connection;
            const string query = "ConsultarPersonas";

            // Llamada al SP usando Dapper
            return await con.QueryAsync<Persona>(query, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Obtener una persona específica por Id
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<Persona> GetPersonaByID(int id)
        {
            using var con = Connection;
            const string query = "ConsultarPersonaPorID";
            var parametros = new { id };

            return await con.QueryFirstOrDefaultAsync<Persona>(query, parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<int> GetPersonaID(string numeroIdentificacion, int codigoVerificacion)
        {
            using var con = Connection;
            const string query = "ObtenerPersonaID";
            var parametros = new { numeroIdentificacion, codigoVerificacion };

            return await con.QueryFirstOrDefaultAsync<int>(query, parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<Persona> GetCodigoVerificado(int personaID, int codigoVerificacion)
        {
            using var con = Connection;
            const string query = "ConsultarPersonaPorCodigoVerificacion";
            var parametros = new { personaID, codigoVerificacion };

            return await con.QueryFirstOrDefaultAsync<Persona>(query, parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<Persona> GetPersonaByNumeroIdentificacion(string numeroIdentificacion)
        {
            using var con = Connection;
            const string query = "ConsultarPersonaPorNumeroIdentificacion";
            var parametros = new { numeroIdentificacion };

            return await con.QueryFirstOrDefaultAsync<Persona>(query, parametros, commandType: CommandType.StoredProcedure);
        }

        /// <summary>
        /// Eliminar una persona específica por Id (eliminado lógico)
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<bool> DeletePersona(int id)
        {
            using var con = Connection;
            string sql = "PersonaEliminadoLogico";
            int affectedRows = await con.ExecuteAsync(sql, new { id }, commandType: CommandType.StoredProcedure);
            return affectedRows > 0;
        }

        /// <summary>
        /// Actualiza datos de una Persona específica por su Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="persona"></param>
        /// <returns></returns>
        public async Task<bool> ActualizaPersona(int id, Persona persona)
        {
            using var con = Connection;
            string sql = @"ActualizarPersona";
            int affectedRows = await con.ExecuteAsync(sql, new
            {
                Nombres = persona.Nombres,
                Apellidos = persona.Apellidos,
                NumeroIdentificacion = persona.NumeroIdentificacion,
                Email = persona.Email,
                TipoIdentificacionID = persona.TipoIdentificacionID,
                FechaCreacion = persona.FechaCreacion,
                Id = id
            }, commandType:CommandType.StoredProcedure);

            return affectedRows > 0;
        }
        
        /// <summary>
        /// Registra una nueva persona
        /// </summary>
        /// <param name="persona"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, int CodigoSecuencia)> GrabarPersona(Persona persona)
        {
            using var con = Connection;
            const string query = "GrabaPersona";

            var parametros = new DynamicParameters();
            parametros.Add("@Nombres", persona.Nombres);
            parametros.Add("@Apellidos", persona.Apellidos);
            parametros.Add("@NumeroIdentificacion", persona.NumeroIdentificacion);
            parametros.Add("@Email", persona.Email);
            parametros.Add("@TipoIdentificacionID", persona.TipoIdentificacionID);
            parametros.Add("@codigoSecuencia", dbType: DbType.Int32, direction: ParameterDirection.Output);
             
            int affectedRows = await con.ExecuteAsync(query, parametros, commandType: CommandType.StoredProcedure);

            int codigoSecuencia = parametros.Get<int>("@codigoSecuencia");

            return (affectedRows == -1, codigoSecuencia);
        }
    }
}
 
