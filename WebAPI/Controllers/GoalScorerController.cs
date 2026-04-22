using Microsoft.AspNetCore.Mvc;
using WebAPI.Client;
using WebAPI.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GoalScorerController : ControllerBase
{
    private readonly IGoalScorerService _goalScorerService;
    private readonly ISportsApiClient _sportsApiClient;

    public GoalScorerController(IGoalScorerService goalScorerService, ISportsApiClient sportsApiClient)
    {
        _goalScorerService = goalScorerService;
        _sportsApiClient = sportsApiClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetTopScorerOfTheLeague([FromQuery] int league = 39, [FromQuery] int season = 2022)
    {
        var GoalScorerData = await _goalScorerService.GetGoalScorersByLeagueAndSeasonAsync(league, season);

        if (GoalScorerData == null)
        {
            return NotFound();
        }

        return Ok(GoalScorerData);
    }

    [HttpGet("raw")]
    public async Task<IActionResult> GetRawTopScorerOfTheLeague([FromQuery] int league = 39,
        [FromQuery] int season = 2022)
    {
        var rawGoalScorerData =
            await _sportsApiClient.GetTheTopGoalScorersByLeagueAsync(league, season, CancellationToken.None);

        if (rawGoalScorerData == null)
        {
            return NotFound();
        }

        return Ok(rawGoalScorerData);
    }

}