using WebAPI.Dto;

namespace WebAPI.Interfaces;

public interface IFixtureDataService
{
    public Task<List<FixtureDataDto>> GetFixtureDataAsync(int league, int season);
    
    public Task<FixtureDataDto> GetFixtureDataByTeamAsync(int fixtureId);
}