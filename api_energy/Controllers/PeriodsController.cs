using api_energy.Models;
using api_energy.Service;
using Microsoft.AspNetCore.Mvc;

namespace api_energy.Controllers
{
    [Route("api/periods")]
    public class PeriodsController : Controller
    {
        private IPeriodsService periodsService;
        private readonly ILogger<PeriodsController> _logger;

        public PeriodsController(IPeriodsService periodsService, ILogger<PeriodsController> logger)
        {
            this.periodsService = periodsService;
            this._logger = logger;
        }


        /// <summary>
        /// Get All Periods.
        /// </summary>
        [HttpGet("AllPeriods")]
        public IActionResult GetAllPeriods()
        {
            return Ok(periodsService.AllPeriods());
        }


        /// <summary>
        /// Get All Periods.
        /// </summary>
        [HttpGet("GetPeriodsById")]
        public IActionResult GetPeriodById(int periodId)
        {
            return Ok(periodsService.GetPeriodById(periodId));
        }

        /// <summary>
        /// Get All Periods.
        /// </summary>
        [HttpGet("GenerateTxT")]
        public IActionResult GenerateTxT(int periodId)
        {
            return Ok(periodsService.GenerateTxT(periodId));
        }


        /// <summary>
        /// Get All Periods.
        /// </summary>
        [HttpGet("export-excel")]
        public IActionResult ExportExcel(int periodId)
        {
            return Ok(periodsService.ExportExcel(periodId));
        }
        /// <summary>
        /// Get All Periods.
        /// </summary>
        [HttpPut("generate-report")]
        public IActionResult GenerateReport([FromBody] string base64)
        {
            periodsService.GenerateReport(base64);
            return Ok("ok");
        }
        

        /// <summary>
        /// Get All Periods.
        /// </summary>
        [HttpGet("getMeasurements")]
        public IActionResult getMeasurements(int id_period, int skip)
        {
            var data = periodsService.getMeasurements(id_period, skip);

            var result = new
            {
                TotalRows = data.TotalRows,
                Measurements = data.Results.ToList()
            };

            return Ok(result);
        }


        /// <summary>
        /// Add new periods with files.
        /// </summary>
        [HttpPost("addPeriod")]
        public async Task<IActionResult> AddPeriod([FromForm] PeriodRequest request)
        {
            if (request.Files == null || request.Files.Count == 0)
            {
                return BadRequest("No files received.");
            }

            await periodsService.AddPeriodFilesAsync(request.Name, request.Files);

            return Ok(new { Message = "Files uploaded successfully." });
        }

        public class PeriodRequest
        {
            public string Name { get; set; }
            public List<IFormFile> Files { get; set; }
        }
    }
}
