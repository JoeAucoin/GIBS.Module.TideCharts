using GIBS.Module.TideCharts.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIBS.Module.TideCharts.Services
{
    public interface ITideChartsService 
    {
        Task<List<Models.TideCharts>> GetTideChartssAsync(int ModuleId);

        Task<Models.TideCharts> GetTideChartsAsync(int TideChartsId, int ModuleId);

        Task<Models.TideCharts> AddTideChartsAsync(Models.TideCharts TideCharts);

        Task<Models.TideCharts> UpdateTideChartsAsync(Models.TideCharts TideCharts);

        Task DeleteTideChartsAsync(int TideChartsId, int ModuleId);
        Task<List<NOAAStation>> GetNOAAStationsAsync();
        Task<List<Prediction>> GetTidePredictionsAsync(string stationId, int days);
    }
}
