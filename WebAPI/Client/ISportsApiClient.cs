using WebAPI.Models.RawFixtureResponse;

namespace WebAPI.Client;

public interface ISportsApiClient
{
    Task<RawFixturesResponse> GetUpcomingFixturesAsync(int leagueId);
    
    Task<RawFixturesResponse> GetAllFixturesByLeagueAsync(int leagueId, int seasonByYear, CancellationToken cancellationToken);
}