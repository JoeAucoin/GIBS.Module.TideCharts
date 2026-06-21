using Oqtane.Models;
using Oqtane.Modules;

namespace GIBS.Module.TideCharts
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "TideCharts",
            Description = "Tide Charts Module for Oqtane",
            Version = "1.0.1",
            ServerManagerType = "GIBS.Module.TideCharts.Manager.TideChartsManager, GIBS.Module.TideCharts.Server.Oqtane",
            ReleaseVersions = "1.0.0,1.0.1",
            Dependencies = "GIBS.Module.TideCharts.Shared.Oqtane",
            PackageName = "GIBS.Module.TideCharts" 
        };
    }
}
