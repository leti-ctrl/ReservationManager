using ReservationManager.Core.Consts;
using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence;

namespace ReservationManager.API;

internal static class Seed
{
    public static async Task UseSeed(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        await using var db = scope.ServiceProvider.GetRequiredService<ReservationManagerDbContext>();

        var dbSet = db.Set<Role>();
        var roles = new List<Role>()
        {
            new() { Code = FixedUserRole.Employee, Name = "Employee" },
            new() { Code = FixedUserRole.FacilityManagement, Name = "Facility Management" },
            new() { Code = FixedUserRole.GeneralServices, Name = "General Services" },
            new() { Code = FixedUserRole.HumanResources, Name = "Human Resources" },
            new() { Code = FixedUserRole.Admin, Name = "Admin" }
        };
        foreach (var role in roles.Where(role => !dbSet.Any(x => x.Code == role.Code)))
        {
            dbSet.AddRange(role);
        }
        await db.SaveChangesAsync();

    }
}