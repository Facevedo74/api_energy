using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using api_energy.Models;
using api_energy.Service.DataBase;
using OfficeOpenXml;
using Microsoft.Extensions.Logging;
using api_energy.Controllers;
using api_energy.Models.DTOs;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController : Controller
{
    private readonly IDatabaseService _service; 
    private readonly ILogger<DatabaseController> _logger;
    public DatabaseController(IDatabaseService service, ILogger<DatabaseController> logger) 
    {
        _service = service;
        this._logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<Database>>> GetAll()
    {
        var databases = await _service.GetAllDatabases();
        return Ok(databases);
    }

    [HttpGet("by-semester/{semesterId}")]
    public async Task<ActionResult<List<Database>>> GetBySemesterId(int semesterId)
    {
        var databases = await _service.GetDatabasesBySemesterId(semesterId);
        
        return Ok(databases);
    }

    [HttpPost("upload-excel")]
    public async Task<ActionResult> UploadExcel(IFormFile file, [FromForm] int semesterId)
    {
        try
        {
            var message = await _service.UploadExcelFile(file, semesterId);
            return Ok(new { message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al subir el archivo Excel");
            return BadRequest("Ocurrió un error al procesar el archivo.");
        }
    }





    [HttpGet("{id}")]
    public async Task<ActionResult<Database>> GetById(int id)
    {
        var database = await _service.GetDatabaseById(id);
        if (database == null)
        {
            return NotFound();
        }
        return Ok(database);
    }

    [HttpPost]
    public async Task<ActionResult> Create(Database database)
    {
        await _service.AddDatabase(database);
        return CreatedAtAction(nameof(GetById), new { id = database.Id }, database);
    }


    // Método para desactivar un registro de la base de datos por su ID
    [HttpPut("deactivate/{id}")]
    public async Task<ActionResult> DeactivateDatabase(int id)
    {
        var result = await _service.DeactivateDatabase(id); // Cambia el estado a inactivo
        if (!result)
        {
            return NotFound(); // Si no se encuentra el registro
        }

        // Retorna un mensaje de éxito
        return Ok(new { message = "El registro ha sido desactivado exitosamente." });
    }

    /*
    [HttpPut("update")]
    public async Task<IActionResult> UpdateDatabase( [FromBody] Database updatedData)
    {

        try
        {
            var updatedDatabase = await _service.UpdateDatabaseAsync(updatedData);
            return Ok(updatedDatabase); // Devuelve el objeto actualizado
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }
    */

    [HttpPut("update")]
    public async Task<IActionResult> UpdateDatabase([FromBody] UpdateDatabaseDto updateDto)
    {
        try
        {
            // Valida que el ID del DTO esté presente
            if (updateDto.Id <= 0)
            {
                return BadRequest("El ID es requerido.");
            }

            var updatedDatabase = await _service.UpdateDatabaseAsync(updateDto);
            return Ok(updatedDatabase); // Devuelve el objeto actualizado
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (Exception)
        {
            return StatusCode(500, "Internal server error");
        }
    }


}
