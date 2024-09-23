using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using api_energy.Models;
using api_energy.Services;
using OfficeOpenXml;

[Route("api/[controller]")]
[ApiController]
public class DatabaseController : Controller
{
    private readonly IDatabaseService _service; // Cambia a la interfaz

    public DatabaseController(IDatabaseService service) // Cambia a IDatabaseService
    {
        _service = service;
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
        // Validación del archivo
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        var databases = new List<Database>();

        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            using (var package = new ExcelPackage(stream))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Obtener la primera hoja
                var rowCount = worksheet.Dimension.Rows;

                // Recorrer las filas del Excel
                for (int row = 2; row <= rowCount; row++) // Comenzar desde 2 para omitir encabezados
                {
                    var database = new Database
                    {
                        Id_Semester = semesterId,
                        NIS = long.Parse(worksheet.Cells[row, 1].Text),
                        NombreArchivo = worksheet.Cells[row, 2].Text,
                        Medidor = worksheet.Cells[row, 3].Text,
                        Provincia = worksheet.Cells[row, 4].Text,
                        Corregimiento = worksheet.Cells[row, 5].Text,
                        Categoria_Tarifaria = worksheet.Cells[row, 6].Text,
                        Departamento = worksheet.Cells[row, 7].Text
                    };

                    databases.Add(database);
                }
            }
        }

        // Llamar al servicio para agregar la lista
        await _service.AddDatabaseRange(databases);
        return Ok(new { message = "Carga masiva realizada con éxito." });

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
