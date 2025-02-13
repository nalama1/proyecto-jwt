using NuGetCommon;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MSPersona.Dominio.Interfaces;
using MSPersona.Dominio.Models;
using MSPersona.Dominio.Models.Request;
using MSPersona.Dominio.Models.Response;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MSPersona.Data
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly string _connectionString;
        private readonly Serilog.ILogger _logger;
        private readonly AppiSettings _appSettings;
        private readonly IConfiguration _configuration;

        public UsuarioRepository(IOptions<AppiSettings> appSettings, IConfiguration configuration, Serilog.ILogger logger)
        {
            _appSettings = appSettings.Value;
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

        public async Task<Usuario> ExisteUsuarioParaPersona(int personaID)
        {
            using var con = Connection;
            const string query = "ConsultarUsuarioPorPersona";
            var parametros = new { personaID };

            return await con.QueryFirstOrDefaultAsync<Usuario>(query, parametros, commandType: CommandType.StoredProcedure);
        }

        public async Task<bool> GrabarUsuario(Usuario usuario)
        {
            string spassword = Encrypts.GetSHA256(usuario.Password);

            using var con = Connection;
            const string query = "GrabarUsuario";
            int affectedRows = await con.ExecuteAsync(query, new
            {
                //ID = usuario.ID,
                PersonaID = usuario.PersonaID,
                Usuario = usuario.UserName,
                Password = spassword
            }, commandType: CommandType.StoredProcedure);

            return affectedRows > 0;
        }

        public UserResponse Auth(AuthRequest model)
        {
            UserResponse userresponse = new UserResponse();

            using (var connection = Connection)
            {
                string spassword = Encrypts.GetSHA256(model.Password); //here
                string sql = "ObtenerUsuarioPorClave";
                var parametros = new { Usuario = model.Usuario, Password = spassword };
                var usuario = connection.QueryFirstOrDefault<Usuario>(sql, parametros, commandType: CommandType.StoredProcedure);

                if (usuario == null) return null;
                userresponse.Usuario = usuario.UserName;
                userresponse.Token = GetToken(usuario); //dar token a Usuario 
            }
            return userresponse;
        }

        private string GetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, usuario.ID.ToString()),
                        new Claim(ClaimTypes.Email, usuario.Email)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
 
