using System.Collections.Generic;
using System.Threading.Tasks;
using api_energy.Models;
using api_energy.Models.DTOs;

public interface IDatabaseService
{
    Task<List<Database>> GetAllDatabases();
    Task<Database> GetDatabaseById(int id);
    Task AddDatabase(Database database);
    //Task<Database> UpdateDatabase(int id, Database updatedData);

    
  //  Task<Database> UpdateDatabaseAsync(Database updatedData);
    // Nuevo método para actualizar con UpdateDatabaseDto
    Task<Database> UpdateDatabaseAsync(UpdateDatabaseDto updateDto);



    Task<bool> DeactivateDatabase(int id); // Nueva firma de método
    Task DeleteDatabase(int id);
    Task<List<Database>> GetDatabasesBySemesterId(int semesterId);
    Task AddDatabaseRange(List<Database> databases); // Nuevo método para carga masiva

    Task<string> UploadExcelFile(IFormFile file, int semesterId);
}
