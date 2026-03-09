using WebAPI.Interfaces;
using WebAPI.Models.Group;

namespace WebAPI.Services;

public class GroupService : IGroupService
{
    public Task<Group> CreateGroup(string groupName, string creatorId)
    {
        throw new NotImplementedException();
    }

    public Task<Group?> GetGroupById(int groupId)
    {
        throw new NotImplementedException();
    }

    public Task<Group?> GetGroupByJoinCode(string joinCode)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Group>> GetUserGroups(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<GroupMember?> JoinGroupByCodeAsync(string joinCode, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<string> GenerateUniqueJoinCodeAsync(int groupId)
    {
        throw new NotImplementedException();
    }
}