using WebAPI.Models.Group;

namespace WebAPI.Interfaces;

public interface ILeaderboardService
{
    
    Task<IEnumerable<GroupLeaderboardEntry>> GetLeaderboardAsync(int groupId, int? seasonId = null, int? matchdayId = null);
    Task UpdateUserPointsAsync(int groupId, string oddsId, int points);
    Task RecalculateLeaderboardAsync(int groupId, int? seasonId = null);
    Task RecalculateAllLeaderboardsAsync();
}