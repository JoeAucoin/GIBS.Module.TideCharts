using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace GIBS.Module.TideCharts.Repository
{
    public class TideChartsRepository : ITideChartsRepository, ITransientService
    {
        private readonly IDbContextFactory<TideChartsContext> _factory;

        public TideChartsRepository(IDbContextFactory<TideChartsContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.TideCharts> GetTideChartss(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.TideCharts.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.TideCharts GetTideCharts(int TideChartsId)
        {
            return GetTideCharts(TideChartsId, true);
        }

        public Models.TideCharts GetTideCharts(int TideChartsId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.TideCharts.Find(TideChartsId);
            }
            else
            {
                return db.TideCharts.AsNoTracking().FirstOrDefault(item => item.TideChartsId == TideChartsId);
            }
        }

        public Models.TideCharts AddTideCharts(Models.TideCharts TideCharts)
        {
            using var db = _factory.CreateDbContext();
            db.TideCharts.Add(TideCharts);
            db.SaveChanges();
            return TideCharts;
        }

        public Models.TideCharts UpdateTideCharts(Models.TideCharts TideCharts)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(TideCharts).State = EntityState.Modified;
            db.SaveChanges();
            return TideCharts;
        }

        public void DeleteTideCharts(int TideChartsId)
        {
            using var db = _factory.CreateDbContext();
            Models.TideCharts TideCharts = db.TideCharts.Find(TideChartsId);
            db.TideCharts.Remove(TideCharts);
            db.SaveChanges();
        }
    }
}
