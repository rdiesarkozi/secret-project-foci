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
        var groupExists = await _dbContext.Groups
            .AnyAsync(g => g.Id == groupId);

        if (!groupExists)
        {
            return new List<LeaderboardDto>();
        }

        var leaderboard = await _dbContext.GroupMembers
            .Where(gm => gm.GroupId == groupId)
            .Select(gm => new LeaderboardDto
            {
                Username = gm.User.UserName,
                Points = gm.Points
            })
            .OrderByDescending(x => x.Points)
            .ToListAsync();

        for (var i = 0; i < leaderboard.Count; i++)
        {
            leaderboard[i].Rank = i + 1;
        }

        return leaderboard;
    }

}