using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerSubstitutes
{
    [JsonPropertyName("in")]
    public int? In { get; set; }

    [JsonPropertyName("out")]
    public int? Out { get; set; }

    [JsonPropertyName("bench")]
    public int? Bench { get; set; }
}