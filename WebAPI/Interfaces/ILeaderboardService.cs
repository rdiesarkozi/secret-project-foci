using WebAPI.Dto;
using WebAPI.Models.Group;

namespace WebAPI.Interfaces;

public interface ILeaderboardService
{
    
    public Task<List<LeaderboardDto>> GetLeaderboardAsync(Guid groupId);
    
}