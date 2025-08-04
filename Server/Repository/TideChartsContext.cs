using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;

namespace GIBS.Module.TideCharts.Repository
{
    public class TideChartsContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.TideCharts> TideCharts { get; set; }

        public TideChartsContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.TideCharts>().ToTable(ActiveDatabase.RewriteName("GIBSTideCharts"));
        }
    }
}
