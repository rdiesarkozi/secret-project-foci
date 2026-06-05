using Microsoft.AspNetCore.Mvc;
using WebAPI.Client;
using WebAPI.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamController : ControllerBase
{
    private readonly ISportsApiClient _sportsApiClient;
    private readonly ITeamService _teamService;
    
    public TeamController(ISportsApiClient sportsApiClient, ITeamService teamService)
    {
        _sportsApiClient = sportsApiClient;
        _teamService = teamService;
    }
    
    [HttpGet("get-all-teams-raw")]
    public async Task<IActionResult> GetAllTeamsOfTheLeague([FromQuery] int league = 39, [FromQuery] int season = 2022)
    {
        var teams = await _sportsApiClient.GetAllTeamsOfTheLeagueAsync(league, season, CancellationToken.None);
        
        if (teams == null || !teams.Response.Any())
        {
            return NotFound("No teams found for the specified league and season.");
        }
        
        return Ok(teams);
    }
    
    [HttpGet("get-all-teams")]
    public async Task<IActionResult> GetAllTeamsOfTheLeagueFromService([FromQuery] int league = 39, [FromQuery] int season = 2022)
    {
        var teams = await _teamService.GetAllTeamDataByLeague(league, season);
        
        if (teams == null || !teams.Any())
        {
            return NotFound("No teams found for the specified league and season.");
        }
        
        return Ok(teams);
    }
}