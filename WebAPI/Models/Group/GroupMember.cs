namespace WebAPI.Models.Group;

public class GroupMember
{
    public Guid Id { get; set; }
    
    public Guid GroupId { get; set; }
    
    public Group Group { get; set; }
    
    public string UserId { get; set; }
    
    public ApplicationUser User { get; set; }
    
    public int Points { get; set; }
    
    public string Role { get; set; }
    
    public DateTime JoinedAtUtc { get; set; }
}