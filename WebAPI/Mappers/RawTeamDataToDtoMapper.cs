using WebAPI.Dto;
using WebAPI.Models;

namespace WebAPI.Mappers;

public class RawTeamDataToDtoMapper : IRawTeamDataToDtoMapper
{
    public List<TeamDataDto> MapRawTeamDataToDto(RawTeamResponse rawTeamDataResponse)
    {
        var teamDataDtoList = new List<TeamDataDto>();
        
        foreach (var item in rawTeamDataResponse.Response)
        {
            var teamDataDto = new TeamDataDto
            {
                Id = item.Team.Id,
                Name = item.Team.Name,
                Code = item.Team.Code,
            };
            
            teamDataDtoList.Add(teamDataDto);
        }

        return teamDataDtoList;
    }
}