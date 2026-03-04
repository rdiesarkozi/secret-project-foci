using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawFixtureItem
{
    [JsonPropertyName("fixture")]
    public RawFixture Fixture { get; set; }
    
    [JsonPropertyName("league")]
    public RawLeague League { get; set; }
    
    [JsonPropertyName("teams")]
    public RawTeams Teams { get; set; }
    
    [JsonPropertyName("goals")]
    public RawGoals Goals { get; set; }
    
    [JsonPropertyName("score")]
    public RawScore Score { get; set; }
}