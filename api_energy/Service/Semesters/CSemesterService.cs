using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using api_energy.Models;
using api_energy.Context;

namespace api_energy.Service.Semesters
{
    public class CSemesterService : ICSemesterService
    {
        private readonly AppDbContext _context;

        public CSemesterService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CSemester>> GetAllSemesters()
        {
            try
            {
                return await _context.CSemesters.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los semestres", ex);
            }
        }

        // Otros métodos...
    }
}
