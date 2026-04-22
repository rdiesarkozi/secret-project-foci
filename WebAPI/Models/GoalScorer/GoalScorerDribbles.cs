using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerDribbles
{
    [JsonPropertyName("attempts")]
    public int? Attempts { get; set; }

    [JsonPropertyName("success")]
    public int? Success { get; set; }

    [JsonPropertyName("past")]
    public int? Past { get; set; }
}