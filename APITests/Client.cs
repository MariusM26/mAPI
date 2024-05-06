using Azure.Core;
using mAPI.Database.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace APITests;

public class Client
{
    private readonly AppFactory _webAppFactory;
    private readonly JsonSerializerOptions _jsonSerializerOptions;
    private readonly HttpClient _client;

    public Client(AppFactory webAppFactory)
    {
        _webAppFactory = webAppFactory;
        _jsonSerializerOptions = new JsonSerializerOptions()
        {
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            },
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        _client = _webAppFactory.CreateClient();
    }

    public async Task<List<DCandidate>?> GetAsync()
    {
        var loginEp = "/login";

        using var loginResponse = await _client.PostAsJsonAsync(loginEp, new User()
        {
            Email = "test@test.test",
            Password = "Pass123$"
        });

        var authorization = await loginResponse.Content.ReadFromJsonAsync<Authorization>(_jsonSerializerOptions);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization!.TokenType, authorization.AccessToken);

        var endpoint = $"/api/DCandidate";

        using var response = await _client.GetAsync(endpoint);

        var abc = await response.Content.ReadAsStringAsync();

        var result = await response.Content.ReadFromJsonAsync<List<DCandidate>>(_jsonSerializerOptions);

        return result;
    }
}