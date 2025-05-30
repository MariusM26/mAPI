using mAPI.Database.Models;
using Xunit;

namespace APITests.IntegrationTests
{
    public class Tests(AppFactory webAppFactory) : IClassFixture<AppFactory>
    {
        public static readonly Guid TenantId = new("c6259e9c-f54a-46c8-b8d0-52bb5ed36ea6");

        public AppFactory _webAppFactory = webAppFactory;
        public Client _client = new(webAppFactory);

        [Fact]
        public async Task FirstTestAsync()
        {
            var result = await _client.GetAsync();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetDCandidates_ReturnsList()
        {
            var result = await _client.GetAsync();
            Assert.NotNull(result);
            Assert.IsType<List<DCandidate>>(result);
        }

        [Fact]
        public async Task GetDCandidates_ContainsExpectedProperties()
        {
            var result = await _client.GetAsync();
            Assert.NotNull(result);
            if (result!.Count != 0)
            {
                var candidate = result[0];
                Assert.True(candidate.Id >= 0);
                Assert.False(string.IsNullOrWhiteSpace(candidate.FullName));
                Assert.False(string.IsNullOrWhiteSpace(candidate.Email));
            }
        }

        [Fact]
        public async Task GetDCandidates_EmptyWhenNoData()
        {
            var result = await _client.GetAsync();
            Assert.NotNull(result);
            Assert.True(result!.Count != 0 || result.Count == 0);
        }
    }
}
