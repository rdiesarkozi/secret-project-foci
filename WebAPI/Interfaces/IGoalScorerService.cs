using WebAPI.Dto;
using WebAPI.Models;

namespace WebAPI.Interfaces;

public interface IGoalScorerService
{
    public Task<List<GoalScorerDataDto>> GetGoalScorersByLeagueAndSeasonAsync(int league, int season);
}