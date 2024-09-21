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
    }
}
