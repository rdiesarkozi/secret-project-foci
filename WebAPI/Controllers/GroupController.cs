using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dto;
using WebAPI.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;
    
    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }
    
    [HttpPost("create")]
    public async Task<IActionResult> CreateNewGroupAsync(string groupName)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in claims.");
        }
        
        var group = await _groupService.CreateGroup(groupName, userId);
        
        return Ok(new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            JoinCode = group.JoinCode,
            Visibility = group.Visibility,
            CreatedAtUtc = group.CreatedAtUtc
        });
    }

    [HttpPost("join")]
    public async Task<IActionResult> JoinGroupByCodeAsync(string joinCode)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in claims.");
        }
        
        var groupMember = await _groupService.JoinGroupByCodeAsync(joinCode, userId);
        
        if (groupMember == null)
        {
            return NotFound("Group not found or join code is invalid.");
        }
        
        return Ok(new GroupMemberDto
        {
            GroupId = groupMember.GroupId,
            UserId = groupMember.UserId,
            Role = groupMember.Role,
            JoinedAtUtc = groupMember.JoinedAtUtc
        });
    }

    [HttpGet("my-groups")]
    public async Task<IActionResult> GetUserGroupsAsync()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in claims.");
        }
        
        var groups = await _groupService.GetUserGroups(userId);
        
        var groupDtos = groups.Select(g => new GroupDto
        {
            Id = g.Id,
            Name = g.Name,
            JoinCode = g.JoinCode,
            Visibility = g.Visibility,
            CreatedAtUtc = g.CreatedAtUtc
        }).ToList();
        
        return Ok(groupDtos);
    }
}