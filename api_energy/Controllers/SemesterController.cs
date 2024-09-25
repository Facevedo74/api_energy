using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using api_energy.Models;
using api_energy.Service.Semesters;

namespace api_energy.Controllers
{
    [Route("api/semesters")]
    [ApiController]
    public class SemestersController : ControllerBase
    {
        private readonly ILogger<SemestersController> _logger;
        private readonly ICSemesterService _service;

        public SemestersController(ICSemesterService service, ILogger<SemestersController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSemesters()
        {
            try
            {
                var semesters = await _service.GetAllSemesters();
                return Ok(semesters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, "Error al obtener los semestres.");
            }
        }

        // Otros métodos...
    }
}
