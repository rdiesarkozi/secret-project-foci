using Microsoft.EntityFrameworkCore;
using WebAPI.Data.Enums;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Services;

public class TipService : ITipService
{
    private readonly AppDbContext _dbContext;
    private readonly IFixtureDataService _footballApiService;
    private readonly ILogger<TipService> _logger;
    
    public TipService(AppDbContext dbContext, IFixtureDataService footballApiService, ILogger<TipService> logger)
    {
        _dbContext = dbContext;
        _footballApiService = footballApiService;
        _logger = logger;
    }
    
    public async Task CreateTipAsync(int fixtureId, string userId, int leagueId, int season, int homeScoreTip, int awayScoreTip)
    {
        var matchResult = await _footballApiService.GetFixturesResultByMatchIdAsync(fixtureId, leagueId, season);

        var kickoffUtc = DateTime.SpecifyKind(matchResult.FixtureDate, DateTimeKind.Utc);

        var lockTimeUtc = kickoffUtc.AddMinutes(-1);

        if (DateTime.UtcNow >= lockTimeUtc)
        {
            throw new InvalidOperationException("The tipping already closed, because the match begin");
        }

        var existingTip = await _dbContext
            .Tips
            .FirstOrDefaultAsync(t => t.MatchId == fixtureId && t.UserId == userId);

        if (existingTip != null)
        {
            throw new InvalidOperationException("You have already created a tip.");
        }

        var tip = new Tip
        {
            Id = Guid.NewGuid(),
            MatchId = fixtureId,
            UserId = userId,
            LeagueId = leagueId,
            SeasonId = season,
            PredictedHomeScore = homeScoreTip,
            PredictedAwayScore = awayScoreTip,
            ResultStatus = ResultStatus.NotStarted.ToString(),
            SubmittedAtUtc = DateTime.UtcNow,
            LockedAtUtc = lockTimeUtc,
            AwardedPoints = null,
            ActualHomeScore = null,
            ActualAwayScore = null
        };

        _dbContext.Tips.Add(tip);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task UpdateTipAsync(int fixtureId, string userId, string homeScoreTip, string awayScoreTip)
    {
        var tip = await _dbContext.Tips
            .FirstOrDefaultAsync(t => t.MatchId == fixtureId && t.UserId == userId);

        if (tip == null)
        {
            throw new InvalidOperationException("Tip not found.");
        }
        
        _logger.LogInformation("LockedAtUtc: {LockedAtUtc}, DateTime.UtcNow: {DateTimeNow}", tip.LockedAtUtc, DateTime.UtcNow); ;

        // Prevent updates to explicitly locked tips
        if (tip.LockedAtUtc < DateTime.UtcNow)
        {
            throw new InvalidOperationException("Cannot update a locked tip.");
        }

        if (!int.TryParse(homeScoreTip, out var parsedHome) || parsedHome < 0)
        {
            throw new ArgumentException("Invalid home score.", nameof(homeScoreTip));
        }

        if (!int.TryParse(awayScoreTip, out var parsedAway) || parsedAway < 0)
        {
            throw new ArgumentException("Invalid away score.", nameof(awayScoreTip));
        }

        tip.PredictedHomeScore = parsedHome;
        tip.PredictedAwayScore = parsedAway;
        tip.SubmittedAtUtc = DateTime.UtcNow;

        _dbContext.Tips.Update(tip);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteTipAsync(int fixtureId, string userId)
    {
        var tip = await _dbContext.Tips
            .FirstOrDefaultAsync(t => t.MatchId == fixtureId && t.UserId == userId);

        if (tip == null)
        {
            throw new InvalidOperationException("Tip not found.");
        }

        // Only allow deletion if the tip hasn't been processed
        if (tip.ResultStatus != ResultStatus.NotStarted.ToString())
        {
            throw new InvalidOperationException("Cannot delete a tip that has already been processed.");
        }

        _dbContext.Tips.Remove(tip);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Tip> GetTipByIdAsync(int fixtureId, string userId)
    {
        var tip = await _dbContext.Tips.FirstOrDefaultAsync(t => t.MatchId == fixtureId && t.UserId == userId);
        if (tip == null)
        {
            throw new InvalidOperationException("Tip not found.");
        }
        return tip;
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
            .Where(t => t.ResultStatus == ResultStatus.NotStarted.ToString())
            .ToListAsync();

        // Collect user points to update
        var userPointsToAdd = new Dictionary<string, int>();

        foreach (var tip in pendingTips)
        {
            var matchResult = await _footballApiService.GetFixturesResultByMatchIdAsync(tip.MatchId, tip.LeagueId, tip.SeasonId);

            if (matchResult.Status != "FT") continue;

            tip.ActualHomeScore = matchResult.HomeScore;
            tip.ActualAwayScore = matchResult.AwayScore;
            tip.AwardedPoints = CalculatePoints(tip);
            tip.SubmittedAtUtc = DateTime.UtcNow;
            tip.ResultStatus = ResultStatus.Closed.ToString();

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