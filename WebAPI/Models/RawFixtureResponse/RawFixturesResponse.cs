using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawFixturesResponse
{
    [JsonPropertyName("get")]
    public string Get { get; set; } = default!;
    
    [JsonPropertyName("parameters")]
    public RawParameters Parameters { get; set; } = new();
    
    [JsonPropertyName("errors")]
    public List<object> Errors { get; set; } = new();
    
    [JsonPropertyName("results")]
    public int Results { get; set; }
    
    [JsonPropertyName("paging")]
    public RawPaging Paging { get; set; } = new();
    
    [JsonPropertyName("response")]
    public List<RawFixtureItem> Response { get; set; } = new();
}