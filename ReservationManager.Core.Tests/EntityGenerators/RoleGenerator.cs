using ReservationManager.DomainModel.Meta;
using ReservationManager.Core.Consts;

namespace Tests.EntityGenerators;

public class RoleGenerator
{
    public Role GetRole(string roleName, string roleCode)
    {
        return new Role { Name = roleName, Code = roleCode };
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