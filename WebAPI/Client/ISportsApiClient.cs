using WebAPI.Models;
using WebAPI.Models.RawFixtureResponse;

namespace WebAPI.Client;

public interface ISportsApiClient
{
    Task<RawFixturesResponse> GetUpcomingFixturesAsync(int leagueId);
    
    Task<RawFixturesResponse> GetAllFixturesByLeagueAsync(int leagueId, int seasonByYear, CancellationToken cancellationToken);
    
    Task<RawFixturesResponse> GetTheUpcomingFixturesByLeagueAsync(int leagueId, int seasonByYear, int numberOfNextMatches, CancellationToken cancellationToken);
    
    Task<RawGoalScorerResponse> GetTheTopGoalScorersByLeagueAsync(int leagueId, int seasonByYear, CancellationToken cancellationToken);
    
    Task<RawFixturesResponse> GetTheTopAssistsByLeagueAsync(int leagueId, int seasonByYear, CancellationToken cancellationToken);
    
    Task<RawFixturesResponse> GetTheWinnersOfTheLeagueAsync(int leagueId, int seasonByYear, CancellationToken cancellationToken);
    
    Task<RawTeamResponse> GetAllTeamsOfTheLeagueAsync(int leagueId, int seasonByYear, CancellationToken cancellationToken);
}