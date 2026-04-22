using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerTackles
{
    [JsonPropertyName("total")]
    public int? Total { get; set; }

    [JsonPropertyName("blocks")]
    public int? Blocks { get; set; }

    [JsonPropertyName("interceptions")]
    public int? Interceptions { get; set; }
}