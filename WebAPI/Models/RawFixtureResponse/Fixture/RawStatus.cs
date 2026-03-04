using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawStatus
{
    [JsonPropertyName("long")]
    public string Long { get; set; } = default!;
    [JsonPropertyName("short")]
    public string Short { get; set; } = default!;
    [JsonPropertyName("elapsed")]
    public int? Elapsed { get; set; }
    [JsonPropertyName("extra")]
    public int? Extra { get; set; }
}