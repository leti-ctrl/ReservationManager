using ReservationManager.DomainModel.Meta;
using ReservationManager.Core.Commons;
using ReservationManager.Persistence;

namespace ReservationManager.API
{
    internal static class Seed
    {
        public static async Task UseSeed(this WebApplication app)
        {
            await using var scope = app.Services.CreateAsyncScope();

            await using var db = scope.ServiceProvider.GetRequiredService<ReservationManagerDbContext>();

            var dbSet = db.Set<TimetableType>();
            var roles = new List<TimetableType>()
            {
                new() { Code = FixedTimetableType.Closure },
                new() { Code = FixedTimetableType.Overtime },
                new() { Code = FixedTimetableType.Nominal },
            };
            foreach (var role in roles.Where(role => !dbSet.Any(x => x.Code == role.Code)))
            {
                dbSet.AddRange(role);
            }
            await db.SaveChangesAsync();

        }
    }
}
