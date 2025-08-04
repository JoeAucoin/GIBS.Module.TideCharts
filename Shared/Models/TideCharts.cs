using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.TideCharts.Models
{
    [Table("GIBSTideCharts")]
    public class TideCharts : IAuditable
    {
        [Key]
        public int TideChartsId { get; set; }
        public int ModuleId { get; set; }
        public string StationName { get; set; }
        public string StationId { get; set; }
        public string State { get; set; }
        public string TimeZoneCorrection { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Slug { get; set; } // For SEO-friendly URLs
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        [NotMapped]
        public string SearchName { get; set; } // Not mapped, for searching only
    }
}
