using WebAPI.Dto;
using WebAPI.Models.RawFixtureResponse;

namespace WebAPI.Mappers;

public interface IRawFixturesToDtoMapper
{
    public List<FixtureDataDto> MapRawFixtureToDto(RawFixturesResponse rawFixture);
}