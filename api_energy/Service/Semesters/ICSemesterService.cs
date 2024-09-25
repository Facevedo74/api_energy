using System.Collections.Generic;
using System.Threading.Tasks;
using api_energy.Models;

namespace api_energy.Service.Semesters
{
    public interface ICSemesterService
    {
        Task<List<CSemester>> GetAllSemesters();
        // Otros métodos...
    }
}
