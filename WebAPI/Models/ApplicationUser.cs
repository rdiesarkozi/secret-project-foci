using Microsoft.AspNetCore.Identity;

namespace WebAPI.Models;

public class ApplicationUser : IdentityUser
{
    public string? FullName { get; set; }
    
    public DateTime CreatedAtUtc { get; set; }
    
    public ICollection<Tip> Tips { get; set; }
}