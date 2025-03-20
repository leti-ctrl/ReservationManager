using NSubstitute;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.Core.Validators;

namespace Tests.Validators;

[Trait("Category", "Unit")]
public class ClosingCalendarFilterValidatorShould
{
    private readonly IClosingCalendarFilterValidator _sut;

    public ClosingCalendarFilterValidatorShould()
    {
        _sut = new ClosingCalendarFilterValidator();
    }

    [Theory]
    [ClassData(typeof(ValidDateOnlyData))]
    public void ReturnTrue_IsLegalDateRange_WhenDateOnlyIsValid(DateOnly? start, DateOnly? end)
    {
        var filter = new ClosingCalendarFilterDto()
        {
            StartDay = start,
            EndDay = end,
        };
        
        var result = _sut.IsLegalDateRange(filter);
        
        Assert.True(result);
    }
    
    
    [Theory]
    [ClassData(typeof(InvalidDateOnlyData))]
    public void ReturnFalse_IsLegalDateRange_WhenDateOnlyIsNotValid(DateOnly? start, DateOnly? end)
    {
        var filter = new ClosingCalendarFilterDto()
        {
            StartDay = start,
            EndDay = end,
        };
        
        var result = _sut.IsLegalDateRange(filter);
        
        Assert.False(result);
    }
}

public class ValidDateOnlyData : TheoryData<DateOnly?, DateOnly?>
{
    public ValidDateOnlyData()
    {
        Add(null, null);
        Add(new DateOnly(2020, 01, 01), null);
        Add(new DateOnly(2020, 01, 01), new DateOnly(2020, 01, 01));
        Add(new DateOnly(2020, 01, 01), new DateOnly(2021, 01, 01));
    }
};

public class InvalidDateOnlyData : TheoryData<DateOnly?, DateOnly?>
{
    public InvalidDateOnlyData()
    {
        Add(null, new DateOnly(2020, 01, 01));
        Add(new DateOnly(2021, 01, 01), new DateOnly(2020, 01, 01));
    }
};