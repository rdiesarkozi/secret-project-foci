using WebAPI.Dto;
using WebAPI.Models;

namespace WebAPI.Interfaces;

public interface ITeamService
{
    public Task<List<TeamDataDto>> GetAllTeamDataByLeague(int leagueId, int seasonId);
}