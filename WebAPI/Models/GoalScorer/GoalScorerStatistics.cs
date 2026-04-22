using System.Text.Json.Serialization;

namespace WebAPI.Models.GoalScorer;

public class GoalScorerStatistics
{
    [JsonPropertyName("team")]
    public GoalScorerTeam Team { get; set; }

    [JsonPropertyName("league")]
    public GoalScorerLeague League { get; set; }

    [JsonPropertyName("games")]
    public GoalScorerGames Games { get; set; }

    [JsonPropertyName("substitutes")]
    public GoalScorerSubstitutes Substitutes { get; set; }

    [JsonPropertyName("shots")]
    public GoalScorerShots Shots { get; set; }

    [JsonPropertyName("goals")]
    public GoalScorerGoals Goals { get; set; }

    [JsonPropertyName("passes")]
    public GoalScorerPasses Passes { get; set; }

    [JsonPropertyName("tackles")]
    public GoalScorerTackles Tackles { get; set; }

    [JsonPropertyName("duels")]
    public GoalScorerDuels Duels { get; set; }

    [JsonPropertyName("dribbles")]
    public GoalScorerDribbles Dribbles { get; set; }

    [JsonPropertyName("fouls")]
    public GoalScorerFouls Fouls { get; set; }

    [JsonPropertyName("cards")]
    public GoalScorerCards Cards { get; set; }

    [JsonPropertyName("penalty")]
    public GoalScorerPenalty Penalty { get; set; }
}