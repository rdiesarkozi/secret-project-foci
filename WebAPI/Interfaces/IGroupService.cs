using WebAPI.Data.Enums;
using WebAPI.Models.Group;

namespace WebAPI.Interfaces;

public interface IGroupService
{
    public Task<Group> CreateGroup(string groupName, string creatorId,int leagueId, int seasonId, GroupVisibility visibility);
    
    public Task<Group?> GetGroupById(Guid groupId);
    
    public Task<Group?> GetGroupByJoinCode(string joinCode);
    
    public Task<IEnumerable<Group>> GetUserGroups(string userId);
    
    public Task<GroupMember?> JoinGroupByCodeAsync(string joinCode, string userId);
    
    public Task<bool> LeaveGroupAsync(Guid groupId, string userId);
    
    public Task<bool> DeleteGroupAsync(Guid groupId, string userId);
}