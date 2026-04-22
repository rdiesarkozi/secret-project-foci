using System.Text.Json.Serialization;
using WebAPI.Models.GoalScorer;

namespace WebAPI.Models;

public class GoalScorerPlayer
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("firstname")]
    public string Firstname { get; set; }

    [JsonPropertyName("lastname")]
    public string Lastname { get; set; }

    [JsonPropertyName("age")]
    public int? Age { get; set; }

    [JsonPropertyName("birth")]
    public GoalScorerBirth Birth { get; set; }

    [JsonPropertyName("nationality")]
    public string Nationality { get; set; }

    [JsonPropertyName("height")]
    public string Height { get; set; }

    [JsonPropertyName("weight")]
    public string Weight { get; set; }

    [JsonPropertyName("injured")]
    public bool? Injured { get; set; }

    [JsonPropertyName("photo")]
    public string Photo { get; set; }
}