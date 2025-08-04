using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GIBS.Module.TideCharts.Shared.Models
{
    public class NOAAStation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lng")]
        public double Lng { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("timezonecorr")]
        public int Timezonecorr { get; set; }

        public string DisplayName => !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(State) ? $"{Name}, {State}" : Name ?? Id ?? "[Unknown Station]";
    }

    public class NOAAStationsResponse
    {
        [JsonPropertyName("stations")]
        public List<NOAAStation> Stations { get; set; }
    }
}
