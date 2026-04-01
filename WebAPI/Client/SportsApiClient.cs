using System.Text.Json;
using RestSharp;
using WebAPI.Models.RawFixtureResponse;

namespace WebAPI.Client;

public class SportsApiClient : ISportsApiClient
{
    private const string ApiKey = "d5d539f3bcf60e8c70fbe510e5debcb2"; // Move to configuration
    
    public Task<RawFixturesResponse> GetUpcomingFixturesAsync(int leagueId)
    {
        throw new NotImplementedException();
    }

    public async Task<RawFixturesResponse> GetAllFixturesByLeagueAsync(int leagueId, int seasonByYear, CancellationToken cancellationToken)
    {
        var options = new RestClientOptions("https://v3.football.api-sports.io")
        {
            Timeout = TimeSpan.FromMinutes(1)
        };
        
        var client = new RestClient(options);
        var request = new RestRequest("/fixtures", Method.Get);
        request.AddHeader("x-apisports-key", ApiKey);
        request.AddQueryParameter("league", leagueId.ToString());
        request.AddQueryParameter("season", seasonByYear.ToString());

        var response = await client.ExecuteAsync(request, cancellationToken);
        
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        Console.WriteLine(response.Content);
        
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var fixturesResponse = JsonSerializer.Deserialize<RawFixturesResponse>(response.Content, jsonOptions);
            return fixturesResponse;
        }
        
        throw new Exception($"API request failed with status code {response.StatusCode}: {response.Content}");
    }

    public async Task<RawFixturesResponse> GetTheUpcomingFixturesByLeagueAsync(int leagueId, int seasonByYear, int next,
        CancellationToken cancellationToken)
    {
        var options = new RestClientOptions("https://v3.football.api-sports.io")
        {
            Timeout = TimeSpan.FromMinutes(1)
        };
        
        var client = new RestClient(options);
        var request = new RestRequest("/fixtures", Method.Get);
        request.AddHeader("x-apisports-key", ApiKey);
        request.AddQueryParameter("league", leagueId.ToString());
        request.AddQueryParameter("season", seasonByYear.ToString());
        request.AddQueryParameter("next", next.ToString());

        var response = await client.ExecuteAsync(request, cancellationToken);
        
        var jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        Console.WriteLine(response.Content);
        
        if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
        {
            var fixturesResponse = JsonSerializer.Deserialize<RawFixturesResponse>(response.Content, jsonOptions);
            return fixturesResponse;
        }
        
        throw new Exception($"API request failed with status code {response.StatusCode}: {response.Content}");
    }
}