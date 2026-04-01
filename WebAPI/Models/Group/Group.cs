namespace WebAPI.Models.Group;

public class Group
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Description { get; set; }
    
    public string JoinCode { get; set; }
    
    public string Visibility { get; set; }
    
    public int LeagueId { get; set; }
    
    public int SeasonId { get; set; }
    
    public string OwnerId { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
    
    public ApplicationUser Owner { get; set; }

    public ICollection<GroupMember> Members { get; set; } = new List<GroupMember>();
    
    public ICollection<GroupLeaderboardEntry> LeaderboardEntries { get; set; } = new List<GroupLeaderboardEntry>();
}