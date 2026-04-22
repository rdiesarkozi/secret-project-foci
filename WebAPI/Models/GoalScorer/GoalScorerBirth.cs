using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerBirth
{
    [JsonPropertyName("date")]
    public string Date { get; set; }

    [JsonPropertyName("place")]
    public string Place { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }
}