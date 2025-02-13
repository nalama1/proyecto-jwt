using Microsoft.AspNetCore.Mvc;
using MSPersona.Dominio.Interfaces;
using MSPersona.Dominio.Models;
using MSPersona.Dominio.Models.Request;
using MSPersona.Dominio.Models.Response;

namespace MSPersona.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _UsuarioService;
        private readonly IPersonaService _PersonaService;
        private readonly Serilog.ILogger _loggers;
       
        public UsuariosController(IUsuarioService UsuarioService, IPersonaService PersonaService, Serilog.ILogger loggers) 
        {
            _UsuarioService = UsuarioService;
            _PersonaService = PersonaService;
            _loggers = loggers;
        }

        [HttpPost("login")]
        //[Authorize]
        public ActionResult<Usuario> Autentificar([FromBody] AuthRequest model)
        {
            string nombreControlador = ControllerContext.ActionDescriptor.ControllerName;
            try
            {
                _loggers.Information("Inicio Autentificar en {NombreControlador}", nombreControlador);
                Respuesta respuesta = new Respuesta();
                var userResponse = _UsuarioService.Auth(model);
                if (userResponse == null)
                {
                    respuesta.Exito = 0;
                    respuesta.Mensaje = "Usuario o contraseña incorrecta";
                    return BadRequest(respuesta);
                }

                respuesta.Exito = 1;
                respuesta.Mensaje = "Autenticación exitosa";
                respuesta.Data = userResponse;              

                return Ok(respuesta);
            }
            catch (ArgumentException ex)
            {
                _loggers.Error(ex, "Error en la solicitud de autenticación en {NombreControlador}", nombreControlador);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _loggers.Error(ex, "Error al autenticar usuario en {NombreControlador}", nombreControlador);
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }


        /// <summary>
        /// Grabar nuevo usuario que cuente con un código verificado
        /// </summary>
        /// <param name="numeroIdentificacion"></param>
        /// <param name="codigoVerificacion"></param>
        /// <param name="usuario"></param>
        /// <returns></returns>
        [HttpPost("{numeroIdentificacion}/{codigoVerificacion}")]
        public async Task<ActionResult> GrabarUsuario(string numeroIdentificacion, int codigoVerificacion, [FromBody] Usuario usuario)
        {
            string nombreControlador = ControllerContext.ActionDescriptor.ControllerName;
            try
            {
                _loggers.Information("Inicio GrabarUsuario en {NombreControlador}", nombreControlador);
                if (string.IsNullOrEmpty(numeroIdentificacion) || 
                    codigoVerificacion <= 0 || usuario == null ||
                    string.IsNullOrEmpty(usuario.UserName) || string.IsNullOrEmpty(usuario.Password))
                {
                    _loggers.Information("Datos de entrada inválidos. Verifique número de identificación, código de verificación e información del usuario.");
                    return BadRequest("Datos de entrada inválidos. Verifique número de identificación, código de verificación e información del usuario.");
                }

                //obtener personaID
                var personaId = await _PersonaService.GetPersonaID(numeroIdentificacion, codigoVerificacion);
                if (personaId == 0)
                {
                    _loggers.Information("Código de verificación no válido o persona no registrada o eliminada.");
                    return BadRequest("Código de verificación no válido o persona no registrada o eliminada.");
                }                

                //verificar que el codigo de verificacion ingresado sea el correcto  
                var personaVerificado = await _PersonaService.GetCodigoVerificado(personaId, codigoVerificacion);

                if (personaVerificado == null)
                {
                    _loggers.Information("Código de verificación no válido o persona no registrada o eliminada");
                    return NotFound("Código de verificación no válido o persona no registrada o eliminada.");
                }

                var usuarioExistente = await _UsuarioService.ExisteUsuarioParaPersona(personaId);
                if(usuarioExistente != null)
                {
                    _loggers.Information("Usuario ya se encuentra registrado");
                    return Conflict("Usuario ya se encuentra registrado.");
                }

                //asignar personaID
                usuario.PersonaID = personaId;

                //llamando al servicio 
                await _UsuarioService.GrabarUsuario(usuario);

                var authRequest = new AuthRequest
                {   Password = usuario.Password, 
                    Usuario = usuario.UserName 
                };

                //se genera el token después de crear el usuario
                var userResponse = _UsuarioService.Auth(authRequest);
                if (userResponse == null)
                {
                    _loggers.Error( nombreControlador, "Error en la autenticación después de crear el usuario");
                    return BadRequest(new Respuesta
                    {
                        Exito = 0,
                        Mensaje = "Error en la autenticación después de crear el usuario"
                    });
                }

                var resultado = new
                {
                    Usuario = usuario.UserName,
                    Token = userResponse.Token
                };

                return CreatedAtAction(nameof(GrabarUsuario), new { numeroIdentificacion, codigoVerificacion }, resultado);
            }
            catch (ArgumentException ex)
            {
                _loggers.Error(ex, "Error al grabar usuario: {ErrorMessage}", ex.Message);  
                return BadRequest(ex.Message);  
            }
            catch (Exception ex)
            {
                _loggers.Error(ex, "Error interno del servidor al grabar usuario: {ErrorMessage}", ex.Message);  
                return StatusCode(500, "Ocurrió un error interno del servidor al procesar su solicitud.");  
            }
        }

    }
}
