using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api_energy.Models;
using api_energy.Context;

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
