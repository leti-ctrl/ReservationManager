using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace Tests.EntityGenerators;

public static class ReservationGenerator
{
    public static Reservation GenerateValidGivenRezIdAndUserDto(int rezId, UserDto userDto)
    {
        return new Reservation
        {
            Id = rezId,
            UserId = userDto.Id,
            Title = "Test Title 1",
            User = new User { Id = userDto.Id, Email = userDto.Email },
            Type = new ReservationType { Code = "TEST" },
            Resource = new Resource { Description = "Test Description 1" }
        };
    }
    
    
}