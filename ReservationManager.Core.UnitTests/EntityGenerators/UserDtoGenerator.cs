using ReservationManager.Core.Dtos;

namespace Tests.EntityGenerators;

public static class UserDtoGenerator
{
    public static UserDto GenerateValidUserDto()
    {
        var userEmail = "test@mail.com";
        var userId = 42;
        return new UserDto
        {
            Id = userId,
            Email = userEmail,
            Name = string.Empty,
            Surname = string.Empty,
        };
    }
}