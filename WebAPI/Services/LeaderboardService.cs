using WebAPI.Interfaces;
using WebAPI.Models.Group;

namespace WebAPI.Services;

public class LeaderboardService : ILeaderboardService
{
    public Task<IEnumerable<GroupLeaderboardEntry>> GetLeaderboardAsync(int groupId, int? seasonId = null, int? matchdayId = null)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserPointsAsync(int groupId, string oddsId, int points)
    {
        throw new NotImplementedException();
    }

    public Task RecalculateLeaderboardAsync(int groupId, int? seasonId = null)
    {
        throw new NotImplementedException();
    }

    public Task RecalculateAllLeaderboardsAsync()
    {
        throw new NotImplementedException();
    }
}