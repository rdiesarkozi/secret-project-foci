using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data.Enums;
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
    public async Task<IActionResult> CreateNewGroupAsync([FromBody] CreateGroupRequestDto createGroupRequestDto)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in claims.");
        }
        
        if(createGroupRequestDto.LeagueId <= 0 || createGroupRequestDto.seasonId <= 0)
        {
            return BadRequest("Invalid league or season ID.");
        }
        
        var group = await _groupService.CreateGroup(createGroupRequestDto.GroupName,
            userId,
            createGroupRequestDto.LeagueId,
            createGroupRequestDto.seasonId,
            createGroupRequestDto.Visibility);
        
        return Ok(new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            JoinCode = group.JoinCode,
            SeasonId = group.SeasonId,
            LeagueId = group.LeagueId,
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

    [HttpGet("{groupId}")]
    public async Task<IActionResult> GetGroupByIdAsync(Guid groupId)
    {
        var group = await _groupService.GetGroupById(groupId);
        if (group == null)
        {
            return NotFound("Group not found.");
        }
        return Ok(new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            JoinCode = group.JoinCode,
            SeasonId = group.SeasonId,
            LeagueId = group.LeagueId,
            Visibility = group.Visibility,
            CreatedAtUtc = group.CreatedAtUtc
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
            SeasonId = g.SeasonId,
            LeagueId = g.LeagueId,
            Visibility = g.Visibility,
            CreatedAtUtc = g.CreatedAtUtc
        }).ToList();
        
        return Ok(groupDtos);
    }

    [HttpGet("join-code/{joinCode}")]
    public async Task<IActionResult> GetGroupByJoinCodeAsync(string joinCode)
    {
        var group = await _groupService.GetGroupByJoinCode(joinCode);
        
        if (group == null)
        {
            return NotFound("Group not found for the provided join code.");
        }
        
        return Ok(new GroupDto
        {
            Id = group.Id,
            Name = group.Name,
            JoinCode = group.JoinCode,
            SeasonId = group.SeasonId,
            LeagueId = group.LeagueId,
            Visibility = group.Visibility,
            CreatedAtUtc = group.CreatedAtUtc
        });
    }
}