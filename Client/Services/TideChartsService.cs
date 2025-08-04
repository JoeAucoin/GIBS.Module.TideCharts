using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using GIBS.Module.TideCharts.Shared.Models;
using Oqtane.Services;
using Oqtane.Shared;

namespace GIBS.Module.TideCharts.Services
{
    public class TideChartsService : ServiceBase, ITideChartsService
    {
        public TideChartsService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("TideCharts");

        public async Task<List<Models.TideCharts>> GetTideChartssAsync(int ModuleId)
        {
            List<Models.TideCharts> TideChartss = await GetJsonAsync<List<Models.TideCharts>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.TideCharts>().ToList());
            return TideChartss.OrderBy(item => item.StationName).ToList();
        }

        public async Task<Models.TideCharts> GetTideChartsAsync(int TideChartsId, int ModuleId)
        {
            return await GetJsonAsync<Models.TideCharts>(CreateAuthorizationPolicyUrl($"{Apiurl}/{TideChartsId}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.TideCharts> AddTideChartsAsync(Models.TideCharts TideCharts)
        {
            return await PostJsonAsync<Models.TideCharts>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, TideCharts.ModuleId), TideCharts);
        }

        public async Task<Models.TideCharts> UpdateTideChartsAsync(Models.TideCharts TideCharts)
        {
            return await PutJsonAsync<Models.TideCharts>(CreateAuthorizationPolicyUrl($"{Apiurl}/{TideCharts.TideChartsId}", EntityNames.Module, TideCharts.ModuleId), TideCharts);
        }

        public async Task DeleteTideChartsAsync(int TideChartsId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{TideChartsId}?moduleid={ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<List<NOAAStation>> GetNOAAStationsAsync()
        {
            return await GetJsonAsync<List<NOAAStation>>($"{Apiurl}/stations");
        }

        public async Task<List<Prediction>> GetTidePredictionsAsync(string stationId, int days)
        {
            if (string.IsNullOrEmpty(stationId) || days <= 0) return new List<Prediction>();
            return await GetJsonAsync<List<Prediction>>($"{Apiurl}/predictions/{stationId}/{days}");
        }
    }
}