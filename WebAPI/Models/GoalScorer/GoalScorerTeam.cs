using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerTeam
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("logo")]
    public string Logo { get; set; }
}