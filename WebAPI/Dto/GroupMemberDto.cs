namespace WebAPI.Dto;

public class GroupMemberDto
{
    public Guid GroupId { get; set; }
    
    public string UserId { get; set; }
    
    public string Role { get; set; }
    
    public DateTime JoinedAtUtc { get; set; }
}