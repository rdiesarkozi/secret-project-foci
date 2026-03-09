using Microsoft.EntityFrameworkCore;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Services;

public class TipService : ITipService
{
    private readonly AppDbContext _dbContext;
    private readonly IFixtureDataService _footballApiService;
    
    public TipService(AppDbContext dbContext, IFixtureDataService footballApiService)
    {
        _dbContext = dbContext;
        _footballApiService = footballApiService;
    }
    
    public async Task CreateTipAsync(int fixtureId, string userId, int homeScoreTip, int awayScoreTip)
    {
        var existingTip = _dbContext.Tips.FirstOrDefault(t => t.MatchId == fixtureId && t.UserId == userId);
        
        if (existingTip != null)
        {
            throw new InvalidOperationException("A tip for this fixture by this user already exists.");
        }

        var tip = new Tip
        {
            Id = Guid.NewGuid(),
            MatchId = fixtureId,
            UserId = userId,
            LeagueId = 39, // Placeholder, should be set based on the fixture's league
            PredictedHomeScore = homeScoreTip,
            PredictedAwayScore = awayScoreTip,
            ResultStatus = "Pending",
            SubmittedAtUtc = DateTime.UtcNow,
            AwardedPoints = null,
            LockedAtUtc = null,
            ActualAwayScore = null,
            ActualHomeScore = null
        };
        
        _dbContext.Tips.Add(tip);
        await _dbContext.SaveChangesAsync();
    }

    public Task UpdateTipAsync(int fixtureId, string userId, string homeScoreTip, string awayScoreTip)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTipAsync(int fixtureId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<Tip> GetTipByIdAsync(int fixtureId, string userId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<Tip>> GetTipsForUserAsync(string userId)
    {
        return await _dbContext.Tips
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.SubmittedAtUtc)
            .ToListAsync();
    }

    public async Task CalculatePointsForCompletedMatchesAsync()
    {
        var pendingTips = await _dbContext.Tips
            .Where(t => t.ResultStatus == "Pending")
            .ToListAsync();

        foreach (var tip in pendingTips)
        {
            // Fetch actual result from external API or your Match table
            var matchResult = await _footballApiService.GetFixturesResultByMatchIdAsync(tip.MatchId);
        
            if (matchResult.Status != "FT") continue; // Match not finished
        
            tip.ActualHomeScore = matchResult.HomeScore;
            tip.ActualAwayScore = matchResult.AwayScore;
            tip.AwardedPoints = CalculatePoints(tip);
            tip.ResultStatus = DetermineResultStatus(tip);
            tip.SubmittedAtUtc = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync();
    }
    
    private int CalculatePoints(Tip tip)
    {
        var actualHome = tip.ActualHomeScore!.Value;
        var actualAway = tip.ActualAwayScore!.Value;
        var predictedHome = tip.PredictedHomeScore;
        var predictedAway = tip.PredictedAwayScore;

        // Telitalálat (Exact score): 8 points
        if (actualHome == predictedHome && actualAway == predictedAway)
        {
            return 8;
        }
        
        var actualOutcome = GetMatchOutcome(actualHome, actualAway);
        var predictedOutcome = GetMatchOutcome(predictedHome, predictedAway);
        
        // Kimenetel (1X2) és gólkülönbség pontszáma: 6 points
        if (actualOutcome == predictedOutcome)
        {
            var actualGoalDifference = actualHome - actualAway;
            var predictedGoalDifference = predictedHome - predictedAway;
            
            if (actualGoalDifference == predictedGoalDifference)
            {
                return 6;
            }
            
            // Kimenetel (1X2) pontszáma: 4 points
            return 4;
        }
        
        // Egyik csapat góljai pontszáma: 1 point each
        int points = 0;
        if (actualHome == predictedHome)
        {
            points += 1;
        }
        if (actualAway == predictedAway)
        {
            points += 1;
        }
        
        return points;
    }
    
    private int GetMatchOutcome(int homeScore, int awayScore)
    {
        if (homeScore > awayScore) return 1; // Home win
        if (homeScore < awayScore) return -1; // Away win
        return 0; // Draw
    }

    private string DetermineResultStatus(Tip tip)
    {
        if (tip.AwardedPoints == 8) return "Telitalálat";
        if (tip.AwardedPoints == 6) return "Kimenetel + Gólkülönbség";
        if (tip.AwardedPoints == 4) return "Kimenetel";
        if (tip.AwardedPoints > 0) return "Részleges";
        return "Hibás";
    }
}