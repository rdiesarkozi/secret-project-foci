using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerCards
{
    [JsonPropertyName("yellow")]
    public int? Yellow { get; set; }

    [JsonPropertyName("yellowred")]
    public int? YellowRed { get; set; }

    [JsonPropertyName("red")]
    public int? Red { get; set; }
}