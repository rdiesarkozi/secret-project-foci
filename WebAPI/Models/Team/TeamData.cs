using System.Text.Json.Serialization;

namespace WebAPI.Models;

public class TeamData
{
    [JsonPropertyName("team")]
    public Team Team { get; set; }

    [JsonPropertyName("venue")]
    public Venue Venue { get; set; }
}