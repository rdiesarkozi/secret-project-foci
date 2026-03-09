using Microsoft.EntityFrameworkCore;
using WebAPI.Data.Enums;
using WebAPI.Interfaces;
using WebAPI.Models.Group;

namespace WebAPI.Services;

public class GroupService : IGroupService
{
    private readonly AppDbContext _context;

    public GroupService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<Group> CreateGroup(string groupName, string creatorId)
    {
        var group = new Group
        {
            Id = Guid.NewGuid(),
            Name = groupName,
            JoinCode = GenerateJoinCode(),
            CreatedAtUtc = DateTime.UtcNow,
            Visibility = nameof(GroupVisibility.Private),
            OwnerId = creatorId
        };

        var owner = new GroupMember
        {
            GroupId = group.Id,
            UserId = creatorId,
            Role = GroupRole.Owner.ToString(),
            JoinedAtUtc = DateTime.UtcNow,
        };

        await _context.Groups.AddAsync(group);
        await _context.GroupMembers.AddAsync(owner);
        await _context.SaveChangesAsync();

        return group;
    }

    public Task<Group?> GetGroupById(int groupId)
    {
        throw new NotImplementedException();
    }

    public Task<Group?> GetGroupByJoinCode(string joinCode)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Group>> GetUserGroups(string userId)
    {
        var groups = await _context.GroupMembers
            .Where(gm => gm.UserId == userId)
            .Select(gm => gm.Group)
            .ToListAsync();

        return groups;
    }

    public async Task<GroupMember?> JoinGroupByCodeAsync(string joinCode, string userId)
    {
        var group = await _context.Groups
            .FirstOrDefaultAsync(g => g.JoinCode == joinCode);

        if (group == null)
            return null;

        var existingMember = await _context.GroupMembers
            .AnyAsync(gm => gm.GroupId == group.Id && gm.UserId == userId);

        if (existingMember)
            return null;

        var member = new GroupMember
        {
            GroupId = group.Id,
            UserId = userId,
            Role = GroupRole.Member.ToString(),
            JoinedAtUtc = DateTime.UtcNow
        };

        await _context.GroupMembers.AddAsync(member);
        await _context.SaveChangesAsync();

        return member;
    }
    
    private string GenerateJoinCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}