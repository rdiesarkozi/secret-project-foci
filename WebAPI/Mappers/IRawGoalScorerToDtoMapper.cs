using WebAPI.Dto;
using WebAPI.Models;

namespace WebAPI.Mappers;

public interface IRawGoalScorerToDtoMapper
{
    public List<GoalScorerDataDto> MapRawGoalScorerToDto(RawGoalScorerResponse rawGoalScorerResponse);
}