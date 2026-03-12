using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawPaging
{
    [JsonPropertyName("current")]
    public int Current { get; set; }
    
    [JsonPropertyName("total")]
    public int Total { get; set; }
}