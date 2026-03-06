using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RestSharp;
using WebAPI.Client;
using WebAPI.Interfaces;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FixtureController : ControllerBase
{
    private readonly IFixtureDataService _fixtureDataService;
    
    public FixtureController(IFixtureDataService fixtureDataService)
    {
        _fixtureDataService = fixtureDataService;
    }

    [HttpGet("results")]
    public async Task<IActionResult> GetFixtureResults([FromQuery] int league = 39, [FromQuery] int season = 2022)
    {
        var fixtureData = await _fixtureDataService.GetFixtureDataAsync(league, season);
        return Ok(fixtureData);
    }

    [HttpGet("resultsbyid")]
    public async Task<IActionResult> GetFixtureById([FromQuery] int fixtureId)
    {
        var fixtureData = await _fixtureDataService.GetFixtureDataByTeamAsync(fixtureId);
        if (fixtureData == null)
        {
            return NotFound();
        }

        return Ok(fixtureData);
    }

    [HttpGet("resultsbydate")]
    public async Task<IActionResult> GetFixtureByDate([FromQuery] DateTime date, [FromQuery] int league = 39,
        [FromQuery] int season = 2022)
    {
        var fixtureData = await _fixtureDataService.GetFixtureDataByDateAsync(date, league, season);
        return Ok(fixtureData);
    }
}