using ReservationManager.DomainModel.Meta;
using ReservationManager.Core.Consts;

namespace Tests.EntityGenerators;

public class RoleGenerator
{
    public List<Role> GetAllRoles()
    {
        return new List<Role>()
        {
            new Role() {Code = FixedUserRole.Employee, Name = "Employee"},
            new Role() {Code = FixedUserRole.Admin, Name = "Admin"},
            new Role() {Code = FixedUserRole.HumanResources, Name = "Human Resources"},
            new Role() {Code = FixedUserRole.FacilityManagement, Name = "Facility Management"},
            new Role() {Code = FixedUserRole.GeneralServices, Name = "General Services"},
        };
    }

    public Role GetAdminRole()
    {
        return new Role { Name = "Admin", Code = FixedUserRole.Admin };
    }

    public Role GetEmployeeRole()
    {
        return new Role { Name = "Employee", Code = FixedUserRole.Employee };
    }
}