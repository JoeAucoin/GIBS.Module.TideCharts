using GIBS.Module.TideCharts.Repository;
using GIBS.Module.TideCharts.Shared.Models;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Modules;
using Oqtane.Security;
using Oqtane.Shared;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace GIBS.Module.TideCharts.Services
{
    public class ServerTideChartsService : ITideChartsService, ITransientService
    {
        private readonly ITideChartsRepository _TideChartsRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;
        private readonly IHttpClientFactory _httpClientFactory;
        private const string NOAA_STATIONS_API_URL = "https://api.tidesandcurrents.noaa.gov/mdapi/prod/webapi/stations.json";
        private const string NOAA_API_BASE_URL = "https://api.tidesandcurrents.noaa.gov/api/prod/datagetter";

        public ServerTideChartsService(ITideChartsRepository TideChartsRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor, IHttpClientFactory httpClientFactory)
        {
            _TideChartsRepository = TideChartsRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
            _httpClientFactory = httpClientFactory;
        }

        public Task<List<Models.TideCharts>> GetTideChartssAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_TideChartsRepository.GetTideChartss(ModuleId).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized TideCharts Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.TideCharts> GetTideChartsAsync(int TideChartsId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_TideChartsRepository.GetTideCharts(TideChartsId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized TideCharts Get Attempt {TideChartsId} {ModuleId}", TideChartsId, ModuleId);
                return null;
            }
        }

        public Task<Models.TideCharts> AddTideChartsAsync(Models.TideCharts TideCharts)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, TideCharts.ModuleId, PermissionNames.Edit))
            {
                TideCharts = _TideChartsRepository.AddTideCharts(TideCharts);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "TideCharts Added {TideCharts}", TideCharts);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized TideCharts Add Attempt {TideCharts}", TideCharts);
                TideCharts = null;
            }
            return Task.FromResult(TideCharts);
        }

        public Task<Models.TideCharts> UpdateTideChartsAsync(Models.TideCharts TideCharts)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, TideCharts.ModuleId, PermissionNames.Edit))
            {
                TideCharts = _TideChartsRepository.UpdateTideCharts(TideCharts);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "TideCharts Updated {TideCharts}", TideCharts);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized TideCharts Update Attempt {TideCharts}", TideCharts);
                TideCharts = null;
            }
            return Task.FromResult(TideCharts);
        }

        public Task DeleteTideChartsAsync(int TideChartsId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _TideChartsRepository.DeleteTideCharts(TideChartsId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "TideCharts Deleted {TideChartsId}", TideChartsId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized TideCharts Delete Attempt {TideChartsId} {ModuleId}", TideChartsId, ModuleId);
            }
            return Task.CompletedTask;
        }

        public async Task<List<NOAAStation>> GetNOAAStationsAsync()
        {
            // This data is public, so no specific permissions check is needed.
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.GetFromJsonAsync<NOAAStationsResponse>(NOAA_STATIONS_API_URL);
                return response?.Stations ?? new List<NOAAStation>();
            }
            catch (System.Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, ex, "Error Getting NOAA Stations");
                return new List<NOAAStation>();
            }
        }

        public async Task<List<Prediction>> GetTidePredictionsAsync(string stationId, int days)
        {
            if (string.IsNullOrEmpty(stationId) || days <= 0)
            {
                return new List<Prediction>();
            }

            try
            {
                DateTime startDate = DateTime.Today;
                DateTime endDate = startDate.AddDays(days);

                string beginDateFormatted = startDate.ToString("yyyyMMdd HH:mm", CultureInfo.InvariantCulture);
                string endDateFormatted = endDate.ToString("yyyyMMdd HH:mm", CultureInfo.InvariantCulture);

                string apiUrl = $"{NOAA_API_BASE_URL}?" +
                                $"product=predictions&" +
                                $"datum=MLLW&" +
                                $"station={stationId}&" +
                                $"begin_date={beginDateFormatted}&" +
                                $"end_date={endDateFormatted}&" +
                                $"interval=hilo&" +
                                $"time_zone=lst_ldt&" +
                                $"units=english&" +
                                $"application=GIBS.Oqtane.TideCharts&" +
                                $"format=json";

                var client = _httpClientFactory.CreateClient();
                var response = await client.GetFromJsonAsync<PredictionsResponse>(apiUrl);
                return response?.Predictions ?? new List<Prediction>();
            }
            catch (System.Exception ex)
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Read, ex, "Error Getting NOAA Tide Predictions for Station {StationId}", stationId);
                return new List<Prediction>();
            }
        }
    }
}