using Microsoft.EntityFrameworkCore;
using ReservationManager.Core.Consts;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
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
        var adminRole = new Role() { Code = FixedUserRole.Admin, Name = "Admin" };
        var roles = new List<Role>()
        {
            new() { Code = FixedUserRole.Employee, Name = "Employee" },
            new() { Code = FixedUserRole.FacilityManagement, Name = "Facility Management" },
            new() { Code = FixedUserRole.GeneralServices, Name = "General Services" },
            new() { Code = FixedUserRole.HumanResources, Name = "Human Resources" },
            adminRole
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
            CreatedOn = DateTime.UtcNow
        };
        if(!reservationTypeTable.IgnoreQueryFilters().Any(x => x.Code == reservationType.Code))
            reservationTypeTable.Add(reservationType);

        var userTable = db.Set<User>();
        var adminUser = new User()
        {
            Name = "Admin",
            Surname = "Admin",
            Email = "admin@admin.com",
            CreatedOn = DateTime.UtcNow,
            Roles = new List<Role>() { adminRole }
        };
        if(!userTable.IgnoreQueryFilters().Any(x => x.Email == adminUser.Email))
            userTable.Add(adminUser);
        
        await db.SaveChangesAsync();// Anche per gli altri controlli:
    }
}