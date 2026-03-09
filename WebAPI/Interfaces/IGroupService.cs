using WebAPI.Models.Group;

namespace WebAPI.Interfaces;

public interface IGroupService
{
    public Task<Group> CreateGroup(string groupName, string creatorId);
    
    public Task<Group?> GetGroupById(int groupId);
    
    public Task<Group?> GetGroupByJoinCode(string joinCode);
    
    public Task<IEnumerable<Group>> GetUserGroups(string userId);
    
    public Task<GroupMember?> JoinGroupByCodeAsync(string joinCode, string userId);
}