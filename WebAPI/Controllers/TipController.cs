using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TipController : ControllerBase
{
    private readonly ILogger<TipController> _logger;
    private readonly ITipService _tipService;
    
    public TipController(ILogger<TipController> logger, ITipService tipService)
    {
        _logger = logger;
        _tipService = tipService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateTip(int fixtureId, int homeScoreTip, int awayScoreTip)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in claims.");
        }

        try
        {
            await _tipService.CreateTipAsync(fixtureId, userId, homeScoreTip, awayScoreTip);
            return Ok("Tip created successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating tip");
            return StatusCode(500, "An error occurred while creating the tip.");
        }
    }
    
    [HttpGet("get")]
    public async Task<IActionResult> GetTipByUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in claims.");
        }

        try
        {
            var tip = await _tipService.GetTipsForUserAsync(userId);
            if (tip == null)
            {
                return NotFound("Tip not found for the specified fixture and user.");
            }
            return Ok(tip);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tip");
            return StatusCode(500, "An error occurred while retrieving the tip.");
        }
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteTip(int fixtureId)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in claims.");
        }

        try
        {
            await _tipService.DeleteTipAsync(fixtureId, userId);
            return Ok("Tip deleted successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting tip");
            return StatusCode(500, "An error occurred while deleting the tip.");
        }
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateTip(int fixtureId, int homeScoreTip, int awayScoreTip)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized("User ID not found in claims.");
        }
        
        try
        {
            await _tipService.UpdateTipAsync(fixtureId, userId, homeScoreTip.ToString(), awayScoreTip.ToString());
            return Ok("Tip updated successfully.");
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tip");
            return StatusCode(500, "An error occurred while updating the tip.");
        }
    }

    [HttpGet("calculate")]
    public async Task<IActionResult> CalculatePointsForCompletedMatches()
    {
        try
        {
            await _tipService.CalculatePointsForCompletedMatchesAsync();
            return Ok("Points calculated for completed matches.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating points");
            return StatusCode(500, "An error occurred while calculating points.");
        }
    }
}