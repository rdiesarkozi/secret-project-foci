using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawScoreDetail
{
    [JsonPropertyName("home")]
    public int? Home { get; set; }
    [JsonPropertyName("away")]
    public int? Away { get; set; }
}