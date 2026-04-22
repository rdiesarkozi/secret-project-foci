using System.Text.Json.Serialization;
using WebAPI.Models.GoalScorer;

namespace WebAPI.Models;

public class GoalScorerData
{
    [JsonPropertyName("player")]
    public GoalScorerPlayer Player { get; set; }

    [JsonPropertyName("statistics")]
    public List<GoalScorerStatistics> Statistics { get; set; } = new();

}