using System;
using GIBS.Module.TideCharts.Repository;
using GIBS.Module.TideCharts.Shared.Models;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Enums;
using Oqtane.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace GIBS.Module.TideCharts.Manager
{
    public class TideChartsManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly ITideChartsRepository _TideChartsRepository;
        private readonly IDBContextDependencies _DBContextDependencies;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string NOAA_STATIONS_API_URL = "https://api.tidesandcurrents.noaa.gov/mdapi/prod/webapi/stations.json";

        public TideChartsManager(ITideChartsRepository TideChartsRepository, IDBContextDependencies DBContextDependencies, IHttpClientFactory httpClientFactory)
        {
            _TideChartsRepository = TideChartsRepository;
            _DBContextDependencies = DBContextDependencies;
            _httpClientFactory = httpClientFactory;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new TideChartsContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new TideChartsContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            List<Models.TideCharts> tideCharts = _TideChartsRepository.GetTideChartss(module.ModuleId).ToList();
            if (tideCharts != null)
            {
                content = JsonSerializer.Serialize(tideCharts);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.TideCharts> tideCharts = null;
            if (!string.IsNullOrEmpty(content))
            {
                tideCharts = JsonSerializer.Deserialize<List<Models.TideCharts>>(content);
            }
            if (tideCharts != null)
            {
                foreach (var tideChart in tideCharts)
                {
                    var newTideChart = new Models.TideCharts
                    {
                        ModuleId = module.ModuleId,
                        StationName = tideChart.StationName,
                        StationId = tideChart.StationId,
                        State = tideChart.State,
                        TimeZoneCorrection = tideChart.TimeZoneCorrection,
                        Latitude = tideChart.Latitude,
                        Longitude = tideChart.Longitude,
                        Slug = tideChart.Slug,
                        IsActive = tideChart.IsActive
                    };
                    _TideChartsRepository.AddTideCharts(newTideChart);
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, System.DateTime lastIndexedOn)
        {
            var searchContentList = new List<SearchContent>();
            var tideCharts = _TideChartsRepository.GetTideChartss(pageModule.ModuleId);

            foreach (var tideChart in tideCharts)
            {
                if (tideChart.ModifiedOn >= lastIndexedOn)
                {
                    searchContentList.Add(new SearchContent
                    {
                        EntityName = "GIBSTideCharts",
                        EntityId = tideChart.TideChartsId.ToString(),
                        Title = tideChart.StationName,
                        Body = $"{tideChart.StationName}, {tideChart.State}",
                        ContentModifiedBy = tideChart.ModifiedBy,
                        ContentModifiedOn = tideChart.ModifiedOn
                    });
                }
            }

            return Task.FromResult(searchContentList);
        }

        public async Task<List<NOAAStation>> GetNOAAStationsAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetFromJsonAsync<NOAAStationsResponse>(NOAA_STATIONS_API_URL);
                return response?.Stations ?? new List<NOAAStation>();
            }
            catch // Consider more specific exception handling and logging
            {
                return new List<NOAAStation>();
            }
        }
    }
}