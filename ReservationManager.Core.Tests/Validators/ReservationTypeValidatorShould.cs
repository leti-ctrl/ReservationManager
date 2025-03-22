using NSubstitute;
using ReservationManager.Core.Validators;
using ReservationManager.DomainModel.Meta;

namespace Tests.Validators;

public class ReservationTypeValidatorShould
{
    private readonly ReservationTypeValidator _sut;

    public ReservationTypeValidatorShould()
    {
        _sut = new ReservationTypeValidator();
    }

    [Fact]
    public void Validate_ValidReservationType_ShouldReturnTrue()
    {
        var reservationType = new ReservationType()
        {
            Code = "TestCode",
            Name = "TestName",
            Start = new TimeOnly(15, 00, 00),
            End = new TimeOnly(16, 00, 00)
        };
        
        var result = _sut.Validate(reservationType);
        
        Assert.True(result.IsValid);
    }

    [Theory]
    [ClassData(typeof(ReservationTypeInvalidModel))]
    public void Validate_InvalidReservationType_ShouldReturnFalse(ReservationType reservationType)
    {
        var result = _sut.Validate(reservationType);
        
        Assert.False(result.IsValid);
    }
}

public class ReservationTypeInvalidModel : TheoryData<ReservationType>
{
    public ReservationTypeInvalidModel()
    {
        Add(new ReservationType()
        {
            Code = "",
            Name = "",
            Start = new TimeOnly(15, 00, 00),
            End = new TimeOnly(16, 00, 00)
        });
        Add(new ReservationType()
        {
            Code = null,
            Name = "TestName",
            Start = new TimeOnly(15, 00, 00),
            End = new TimeOnly(16, 00, 00)
        });
        Add(new ReservationType()
        {
            Code = "TestCode",
            Name = null,
            Start = new TimeOnly(15, 00, 00),
            End = new TimeOnly(16, 00, 00)
        });
        Add(new ReservationType()
        {
            Code = "TestCode",
            Name = "TestName",
            Start = new TimeOnly(18, 00, 00),
            End = new TimeOnly(16, 00, 00)
        });
        Add(new ReservationType()
        {
            Code = "TestCode",
            Name = "TestName",
            Start = new TimeOnly(),
            End = new TimeOnly()
        });
    }
}