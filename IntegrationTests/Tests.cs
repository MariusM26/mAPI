using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTests
{
    public class Tests(CustomWebApplicationFactory testClient) : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _testClient = testClient;

        [Fact]
        public async Task FirstTest()
        {
            await _testClient.Get();
        }
    }
}
