using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawParameters
{
    [JsonPropertyName("league")]
    public string League { get; set; } = default!;
    
    [JsonPropertyName("season")]
    public string Season { get; set; } = default!;
}