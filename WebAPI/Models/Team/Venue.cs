using System.Text.Json.Serialization;

namespace WebAPI.Models;

public class Venue
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("address")]
    public string Address { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; }

    [JsonPropertyName("capacity")]
    public int? Capacity { get; set; }

    [JsonPropertyName("surface")]
    public string Surface { get; set; }

    [JsonPropertyName("image")]
    public string Image { get; set; }
}