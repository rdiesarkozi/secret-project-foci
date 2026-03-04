using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawGoals
{
    [JsonPropertyName("home")]
    public int? Home { get; set; }
    
    [JsonPropertyName("away")]
    public int? Away { get; set; }
}