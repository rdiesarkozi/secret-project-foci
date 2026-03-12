using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawFixture
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("referee")]
    public string Referee { get; set; } = default!;
    
    [JsonPropertyName("timezone")]
    public string Timezone { get; set; } = default!;
    
    [JsonPropertyName("date")]
    public DateTime Date { get; set; }
    
    [JsonPropertyName("timestamp")]
    public long Timestamp { get; set; }
    
    [JsonPropertyName("periods")]
    public RawPeriods Periods { get; set; } = new();
    
    [JsonPropertyName("venue")]
    public RawVenue Venue { get; set; } = new();
    
    [JsonPropertyName("status")]
    public RawStatus Status { get; set; } = new();
}