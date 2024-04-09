using mAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

namespace APITests;

public class AppFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureClient(HttpClient client)
    {
        client.BaseAddress = new Uri("https://localhost:5187");
        base.ConfigureClient(client);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        var host = builder.Build();
        Task.Run(() => host.StartAsync()).GetAwaiter().GetResult();

        return host;
    }
}