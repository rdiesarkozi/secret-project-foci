using System.Text.Json.Serialization;

namespace WebAPI.Models.RawFixtureResponse;

public class RawVenue
{
    [JsonPropertyName("id")]
    public int? Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;
    [JsonPropertyName("city")]
    public string City { get; set; } = default!;
}