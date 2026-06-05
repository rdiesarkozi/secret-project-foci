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
    private readonly ILogger<FixtureDataService> _logger;
    
    public FixtureDataService(
        ISportsApiClient sportsApiClient,
        IRawFixturesToDtoMapper rawFixturesToDtoMapper,
        IDistributedCache cache,
        ILogger<FixtureDataService> logger)
    {
        _sportsApiClient = sportsApiClient;
        _rawFixturesToDtoMapper = rawFixturesToDtoMapper;
        _cache = cache;
        _logger = logger;
    }
    
    public async Task<List<FixtureDataDto>> GetFixtureDataAsync(int league, int season)
    {
        var cacheKey = $"fixtures_{league}_{season}";
        
        var cachedBytes = await _cache.GetAsync(cacheKey);
        if (cachedBytes is not null)
        {
            _logger.LogInformation("Cache hit for key {CacheKey}", cacheKey);
            return JsonSerializer.Deserialize<List<FixtureDataDto>>(cachedBytes)!;
        }

        var rawFixtureData = 
            await _sportsApiClient.GetAllFixturesByLeagueAsync(league, season, CancellationToken.None);
        _logger.LogInformation("Fetched raw fixture data for league {League} and season {Season}. Response count: {Count}",
            league, season, rawFixtureData?.Response?.Count ?? 0);

        var fixtureDataDtos = _rawFixturesToDtoMapper.MapRawFixtureToDto(rawFixtureData);
        _logger.LogInformation("Mapped raw fixture data to DTOs. Mapped count: {Count}", fixtureDataDtos?.Count ?? 0);

        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };
        
        _logger.LogInformation("Caching fixture data for key {CacheKey} with expiration of {Expiration} hours",
            cacheKey,
            cacheOptions.AbsoluteExpirationRelativeToNow?.TotalHours);

        await _cache.SetAsync(
            cacheKey,
            JsonSerializer.SerializeToUtf8Bytes(fixtureDataDtos),
            cacheOptions);
        
        return fixtureDataDtos;
    }


    public async Task<FixtureDataDto> GetFixtureDataByTeamAsync(int fixtureId)
    {
        var rawFixtureData = await _sportsApiClient.GetAllFixturesByLeagueAsync(39, 2022, CancellationToken.None);
        _logger.LogInformation("Fetched raw fixture data for fixture {FixtureId}", fixtureId);
        var fixtureDataDtos = _rawFixturesToDtoMapper.MapRawFixtureToDto(rawFixtureData);
        var fixtureDataDto = fixtureDataDtos.FirstOrDefault(x => x.FixtureId == fixtureId);
        return fixtureDataDto;
    }

    public async Task<List<FixtureDataDto>> GetFixtureDataByDateAsync(DateTime date, int league, int season)
    {
        var cacheKey = $"fixtures_{league}_{season}_{date:yyyy-MM-dd}";
        var cachedBytes = await _cache.GetAsync(cacheKey);
        
        _logger.LogInformation("Checking cache for key {CacheKey}", cacheKey);
        if (cachedBytes is not null)
        {
            return JsonSerializer.Deserialize<List<FixtureDataDto>>(cachedBytes)!;
        }
        
        var rawFixtureData = await _sportsApiClient.GetAllFixturesByLeagueAsync(league, season, CancellationToken.None);
        _logger.LogInformation("Fetched raw fixture data for league {League} and season {Season}. Response count: {Count}",
            league, season, rawFixtureData?.Response?.Count ?? 0);
        
        var fixtureDataDtos = _rawFixturesToDtoMapper.MapRawFixtureToDto(rawFixtureData);
        _logger.LogInformation("Mapped raw fixture data to DTOs. Mapped count: {Count}", fixtureDataDtos?.Count ?? 0);
        
        var filteredFixtures = fixtureDataDtos.Where(x => x.FixtureDate.Date == date.Date).ToList();
        _logger.LogInformation("Filtered fixtures: {FixturesCount}", filteredFixtures.Count);
        
        var cacheOptions = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        };
        
        await _cache.SetAsync(
            cacheKey,
            JsonSerializer.SerializeToUtf8Bytes(filteredFixtures),
            cacheOptions);
        
        return filteredFixtures;
    }

    public async Task<FixtureDataDto> GetFixturesResultByMatchIdAsync(long matchId, int leagueId = 39, int season = 2022)
    {
        var cacheKey = $"fixture_result_{matchId}";
        var cachedBytes = await _cache.GetAsync(cacheKey);
        
        if (cachedBytes is not null)        {
            return JsonSerializer.Deserialize<FixtureDataDto>(cachedBytes)!;
        }
        
        var rawFixtureData = await _sportsApiClient.GetAllFixturesByLeagueAsync(leagueId, season, CancellationToken.None);
        _logger.LogInformation("Fetched raw fixture data for league {League} and season {Season}. Response count: {Count}",
            leagueId, season, rawFixtureData?.Response?.Count ?? 0);
        
        var fixtureDataDtos = _rawFixturesToDtoMapper.MapRawFixtureToDto(rawFixtureData);
        _logger.LogInformation("Mapped raw fixture data to DTOs. Mapped count: {Count}", fixtureDataDtos?.Count ?? 0);
        
        var fixtureDataDto = fixtureDataDtos.FirstOrDefault(x => x.FixtureId == matchId);
        _logger.LogInformation("Found fixture data for match ID {MatchId}: {Found}", matchId, fixtureDataDto != null);
        
        if (fixtureDataDto is not null)
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            
            await _cache.SetAsync(
                cacheKey,
                JsonSerializer.SerializeToUtf8Bytes(fixtureDataDto),
                cacheOptions);
            _logger.LogInformation("Cached fixture result for match ID {MatchId} with expiration of {Expiration} hours",
                matchId,
                cacheOptions.AbsoluteExpirationRelativeToNow?.TotalHours);
        }
        
        return fixtureDataDto;
    }

    public async Task<List<FixtureDataDto>> GetAllUpcomingFixturesByLeagueAsync(int league, int season, int numberOfNextMatches)
    {
        var cacheKey = $"upcoming_fixtures_{league}_{season}_{numberOfNextMatches}";
        
        var cachedBytes = await _cache.GetAsync(cacheKey);
        if (cachedBytes is not null)
        {
            return JsonSerializer.Deserialize<List<FixtureDataDto>>(cachedBytes)!;
        }
        
        var rawFixtureData = await _sportsApiClient.GetTheUpcomingFixturesByLeagueAsync(league, season, numberOfNextMatches, CancellationToken.None);
            _logger.LogInformation("Fetched raw upcoming fixture data for league {League} and season {Season}. Response count: {Count}",
                league, season, rawFixtureData?.Response?.Count ?? 0);
            
        var fixtureDataDtos = _rawFixturesToDtoMapper.MapRawFixtureToDto(rawFixtureData);
        _logger.LogInformation("Mapped raw upcoming fixture data to DTOs. Mapped count: {Count}", fixtureDataDtos?.Count ?? 0);
        
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
}