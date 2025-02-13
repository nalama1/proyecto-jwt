using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MSPersona.Dominio.Interfaces;
using MSPersona.Dominio.Models;

namespace MSPersona.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class PersonasController : ControllerBase
    {
        private readonly IPersonaService _PersonaService;
        private readonly Serilog.ILogger _logger;

        public PersonasController(IPersonaService PersonaService, Serilog.ILogger logger)
        {
            _PersonaService = PersonaService;
            _logger = logger;
        }

        [HttpGet]
        [EnableCors("AccesoPersona")]
        public async Task<ActionResult<IEnumerable<Persona>>> GetPersona()
        {
            string nombreControlador = ControllerContext.ActionDescriptor.ControllerName;
            try
            {
                _logger.Information("Inicio GetPersona en {NombreControlador}", nombreControlador);
                var Persona = await _PersonaService.GetPersona();

                if (Persona == null)
                {
                    _logger.Information("Persona no encontrado o eliminado");
                    return NotFound($"Persona no encontrado o eliminado.");
                }

                return Ok(Persona);
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "Error en la solicitud de Persona");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener la Persona");
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }

        /// <summary>
        /// Endpoint para consulta de Persona por Número de Identificación
        /// </summary>
        /// <param name="NumeroIdentificacion"></param>
        /// <returns></returns>
        [HttpGet("{NumeroIdentificacion}")]
        public async Task<ActionResult<Persona>> GetPersonaByNumeroIdentificacion(string NumeroIdentificacion)
        {
            string nombreControlador = ControllerContext.ActionDescriptor.ControllerName;
            try
            {
                _logger.Information("Inicio GetPersonaByNumeroIdentificacion en {NombreControlador}", nombreControlador);
                var Persona = await _PersonaService.GetPersonaByNumeroIdentificacion(NumeroIdentificacion);

                if (Persona == null)
                {
                    _logger.Information("Persona con Número de Identificación {NumeroIdentificacion} no encontrado o eliminado", NumeroIdentificacion);
                    return NotFound($"Persona con Número de Identificación {NumeroIdentificacion} no encontrado o eliminado.");
                }

                return Ok(Persona);
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "Error en la solicitud de Persona");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al obtener la Persona");
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }

        /// <summary>
        /// EndPoint para eliminar Persona por Número de Identificación
        /// </summary>
        /// <param name="NumeroIdentificacion"></param>
        /// <returns></returns>
        [HttpDelete("{NumeroIdentificacion}")]
        public async Task<ActionResult<Persona>> DeletePersona (string NumeroIdentificacion)
        {
            string nombreControlador = ControllerContext.ActionDescriptor.ControllerName;
            try
            {
                _logger.Information("Inicio DeletePersona en {NombreControlador}", nombreControlador);
                var Persona = await _PersonaService.GetPersonaByNumeroIdentificacion(NumeroIdentificacion);

                if (Persona == null)
                {
                    _logger.Information("Persona con Número de  Identificación {NumeroIdentificacion} no encontrado o eliminado", NumeroIdentificacion);
                    return NotFound($"Persona con Número de  Identificación {NumeroIdentificacion} no encontrado o eliminado.");
                }

                var ID = Persona.Id;
                await _PersonaService.DeletePersona(ID);

                return NoContent(); //204 no content
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "Error al eliminar Persona");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al eliminar Persona");
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }

        /// <summary>
        /// EndPoint para actualizar Persona por Número de Identificación
        /// </summary>
        /// <param name="NumeroIdentificacion"></param>
        /// <param name="persona"></param>
        /// <returns></returns>
        [HttpPut("{NumeroIdentificacion}/{Email}")]
        public async Task<ActionResult<Persona>> ActualizarPersona(string NumeroIdentificacion, string Email)
        {
            string nombreControlador = ControllerContext.ActionDescriptor.ControllerName;
            try
            {
                _logger.Information("Inicio ActualizarPersona en {NombreControlador}", nombreControlador);
                var personaExistente = await _PersonaService.GetPersonaByNumeroIdentificacion(NumeroIdentificacion);

                if (personaExistente == null)
                {
                    _logger.Information(" {NumeroIdentificacion} no encontrado o eliminado", NumeroIdentificacion);
                    return NotFound($"Persona con Número de Identificación {NumeroIdentificacion} no encontrado o eliminado.");
                }

                //actualizar las propiedades de persona
                //personaExistente.Nombres = persona.Nombres;
                //personaExistente.Apellidos = persona.Apellidos;
                //personaExistente.NumeroIdentificacion = persona.NumeroIdentificacion;
                personaExistente.Email = Email;
                //personaExistente.TipoIdentificacionID = persona.TipoIdentificacionID;
                //personaExistente.FechaCreacion = persona.FechaCreacion;

                //llamando al servicio 
                var ID = personaExistente.Id;
                await _PersonaService.ActualizaPersona(ID, personaExistente);

                return NoContent(); //204 no content
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "Error al actualizar Persona");
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error al actualizar Persona");
                return StatusCode(500, "Ocurrió un error al procesar su solicitud.");
            }
        }

        /// <summary>
        /// EndPoint para grabar nueva entidad Persona
        /// </summary>
        /// <param name="persona"></param>
        /// <returns></returns>
        [HttpPost()]
        public async Task<ActionResult> GrabarPersona([FromBody] Persona persona)
        {
            string nombreControlador = ControllerContext.ActionDescriptor.ControllerName;
            string NumeroIdentificacion = persona.NumeroIdentificacion;
            try
            {
                _logger.Information("Inicio GrabarPersona en {NombreControlador}", nombreControlador);
                if (persona == null)
                {
                    _logger.Information("La información de la persona no puede ser nula");
                    return BadRequest("La información de la persona no puede ser nula.");
                }

                var personaExistente = await _PersonaService.GetPersonaByNumeroIdentificacion(persona.NumeroIdentificacion);

                if (personaExistente != null)
                {
                    _logger.Information("Persona con número de identificación: {NumeroIdentificacion} ya se encuentra registrada.", NumeroIdentificacion);
                    return Conflict($"Persona con número de identificación: {NumeroIdentificacion} ya se encuentra registrada.");
                }

                var (isSuccess, codigoSecuencia) = await _PersonaService.GrabarPersona(persona);

                if (isSuccess)
                {
                    return CreatedAtAction(nameof(GrabarPersona), new { persona }, new { CodigoSecuencia = codigoSecuencia });
                }

                _logger.Error( "Hubo un error al grabar el usuario");
                return BadRequest("Hubo un error al grabar el usuario.");
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "Error al grabar Persona: {ErrorMessage}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error interno del servidor al grabar Persona: {ErrorMessage}", ex.Message);
                return StatusCode(500, "Ocurrió un error interno del servidor al procesar su solicitud.");
            }
        }


    }
}





