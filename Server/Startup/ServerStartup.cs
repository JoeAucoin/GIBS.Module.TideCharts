using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using GIBS.Module.TideCharts.Repository;
using GIBS.Module.TideCharts.Services;

namespace GIBS.Module.TideCharts.Startup
{
    public class ServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // not implemented
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ITideChartsService, ServerTideChartsService>();
            services.AddDbContextFactory<TideChartsContext>(opt => { }, ServiceLifetime.Transient);
        }
    }
}
