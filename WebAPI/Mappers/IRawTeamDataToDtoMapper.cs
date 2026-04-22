using WebAPI.Dto;
using WebAPI.Models;

namespace WebAPI.Mappers;

public interface IRawTeamDataToDtoMapper
{
    public List<TeamDataDto> MapRawTeamDataToDto(RawTeamResponse rawTeamDataResponse);
    
}