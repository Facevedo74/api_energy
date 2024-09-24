using System.Collections.Generic;
using System.Threading.Tasks;
using api_energy.Models;
using api_energy.Models.DTOs;

public interface IDatabaseService
{
    Task<List<Database>> GetAllDatabases();
    Task<Database> GetDatabaseById(int id);
    Task AddDatabase(Database database);
    Task<UpdateDatabaseDto> UpdateDatabaseAsync(UpdateDatabaseDto updateDto);
    Task<bool> DeactivateDatabase(int id);
    Task DeleteDatabase(int id);
    Task<List<Database>> GetDatabasesBySemesterId(int semesterId);
    Task AddDatabaseRange(List<Database> databases); 

    Task<string> UploadExcelFile(IFormFile file, int semesterId);
}
