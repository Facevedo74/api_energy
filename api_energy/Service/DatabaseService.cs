using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api_energy.Models;
using api_energy.Context;
using OfficeOpenXml;

namespace api_energy.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly AppDbContext _context;

        public DatabaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Database>> GetAllDatabases()
        {
            return await _context.Databases.Include(d => d.Semester).ToListAsync();
        }

        public async Task<List<Database>> GetDatabasesBySemesterId(int semesterId)
        {
            return await _context.Databases
                .Where(d => d.Id_Semester == semesterId)
                .Include(d => d.Semester) // Si necesitas incluir información de la tabla Semester
                .ToListAsync();
        }

        public async Task AddDatabaseRange(List<Database> databases)
        {
            await _context.Databases.AddRangeAsync(databases);
            await _context.SaveChangesAsync();
        }



        public async Task<Database> GetDatabaseById(int id)
        {
            return await _context.Databases.Include(d => d.Semester).FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task AddDatabase(Database database)
        {
            _context.Databases.Add(database);
            await _context.SaveChangesAsync();
        }

        public async Task<string> UploadExcelFile(IFormFile file, int semesterId)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file uploaded.");
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

            // Llamar al método para agregar la lista
            await AddDatabaseRange(databases);
            return "Carga masiva realizada con éxito.";
        }

        public async Task UpdateDatabase(Database database)
        {
            _context.Databases.Update(database);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDatabase(int id)
        {
            var database = await _context.Databases.FindAsync(id);
            if (database != null)
            {
                _context.Databases.Remove(database);
                await _context.SaveChangesAsync();
            }
        }
    }
}
