using WebAPI.Data.Enums;
using WebAPI.Models.Group;

namespace WebAPI.Interfaces;

public interface IGroupMemberService
{
    Task<GroupMember> AddMemberAsync(int groupId, string userId, GroupRole role = GroupRole.Member);
    Task<bool> RemoveMemberAsync(int groupId, string userId);
    Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int groupId);
    Task<bool> IsUserMemberAsync(int groupId, string userId);
    Task<bool> IsUserAdminOrOwnerAsync(int groupId, string userId);
    Task<bool> UpdateMemberRoleAsync(int groupId, string userId, GroupRole newRole);

}