using WebAPI.Models;

namespace WebAPI.Interfaces;

public interface ITipService
{
    public Task CreateTipAsync(int fixtureId, string userId, int leagueId, int seasonId, int homeScoreTip, int awayScoreTip);
    
    public Task UpdateTipAsync(int fixtureId, string userId, string homeScoreTip, string awayScoreTip);
    
    public Task DeleteTipAsync(int fixtureId, string userId);
    
    public Task<Tip> GetTipByIdAsync(int fixtureId, string userId);
    
    public Task<List<Tip>> GetTipsForUserAsync(string userId);
    
    public Task CalculatePointsForCompletedMatchesAsync();
    
    public Task ChooseWinnerAndTopScorerAsync(int leagueId, int seasonId);
}