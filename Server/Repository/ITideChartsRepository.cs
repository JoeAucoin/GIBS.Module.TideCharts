using Oqtane.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GIBS.Module.TideCharts.Repository
{
    public interface ITideChartsRepository
    {
        IEnumerable<Models.TideCharts> GetTideChartss(int ModuleId);
        Models.TideCharts GetTideCharts(int TideChartsId);
        Models.TideCharts GetTideCharts(int TideChartsId, bool tracking);
        Models.TideCharts AddTideCharts(Models.TideCharts TideCharts);
        Models.TideCharts UpdateTideCharts(Models.TideCharts TideCharts);
        void DeleteTideCharts(int TideChartsId);

    }
}
