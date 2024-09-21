using System;
using api_energy.Models;

namespace api_energy.Service
{
    public interface IPeriodsService
    {
        public List<Periods> AllPeriods();
        public string GenerateTxT(int periodId);
        public string ExportExcel(int periodId);

        public (int TotalRows, List<Measurements> Results) getMeasurements(int id_period, int skip);
    }
}

