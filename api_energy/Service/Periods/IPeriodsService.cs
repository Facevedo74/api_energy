using System;
using api_energy.Models;

namespace api_energy.Service
{
    public interface IPeriodsService
    {
        public List<Periods> AllPeriods();
        public Periods GetPeriodById(int periodId);
        public string GenerateTxT(int periodId);
        public string ExportExcel(int periodId);

        public (int TotalRows, List<Measurements> Results) getMeasurements(int id_period, int skip);

        Task AddPeriodFilesAsync(string name, List<IFormFile> files);
        Task AditFilesPeriod(int id, List<IFormFile> files);
        void GenerateReport(string base64String);

    }
}

