using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly ILeaderboardService _leaderboardService;
    
    public LeaderboardController(ILeaderboardService leaderboardService)
    {
        _leaderboardService = leaderboardService;
    }

    [HttpGet("overall")]
    public async Task<IActionResult> GetGroupLeaderboardAsync(Guid groupId)
    {
        var leaderboard = await _leaderboardService.GetLeaderboardAsync(groupId);
        if (leaderboard == null)
        {
            return NotFound("Group not found or no leaderboard data available.");
        }
        
        return Ok(leaderboard);
        
    }
}