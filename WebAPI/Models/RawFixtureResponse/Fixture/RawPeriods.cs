using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawPeriods
{
    [JsonPropertyName("first")]
    public long? First { get; set; }
    
    [JsonPropertyName("second")]
    public long? Second { get; set; }
}