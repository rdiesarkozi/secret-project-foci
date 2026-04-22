using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerLeague
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("logo")]
    public string Logo { get; set; }

    [JsonPropertyName("flag")]
    public string Flag { get; set; }

    [JsonPropertyName("season")]
    public int? Season { get; set; }
}