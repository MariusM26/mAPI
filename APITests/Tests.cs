using Xunit;

namespace APITests
{
    public class Tests : IClassFixture<AppFactory>
    {
        public static readonly Guid TenantId = new("c6259e9c-f54a-46c8-b8d0-52bb5ed36ea6");

        public AppFactory _webAppFactory;

        public Client _client;

        public Tests(AppFactory webAppFactory)
        {
            _webAppFactory = webAppFactory;
            _client = new Client(webAppFactory);
        }


        [Fact]
        public async Task FirstTestAsync()
        {
            var abc = await _client.GetAsync();
        }
    }
}
