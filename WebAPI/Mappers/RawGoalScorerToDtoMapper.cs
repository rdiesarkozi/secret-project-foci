using WebAPI.Dto;
using WebAPI.Models;

namespace WebAPI.Mappers;

public class RawGoalScorerToDtoMapper : IRawGoalScorerToDtoMapper
{
    public List<GoalScorerDataDto> MapRawGoalScorerToDto(RawGoalScorerResponse rawGoalScorerResponse)
    {
        var goalScorerDtos = new List<GoalScorerDataDto>();
        
        foreach (var items in rawGoalScorerResponse.Response)
        {
            var goalScorerDataDto = new GoalScorerDataDto
            {
                Name = items.Player.Name,
                FirstName = items.Player.Firstname,
                LastName = items.Player.Lastname,
                amountOfGoals = items.Statistics.FirstOrDefault(s => s.Goals?.Total != null)?.Goals?.Total
             };
            goalScorerDtos.Add(goalScorerDataDto);
        }

        return goalScorerDtos;
    }
}