using System;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using api_energy.Models;
using api_energy.Service;
using System.Text;
using System.Security.Claims;

namespace api_energy.Controllers
{
    [Route("api/energy")]
    public class UserController : Controller
    {
        private IUserService userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            this.userService = userService;
            this._logger = logger;
        }

        /// <summary>
        /// Gets the state of the helper.
        /// </summary>
        /// <remarks>
        /// This method returns the state of the helper.
        /// </remarks>
        /// <returns>1 if exist elements</returns>
        [HttpGet("HelperStatus")]
        [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult HelperStatus()
        {
            try
            {
                return Ok(userService.HelperStatus());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Get User Info.
        /// </summary>
        [HttpGet("user-info")]
        public IActionResult GetUserInfo()
        {
            var username = "jhond";//User.FindFirst(ClaimTypes.NameIdentifier)?.Value 
            return Ok(userService.GetUser(username));
        }



        /// <summary>
        /// LeerArchivo.
        /// </summary>
        [HttpGet("leer")]
        public IActionResult LeerArchivo()
        {
            try
            {
                var _rutaDelArchivo = "D:/Descargas/ARCHIVOS PRUEBA AUTOMATIZACION/ARCHIVOS PRUEBA AUTOMATIZACION/AT174570.std";
                // Verifica si el archivo existe


                try
                {
                    if (!System.IO.File.Exists(_rutaDelArchivo))
                    {
                        return NotFound("El archivo no se encontró.");
                    }

                    // Intenta leer el archivo con diferentes codificaciones
                    string contenido;
                    Encoding[] codificaciones = {
                    Encoding.UTF8,
                    Encoding.ASCII,
                    Encoding.Unicode,
                    Encoding.UTF32,
                    Encoding.BigEndianUnicode,
                    Encoding.Default,
                    Encoding.GetEncoding("ISO-8859-1"),
                };
                    foreach (var codificacion in codificaciones)
                    {
                        try
                        {
                            using (StreamReader lector = new StreamReader(_rutaDelArchivo, codificacion))
                            {
                                contenido = lector.ReadToEnd();
                                // Verifica si el contenido parece texto legible
                                if (!contenido.Contains('\0')) // Verifica si el contenido no tiene caracteres nulos
                                {
                                    return Ok(contenido);
                                }
                            }
                        }
                        catch
                        {
                            // Ignora errores de codificación y prueba la siguiente
                        }
                    }

                    return StatusCode(415, "No se pudo leer el archivo con las codificaciones intentadas.");
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al leer el archivo: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                // Maneja otros errores (como problemas de permisos)
                return StatusCode(500, $"Error al leer el archivo: {ex.Message}");
            }
        }


        /// <summary>
        /// Get All Employees.
        /// </summary>
        /// <remarks>
        /// This method returns the list of all employees
        /// </remarks>
        /// <returns>This method returns the list of all employees</returns>
        [Authorize]
        [HttpGet()]
        [ProducesResponseType(typeof(List<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllUser()
        {
            try
            {
                return Ok(userService.GetAllUser());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }



        

        /// <summary>
        /// Insert new employee
        /// </summary>
        /// <remarks>
        /// This method add new employee.
        /// </remarks>
        /// <returns>This method returns the employee item</returns>
        [Authorize]
        [HttpPost()]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddUser([FromBody] User user)
        {
            try
            {
                return Ok(userService.AddUser(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Update employee
        /// </summary>
        /// <remarks>
        /// This method update the data to the employee.
        /// </remarks>
        /// <returns>This method returns void</returns>
        [Authorize]
        [HttpPut()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateUser([FromBody] User user)
        {
            try
            {
                userService.UpdateUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }

        /// <summary>
        /// Delete employere
        /// </summary>
        /// <remarks>
        /// This method delete the data to the employee.
        /// </remarks>
        /// <returns>This method returns void</returns>
        [Authorize]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                userService.DeleteUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest();
            }
        }
    }
}

