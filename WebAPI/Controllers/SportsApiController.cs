using Microsoft.AspNetCore.Mvc;
using WebAPI.Client;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SportsApiController : ControllerBase
{
        private readonly ILogger<SportsApiController> _logger;
        private readonly ISportsApiClient _sportsApiClient;
    
        public SportsApiController(ILogger<SportsApiController> logger, ISportsApiClient sportsApiClient)
        {
            _logger = logger;
            _sportsApiClient = sportsApiClient;
        }
    
        [HttpGet("ping")]
        public IActionResult Ping()
        {
            _logger.LogInformation("Ping endpoint was called.");
            return Ok("Pong");
        }
        
        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingFixtures(int league = 1, int season = 2026, int next = 30)
        {
            var upcomingFixtures = await _sportsApiClient.GetTheUpcomingFixturesByLeagueAsync(league, season, next, CancellationToken.None);
            
            if (upcomingFixtures == null || !upcomingFixtures.Response.Any())
            {
                _logger.LogWarning("No upcoming fixtures found for league {League} and season {Season}.", league, season);
                return NotFound("No upcoming fixtures found.");
            }
            
            return Ok(upcomingFixtures);
        }
    
}