using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerGoals
{
    [JsonPropertyName("total")]
    public int? Total { get; set; }

    [JsonPropertyName("conceded")]
    public int? Conceded { get; set; }

    [JsonPropertyName("assists")]
    public int? Assists { get; set; }

    [JsonPropertyName("saves")]
    public int? Saves { get; set; }
}