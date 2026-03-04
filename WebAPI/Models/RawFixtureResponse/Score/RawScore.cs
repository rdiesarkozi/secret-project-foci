using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawScore
{
    [JsonPropertyName("halftime")]
    public RawScoreDetail HalfTime { get; set; } = new();
    [JsonPropertyName("fulltime")]
    public RawScoreDetail FullTime { get; set; } = new();
    [JsonPropertyName("extratime")]
    public RawScoreDetail ExtraTime { get; set; } = new();
    [JsonPropertyName("penalty")]
    public RawScoreDetail Penalty { get; set; } = new();
}