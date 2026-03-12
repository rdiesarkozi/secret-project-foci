using Microsoft.EntityFrameworkCore;
using WebAPI.Dto;
using WebAPI.Interfaces;
using WebAPI.Models.Group;

namespace WebAPI.Services;

public class LeaderboardService : ILeaderboardService
{
    private readonly AppDbContext _dbContext;
    
    public LeaderboardService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<List<LeaderboardDto>> GetLeaderboardAsync(Guid groupId)
    {
        var members = await _dbContext.GroupMembers
            .Where(gm => gm.GroupId == groupId)
            .OrderByDescending(gm => gm.Points)
            .Select(gm => new LeaderboardDto
            {
                Username = gm.User.UserName,
                Points = gm.Points
            })
            .ToListAsync();

        for (int i = 0; i < members.Count; i++)
        {
            members[i].Rank = i + 1;
        }

        return members;
    }
}