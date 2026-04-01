namespace WebAPI.Models.Group;

public class GroupLeaderboardEntry
{
    public Guid Id { get; set; }
    
    public Guid GroupId { get; set; }
    
    public Group Group { get; set; }
    
    public string UserId { get; set; }
    
    public ApplicationUser User { get; set; }
    
    public string PeriodKey { get; set; }
    
    public string Period { get; set; }
    
    public int? LeagueId { get; set; }
    
    public int? SeasonId { get; set; }
    
    public int Points { get; set; }
    
    public int Rank { get; set; }
    
    public DateTime UpdatedAtUtc { get; set; }
}