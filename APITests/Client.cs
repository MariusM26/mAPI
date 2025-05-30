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
        var registerEp = "/register"; // sau endpointul tău real, ex: "/api/account/register"
        var user = new User()
        {
            Email = "test@test.test",
            Password = "Pass123$"
        };

        // Încearcă să înregistrezi utilizatorul
        using (var registerResponse = await _client.PostAsJsonAsync(registerEp, user))
        {
            // Dacă userul există deja, ignoră eroarea
            if (!registerResponse.IsSuccessStatusCode &&
                registerResponse.StatusCode != System.Net.HttpStatusCode.Conflict &&
                registerResponse.StatusCode != System.Net.HttpStatusCode.BadRequest)
            {
                // Dacă e altă eroare, aruncă excepție
                registerResponse.EnsureSuccessStatusCode();
            }
        }

        var loginEp = "/login";

        using var loginResponse = await _client.PostAsJsonAsync(loginEp, user);

        var authorization = await loginResponse.Content.ReadFromJsonAsync<Authorization>(_jsonSerializerOptions);

        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authorization?.TokenType!, authorization?.AccessToken);

        var endpoint = $"/api/DCandidate";

        using var response = await _client.GetAsync(endpoint);

        var result = await response.Content.ReadFromJsonAsync<List<DCandidate>>(_jsonSerializerOptions);

        return result;
    }
}