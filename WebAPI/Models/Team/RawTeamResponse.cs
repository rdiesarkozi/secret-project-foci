using System.Text.Json.Serialization;

namespace WebAPI.Models;

public class RawTeamResponse
{
    [JsonPropertyName("get")]
    public string Get { get; set; }

    [JsonPropertyName("results")]
    public int Results { get; set; }

    [JsonPropertyName("response")]
    public List<TeamData> Response { get; set; }
}