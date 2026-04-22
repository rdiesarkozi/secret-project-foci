using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using WebAPI.Client;
using WebAPI.Dto;
using WebAPI.Interfaces;
using WebAPI.Mappers;
using WebAPI.Models;

namespace WebAPI.Services;

public class GoalScorerService : IGoalScorerService
{
    private readonly ISportsApiClient _sportsApiClient;
    private readonly IRawGoalScorerToDtoMapper _rawGoalScorerToDtoMapper;
    private readonly IDistributedCache _cache;

    public GoalScorerService(ISportsApiClient sportsApiClient, IRawGoalScorerToDtoMapper rawGoalScorerToDtoMapper, IDistributedCache cache)
    {
        _sportsApiClient = sportsApiClient;
        _rawGoalScorerToDtoMapper = rawGoalScorerToDtoMapper;
        _cache = cache;
    }
    
    public async Task<List<GoalScorerDataDto>> GetGoalScorersByLeagueAndSeasonAsync(int league, int season)
    {
        var cacheKey = $"goalscorerByLeague_{league}_{season}";
        
        var cachedBytes = await _cache.GetAsync(cacheKey);
        
        if (cachedBytes is not null)
        {
            return JsonSerializer.Deserialize<List<GoalScorerDataDto>>(cachedBytes)!;
        }

        var rawGoalScorerData = await _sportsApiClient.GetTheTopGoalScorersByLeagueAsync(league, season, CancellationToken.None);

        var goalScorerDto = _rawGoalScorerToDtoMapper.MapRawGoalScorerToDto(rawGoalScorerData);
        
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };
        
        await _cache.SetAsync(
            cacheKey,
            JsonSerializer.SerializeToUtf8Bytes(goalScorerDto),
            cacheOptions);

        return goalScorerDto;
    }
}