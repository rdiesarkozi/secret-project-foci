using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerShots
{
    [JsonPropertyName("total")]
    public int? Total { get; set; }

    [JsonPropertyName("on")]
    public int? On { get; set; }
}