using NSubstitute;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.Core.Validators;
using Tests.EntityGenerators;

namespace Tests.Validators;

[Trait("Category", "Unit")]
public class ClosingCalendarFilterValidatorShould
{
    private readonly ClosingCalendarFilterDtoValidator _sut;

    public ClosingCalendarFilterValidatorShould()
    {
        _sut = new ClosingCalendarFilterDtoValidator();
    }

    [Theory]
    [ClassData(typeof(ClosingCalendarFilterDtoValidModelGenerator))]
    public void ReturnTrue_IsLegalDateRange_WhenDateOnlyIsValid(ClosingCalendarFilterDto filter)
    {
        var result = _sut.Validate(filter);
        
        Assert.True(result.IsValid);
    }
    
    
    [Theory]
    [ClassData(typeof(ClosingCalendarFilterDtoInvalidModelGenerator))]
    public void ReturnFalse_IsLegalDateRange_WhenDateOnlyIsNotValid(ClosingCalendarFilterDto filter)
    {
        var result = _sut.Validate(filter);
        
        Assert.False(result.IsValid);
    }
}



