using WebAPI.Dto;

namespace WebAPI.Interfaces;

public interface IFixtureDataService
{
    public Task<List<FixtureDataDto>> GetFixtureDataAsync(int league, int season);
    
    public Task<FixtureDataDto> GetFixtureDataByTeamAsync(int fixtureId);
    
    public Task<List<FixtureDataDto>> GetFixtureDataByDateAsync(DateTime date, int league, int season);
    
    public Task<FixtureDataDto> GetFixturesResultByMatchIdAsync(long matchId);
}