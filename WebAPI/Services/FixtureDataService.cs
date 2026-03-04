using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using WebAPI.Client;
using WebAPI.Dto;
using WebAPI.Interfaces;
using WebAPI.Mappers;

namespace WebAPI.Services;

public class FixtureDataService : IFixtureDataService
{
    private readonly ISportsApiClient _sportsApiClient;
    private readonly IRawFixturesToDtoMapper _rawFixturesToDtoMapper;
    private readonly IDistributedCache _cache;
    
    public FixtureDataService(ISportsApiClient sportsApiClient, IRawFixturesToDtoMapper rawFixturesToDtoMapper, IDistributedCache cache)
    {
        _sportsApiClient = sportsApiClient;
        _rawFixturesToDtoMapper = rawFixturesToDtoMapper;
        _cache = cache;
    }
    
    public async Task<List<FixtureDataDto>> GetFixtureDataAsync(int league, int season)
    {
        var cacheKey = $"fixtures_{league}_{season}";
        
        var cachedBytes = await _cache.GetAsync(cacheKey);
        if (cachedBytes is not null)
        {
            return JsonSerializer.Deserialize<List<FixtureDataDto>>(cachedBytes)!;
        }

        var rawFixtureData = 
            await _sportsApiClient.GetAllFixturesByLeagueAsync(league, season, CancellationToken.None);

        var fixtureDataDtos = _rawFixturesToDtoMapper.MapRawFixtureToDto(rawFixtureData);

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };

        await _cache.SetAsync(
            cacheKey,
            JsonSerializer.SerializeToUtf8Bytes(fixtureDataDtos),
            cacheOptions);

        return fixtureDataDtos;
    }


    public async Task<FixtureDataDto> GetFixtureDataByTeamAsync(int fixtureId)
    {
        var rawFixtureData = await _sportsApiClient.GetAllFixturesByLeagueAsync(39, 2022, CancellationToken.None);
        var fixtureDataDtos = _rawFixturesToDtoMapper.MapRawFixtureToDto(rawFixtureData);
        var fixtureDataDto = fixtureDataDtos.FirstOrDefault(x => x.FixtureId == fixtureId);
        return fixtureDataDto;
    }
}