using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RestSharp;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeagueStandingsController : ControllerBase
{
    private const string ApiKey = "d5d539f3bcf60e8c70fbe510e5debcb2"; // Move to configuration
    private readonly IDistributedCache _cache;

    public LeagueStandingsController(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    [HttpGet("standings")]
    public async Task<IActionResult> GetStandings([FromQuery] int league = 2, [FromQuery] int season = 2024)
    {
        var cacheKey = $"standings_{league}_{season}";

        var cachedData = await _cache.GetStringAsync(cacheKey);
        if (!string.IsNullOrEmpty(cachedData))
        {
            var cachedJson = JsonSerializer.Deserialize<JsonElement>(cachedData);
            return Ok(cachedJson);
        }

        var options = new RestClientOptions("https://v3.football.api-sports.io")
        {
            Timeout = TimeSpan.FromMinutes(1)
        };

        var client = new RestClient(options);
        var request = new RestRequest("/standings", Method.Get);
        request.AddHeader("x-apisports-key", ApiKey);
        request.AddQueryParameter("league", league.ToString());
        request.AddQueryParameter("season", season.ToString());

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            await _cache.SetStringAsync(cacheKey, response.Content, cacheOptions);

            var json = JsonSerializer.Deserialize<JsonElement>(response.Content);
            return Ok(json);
        }

        return StatusCode((int)response.StatusCode, response.ErrorMessage);
    }
}