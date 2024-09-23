using System.Collections.Generic;
using System.Threading.Tasks;
using api_energy.Models;

public interface IDatabaseService
{
    Task<List<Database>> GetAllDatabases();
    Task<Database> GetDatabaseById(int id);
    Task AddDatabase(Database database);
    Task UpdateDatabase(Database database);
    Task DeleteDatabase(int id);
    Task<List<Database>> GetDatabasesBySemesterId(int semesterId);
    Task AddDatabaseRange(List<Database> databases); // Nuevo método para carga masiva

}
