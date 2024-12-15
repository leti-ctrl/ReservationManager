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
        };
    }
}