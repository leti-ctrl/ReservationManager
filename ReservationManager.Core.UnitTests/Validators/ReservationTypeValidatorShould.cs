using FluentAssertions;
using NSubstitute;
using ReservationManager.Core.Validators;
using ReservationManager.DomainModel.Meta;
using Tests.EntityGenerators;

namespace Tests.Validators;

[Trait("Category", "Unit")]
public class ReservationTypeValidatorShould
{
    private readonly ReservationTypeValidator _sut = new();

    [Fact]
    public void ReturnTrue_Validate_WhenValidReservationType()
    {
        var reservationType = new ReservationType()
        {
            Code = "TestCode",
            Name = "TestName",
            Start = new TimeOnly(15, 00, 00),
            End = new TimeOnly(16, 00, 00)
        };
        
        var result = _sut.Validate(reservationType);
        
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(ReservationTypeInvalidModelGenerator))]
    public void ReturnFalse_Validate_InvalidReservationType(ReservationType reservationType)
    {
        var result = _sut.Validate(reservationType);
        
        result.IsValid.Should().BeFalse();
    }
}

