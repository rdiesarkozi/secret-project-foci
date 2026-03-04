using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawTeams
{
    [JsonPropertyName("home")]
    public RawTeam Home { get; set; } = new();
    [JsonPropertyName("away")]
    public RawTeam Away { get; set; } = new();
}