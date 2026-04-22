using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using WebAPI.Client;
using WebAPI.Dto;
using WebAPI.Interfaces;
using WebAPI.Mappers;
using WebAPI.Models;

namespace WebAPI.Services;

public class TeamService : ITeamService
{
    private readonly ISportsApiClient _sportsApiClient;
    private readonly IRawTeamDataToDtoMapper _rawTeamDataToDtoMapper;
    private readonly IDistributedCache _distributedCache;
    
    public TeamService(ISportsApiClient sportsApiClient, IDistributedCache distributedCache, IRawTeamDataToDtoMapper rawTeamDataToDtoMapper)
    {
        _sportsApiClient = sportsApiClient;
        _rawTeamDataToDtoMapper = rawTeamDataToDtoMapper;
        _distributedCache = distributedCache;
    }
    
    public async Task<List<TeamDataDto>> GetAllTeamDataByLeague(int leagueId, int seasonId)
    {
        var cacheKey = $"teamDataByLeague_{leagueId}_{seasonId}";
        
        var cachedBytes = await _distributedCache.GetAsync(cacheKey);
        
        if (cachedBytes is not null)
        {
            return JsonSerializer.Deserialize<List<TeamDataDto>>(cachedBytes)!;
        }
        
        var teamData = await _sportsApiClient.GetAllTeamsOfTheLeagueAsync(leagueId, seasonId, CancellationToken.None);

        // Add these temporarily to identify where data is lost
        Console.WriteLine($"Raw response count: {teamData?.Response?.Count}");

        var mappedTeamData = _rawTeamDataToDtoMapper.MapRawTeamDataToDto(teamData);

        Console.WriteLine($"Mapped data count: {mappedTeamData?.Count}");
        
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };
        
        await _distributedCache.SetAsync(
            cacheKey,
            JsonSerializer.SerializeToUtf8Bytes(mappedTeamData),
            cacheOptions);
        
        return mappedTeamData;
    }
}