using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerPasses
{
    [JsonPropertyName("total")]
    public int? Total { get; set; }

    [JsonPropertyName("key")]
    public int? Key { get; set; }

    [JsonPropertyName("accuracy")]
    public int? Accuracy { get; set; }
}