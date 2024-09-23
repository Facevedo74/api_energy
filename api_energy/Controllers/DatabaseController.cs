using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using api_energy.Models;
using api_energy.Services;
using OfficeOpenXml;
using Microsoft.Extensions.Logging;
using api_energy.Controllers;

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

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, Database database)
    {
        if (id != database.Id)
        {
            return BadRequest();
        }
        await _service.UpdateDatabase(database);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await _service.DeleteDatabase(id);
        return NoContent();
    }
}
