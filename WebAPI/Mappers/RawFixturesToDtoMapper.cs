using WebAPI.Dto;
using WebAPI.Models.RawFixtureResponse;

namespace WebAPI.Mappers;

public class RawFixturesToDtoMapper : IRawFixturesToDtoMapper
{
    public List<FixtureDataDto> MapRawFixtureToDto(RawFixturesResponse rawFixture)
    {
        var fixtureDataDtos = new List<FixtureDataDto>();
        
        foreach (var item in rawFixture.Response)
        {
            var fixtureDataDto = new FixtureDataDto
            {
                FixtureId = item.Fixture.Id,
                Status = item.Fixture.Status.Short,
                AwayScore = item.Goals.Away,
                HomeScore = item.Goals.Home,
                FixtureDate = item.Fixture.Date,
                HomeTeam = item.Teams.Home.Name,
                AwayTeam = item.Teams.Away.Name,
                Result = $"{item.Score.FullTime.Home}:{item.Score.FullTime.Away}"
            };
            
            fixtureDataDtos.Add(fixtureDataDto);
        }

        return fixtureDataDtos;
    }
}