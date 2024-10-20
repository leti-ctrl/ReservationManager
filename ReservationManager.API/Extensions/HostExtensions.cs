using Microsoft.EntityFrameworkCore;

namespace ReservationManager.API.Extensions
{
    public static class HostExtensions
    {
        public static async Task UseMigration<T>(this IHost app) where T : DbContext
        {
            await using var scope = app.Services.CreateAsyncScope();

            await using var db = scope.ServiceProvider.GetRequiredService<T>();

            await db.Database.MigrateAsync();
        }
    }
}
