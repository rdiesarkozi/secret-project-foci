using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawTeam
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
    [JsonPropertyName("logo")]
    public string Logo { get; set; } = default!;
    [JsonPropertyName("winner")]
    public bool? Winner { get; set; }
}