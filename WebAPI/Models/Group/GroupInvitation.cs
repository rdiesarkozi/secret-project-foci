namespace WebAPI.Models.Group;

public class GroupInvitation
{
    public Guid Id { get; set; }
    
    public Guid GroupId { get; set; }
    
    public Group Group { get; set; }
    
    public string InvitedByUserId { get; set; }  // Foreign key (string to match ApplicationUser.Id)
    
    public ApplicationUser InvitedByUser { get; set; }
    
    public string InviteEmail { get; set; }
    
    public string Status { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
    
    public DateTime ExpiresAtUtc { get; set; }
    
    public string Token { get; set; }
}