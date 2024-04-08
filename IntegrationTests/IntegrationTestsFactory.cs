using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Hosting;

public class IntegrationTestsFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseStartup<TStartup>().UseContentRoot(GetApplicationRootPath());
            });
    }

    private string GetApplicationRootPath()
    {
        var projectName = typeof(TStartup).Assembly.GetName().Name;
        var applicationBasePath = AppDomain.CurrentDomain.BaseDirectory;
        var directoryInfo = new DirectoryInfo(applicationBasePath);

        do
        {
            directoryInfo = directoryInfo.Parent;

            var projectDirectoryInfo = directoryInfo?.GetDirectories(projectName, SearchOption.AllDirectories).FirstOrDefault();
            if (projectDirectoryInfo != null)
            {
                return projectDirectoryInfo.FullName;
            }
        }
        while (directoryInfo?.Parent != null);

        throw new Exception($"Project root could not be located using the application root {applicationBasePath}.");
    }
}
