using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawLeague
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
    [JsonPropertyName("country")]
    public string Country { get; set; } = default!;
    [JsonPropertyName("logo")]
    public string Logo { get; set; } = default!;
    [JsonPropertyName("flag")]
    public string Flag { get; set; } = default!;
    [JsonPropertyName("season")]
    public int Season { get; set; }
    [JsonPropertyName("round")]
    public string Round { get; set; } = default!;
    [JsonPropertyName("standings")]
    public bool Standings { get; set; }
}