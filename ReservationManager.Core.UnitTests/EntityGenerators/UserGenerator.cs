using ReservationManager.Core.Consts;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace Tests.EntityGenerators;

public class UserGenerator
{
    public User GetBasicUser()
    {
        return new User
        {
            Email = "basic@mail.it",
            Name = "Basic",
            Surname = "Basic",
            Roles = new List<Role> { new Role { Name = "Employee", Code = FixedUserRole.Employee } }
        };
    }

    public List<User> GenerateList()
    {
        return new List<User>
        { 
            new User()
            {
                Email = "userwithroles@mail.it",
                Name = "RoleUser",
                Surname = "WithRoles",
                Roles = new List<Role> { new Role { Name = "Employee", Code = FixedUserRole.Employee } }
            },
            new User
            {
                Email = "basic@mail.it",
                Name = "Basic",
                Surname = "Basic",
                Roles = new List<Role> { new Role { Name = "Employee", Code = FixedUserRole.Employee } }
            }
        };
    }
}