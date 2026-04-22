using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerPenalty
{
    [JsonPropertyName("won")]
    public int? Won { get; set; }

    [JsonPropertyName("commited")]
    public int? Commited { get; set; }

    [JsonPropertyName("scored")]
    public int? Scored { get; set; }

    [JsonPropertyName("missed")]
    public int? Missed { get; set; }

    [JsonPropertyName("saved")]
    public int? Saved { get; set; }
}