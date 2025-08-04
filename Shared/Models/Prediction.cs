using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace GIBS.Module.TideCharts.Shared.Models
{
    public class Prediction
    {
        [JsonPropertyName("t")]
        public string T { get; set; } // Time string from API (e.g., "2025-05-26 12:00")

        [JsonPropertyName("v")]
        public string V { get; set; } // Value string from API (e.g., "5.213")

        [JsonPropertyName("type")]
        public string Type { get; set; } // H for High, L for Low

        // Helper properties for client-side use
        [JsonIgnore]
        public DateTime PredictedDateTime => DateTime.ParseExact(T, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

        [JsonIgnore]
        public double PredictedValue => double.Parse(V, CultureInfo.InvariantCulture);
    }
}