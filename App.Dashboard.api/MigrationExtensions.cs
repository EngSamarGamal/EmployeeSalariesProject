using App.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace App.Api
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbContext = services.GetRequiredService<ApplicationDbContext>();
                    //dbContext.Database.Migrate(); // Apply any pending migrations
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while applying migrations.");
                    throw;
                }
            }
        }
    }
}
