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

        // Collect user points to update
        var userPointsToAdd = new Dictionary<string, int>();

        foreach (var tip in pendingTips)
        {
            var matchResult = await _footballApiService.GetFixturesResultByMatchIdAsync(tip.MatchId);

            if (matchResult.Status != "FT") continue;

            tip.ActualHomeScore = matchResult.HomeScore;
            tip.ActualAwayScore = matchResult.AwayScore;
            tip.AwardedPoints = CalculatePoints(tip);
            tip.ResultStatus = DetermineResultStatus(tip);
            tip.SubmittedAtUtc = DateTime.UtcNow;

            // Accumulate points per user
            if (tip.AwardedPoints > 0)
            {
                if (userPointsToAdd.ContainsKey(tip.UserId))
                    userPointsToAdd[tip.UserId] += tip.AwardedPoints.Value;
                else
                    userPointsToAdd[tip.UserId] = tip.AwardedPoints.Value;
            }
        }

        // Update GroupMember points for all affected users
        foreach (var (userId, points) in userPointsToAdd)
        {
            var memberships = await _dbContext.GroupMembers
                .Where(gm => gm.UserId == userId)
                .ToListAsync();

            foreach (var member in memberships)
            {
                member.Points += points;
            }
        }

        await _dbContext.SaveChangesAsync();
    }
    
    private int CalculatePoints(Tip tip)
    {
        int points = 0;

        var actualHome = tip.ActualHomeScore!.Value;
        var actualAway = tip.ActualAwayScore!.Value;
        var predictedHome = tip.PredictedHomeScore;
        var predictedAway = tip.PredictedAwayScore;

        // (Exact score): 8 points
        if (actualHome == predictedHome && actualAway == predictedAway)
        {
            return 8;
        }
        
        var actualOutcome = GetMatchOutcome(actualHome, actualAway);
        var predictedOutcome = GetMatchOutcome(predictedHome, predictedAway);
        
        if (actualOutcome == predictedOutcome)
        {
            var actualGoalDifference = actualHome - actualAway;
            var predictedGoalDifference = predictedHome - predictedAway;
            
            // Correct outcome (1X2) + correct goal difference: 6 points
            if (actualGoalDifference == predictedGoalDifference)
            {
                return 6;
            }
            
            // Correct outcome (1X2) only: 4 points
            points = 4;
        }
        
        // Correct goals for one team: 1 point each
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
        if (tip.AwardedPoints == 8) return "Exact";
        if (tip.AwardedPoints == 6) return "Correct Outcome + Goal Difference";
        if (tip.AwardedPoints >= 4) return "Correct Outcome";
        if (tip.AwardedPoints > 0) return "Partial";
        return "Incorrect";
    }
}