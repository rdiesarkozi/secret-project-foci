using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using RestSharp;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FixtureController : ControllerBase
{
    private const string ApiKey = "d5d539f3bcf60e8c70fbe510e5debcb2"; // Move to configuration
    private readonly IDistributedCache _distributedCache;

    public FixtureController(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    [HttpGet("results")]
    public async Task<IActionResult> GetFixtureResults([FromQuery] int league = 39, [FromQuery] int season = 2022)
    {
        var cacheKey = $"fixture_results_{league}_{season}";
        var cachedData = await _distributedCache.GetStringAsync(cacheKey);
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
        var request = new RestRequest("/fixtures", Method.Get);
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
            await _distributedCache.SetStringAsync(cacheKey, response.Content, cacheOptions);

            var json = JsonSerializer.Deserialize<JsonElement>(response.Content);
            return Ok(json);
        }
        
        return StatusCode((int)response.StatusCode, response.Content);
    }

}