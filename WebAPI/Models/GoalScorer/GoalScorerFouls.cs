using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerFouls
{
    [JsonPropertyName("drawn")]
    public int? Drawn { get; set; }

    [JsonPropertyName("committed")]
    public int? Committed { get; set; }
}