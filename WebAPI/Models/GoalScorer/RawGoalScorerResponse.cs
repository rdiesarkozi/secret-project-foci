using System.Text.Json.Serialization;

namespace WebAPI.Models;

public class RawGoalScorerResponse
{
    [JsonPropertyName("get")]
    public string Get { get; set; }

    [JsonPropertyName("results")]
    public int Results { get; set; }

    [JsonPropertyName("response")]
    public List<GoalScorerData> Response { get; set; } = new();
}