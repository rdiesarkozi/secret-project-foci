namespace WebAPI.Models;

public class Tip
{
    public Guid Id { get; set; }
    
    public string UserId { get; set; }
    
    public ApplicationUser User { get; set; }
    
    public long MatchId { get; set; }
    
    public int LeagueId { get; set; }
    
    public int SeasonId { get; set; }
    
    public int PredictedHomeScore { get; set; }
    
    public int PredictedAwayScore { get; set; }
    
    public int? ActualHomeScore { get; set; }
    
    public int? ActualAwayScore { get; set; }
    
    public DateTime SubmittedAtUtc { get; set; }
    
    public DateTime? LockedAtUtc { get; set; }
    
    public string ResultStatus { get; set; } = string.Empty;
    
    public int? AwardedPoints { get; set; }
}