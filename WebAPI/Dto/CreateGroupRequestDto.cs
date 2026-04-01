using WebAPI.Data.Enums;

namespace WebAPI.Dto;

public class CreateGroupRequestDto
{
    public string GroupName { get; set; }

    public GroupVisibility Visibility { get; set; }

    public int LeagueId { get; set; }
    
    public int seasonId { get; set; }
    
}