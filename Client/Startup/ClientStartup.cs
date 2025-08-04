using Microsoft.Extensions.DependencyInjection;
using Oqtane.Services;
using GIBS.Module.TideCharts.Services;

namespace GIBS.Module.TideCharts.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ITideChartsService, TideChartsService>();
        }
    }
}
