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

        //Add fixed user role
        var roleTable = db.Set<Role>();
        var roles = new List<Role>()
        {
            new() { Code = FixedUserRole.Employee, Name = "Employee" },
            new() { Code = FixedUserRole.FacilityManagement, Name = "Facility Management" },
            new() { Code = FixedUserRole.GeneralServices, Name = "General Services" },
            new() { Code = FixedUserRole.HumanResources, Name = "Human Resources" },
            new() { Code = FixedUserRole.Admin, Name = "Admin" }
        };
        foreach (var role in roles.Where(role => !roleTable.Any(x => x.Code == role.Code)))
        {
            roleTable.AddRange(role);
        }
        
        //Add standard reservation type 
        var reservationTypeTable = db.Set<ReservationType>();
        var reservationType = new ReservationType()
        {
            Code = FixedReservationType.Customizable,
            Name = "Customizable reservation time",
            Start = TimeOnly.MinValue,
            End = TimeOnly.MaxValue,
        };
        if(!reservationTypeTable.Any(x => x.Code == reservationType.Code))
            reservationTypeTable.Add(reservationType);
        
        await db.SaveChangesAsync();
    }
}