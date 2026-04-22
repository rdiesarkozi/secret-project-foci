using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerDuels
{
    [JsonPropertyName("total")]
    public int? Total { get; set; }

    [JsonPropertyName("won")]
    public int? Won { get; set; }
}