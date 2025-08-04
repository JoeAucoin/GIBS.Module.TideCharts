using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GIBS.Module.TideCharts.Shared.Models
{
    public class PredictionsResponse
    {
        [JsonPropertyName("predictions")]
        public List<Prediction> Predictions { get; set; }
    }
}