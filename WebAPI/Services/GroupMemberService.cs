using WebAPI.Data.Enums;
using WebAPI.Interfaces;
using WebAPI.Models.Group;

namespace WebAPI.Services;

public class GroupMemberService : IGroupMemberService
{
    public Task<GroupMember> AddMemberAsync(int groupId, string userId, GroupRole role = GroupRole.Member)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveMemberAsync(int groupId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int groupId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUserMemberAsync(int groupId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsUserAdminOrOwnerAsync(int groupId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateMemberRoleAsync(int groupId, string userId, GroupRole newRole)
    {
        throw new NotImplementedException();
    }
}