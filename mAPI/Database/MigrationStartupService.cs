using Microsoft.EntityFrameworkCore;

namespace mAPI.Database;
public class MigrationStartupService<TContext> : IStartupFilter where TContext : DbContext
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                scope.ServiceProvider.GetServices<TContext>().FirstOrDefault()?.Database.Migrate();
            }

            next(app);
        };
    }
}
