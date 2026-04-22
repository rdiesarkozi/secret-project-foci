using System.Text.Json.Serialization;

namespace WebAPI.Models;

public class Team
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("country")]
    public string Country { get; set; }

    [JsonPropertyName("founded")]
    public int? Founded { get; set; }

    [JsonPropertyName("national")]
    public bool? National { get; set; }

    [JsonPropertyName("logo")]
    public string Logo { get; set; }
}