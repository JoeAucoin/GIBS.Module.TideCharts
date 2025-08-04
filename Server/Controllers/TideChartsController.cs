using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Oqtane.Shared;
using Oqtane.Enums;
using GIBS.Module.TideCharts.Repository;
using Oqtane.Infrastructure;
using GIBS.Module.TideCharts.Services;
using GIBS.Module.TideCharts.Shared.Models;

namespace GIBS.Module.TideCharts.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class TideChartsController : Controller
    {
        private readonly ITideChartsService _TideChartsService;
        private readonly ILogManager _logger;

        public TideChartsController(ITideChartsService TideChartsService, ILogManager logger)
        {
            _TideChartsService = TideChartsService;
            _logger = logger;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.TideCharts>> Get(string moduleid)
        {
            return await _TideChartsService.GetTideChartssAsync(int.Parse(moduleid));
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.TideCharts> Get(int id)
        {
            return await _TideChartsService.GetTideChartsAsync(id, int.Parse(Request.Query["moduleid"]));
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.TideCharts> Post([FromBody] Models.TideCharts TideCharts)
        {
            if (ModelState.IsValid)
            {
                TideCharts = await _TideChartsService.AddTideChartsAsync(TideCharts);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "TideCharts Added {TideCharts}", TideCharts);
            }
            return TideCharts;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.TideCharts> Put(int id, [FromBody] Models.TideCharts TideCharts)
        {
            if (ModelState.IsValid)
            {
                TideCharts = await _TideChartsService.UpdateTideChartsAsync(TideCharts);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "TideCharts Updated {TideCharts}", TideCharts);
            }
            return TideCharts;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id)
        {
            await _TideChartsService.DeleteTideChartsAsync(id, int.Parse(Request.Query["moduleid"]));
            _logger.Log(LogLevel.Information, this, LogFunction.Delete, "TideCharts Deleted {TideChartsId}", id);
        }

        // GET: api/<controller>/stations
        [HttpGet("stations")]
        [AllowAnonymous] // NOAA data is public
        public async Task<IEnumerable<NOAAStation>> GetNOAAStations()
        {
            return await _TideChartsService.GetNOAAStationsAsync();
        }

        // GET: api/<controller>/predictions/stationId/days
        [HttpGet("predictions/{stationId}/{days}")]
        [AllowAnonymous]
        public async Task<IEnumerable<Prediction>> GetTidePredictions(string stationId, int days)
        {
            return await _TideChartsService.GetTidePredictionsAsync(stationId, days);
        }
    }
}