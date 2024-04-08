using System.Text.Json.Serialization;
using System.Text.Json;
using System.Net.Http.Json;
using Xunit;
using Microsoft.AspNetCore.Hosting;

namespace IntegrationTests
{
    public class CustomWebApplicationFactory : IClassFixture<IntegrationTestsFactory<Program>>
    {
        private readonly IntegrationTestsFactory<Program> _webAppFactory;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly HttpClient _client;

        public CustomWebApplicationFactory(IntegrationTestsFactory<Program> webAppFactory)
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


        [Fact]
        public async Task/*<GetResponseWrapper> */Get()
        {
            var endpoint = $"/api/DCandidate";

            using var response = await _client.GetAsync(endpoint);

            var result = await response.Content.ReadFromJsonAsync<GetResponseWrapper>(_jsonSerializerOptions);

            //return result!;
        }

    }
}
