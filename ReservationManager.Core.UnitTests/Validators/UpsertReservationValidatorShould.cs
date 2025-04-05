using FluentAssertions;
using NSubstitute;
using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Validators;
using ReservationManager.DomainModel.Meta;

namespace Tests.Validators;

[Trait("Category", "Unit")]
public class UpsertReservationValidatorShould
{
    private readonly UpsertReservationValidator _sut = new();

    [Fact]
    public void ReturnFalse_WhenReservationDateInThePast()
    {
        var upserRez = new UpsertReservationDto
        {
            Title = "title",
            Day = new DateOnly(2020, 01, 01) 
        };
        var rezType = new ReservationType() { Code = "TEST" };
        
        var result = _sut.IsDateRangeValid(upserRez, rezType);
        
        result.Should().BeFalse();
    }

    [Theory]
    [ClassData(typeof(InvalidTimeOnlyGenerator))]
    public void ReturnFalse_WhenRezTypeIsCustomizableAndUpsertReservationHasInvalidTime(TimeOnly? start, TimeOnly? end)
    {
        var rezType = new ReservationType() { Code = FixedReservationType.Customizable };
        var upserRez = new UpsertReservationDto
        {
            Title = "title",
            Day = DateOnly.FromDateTime(DateTime.Today),
            Start = start,
            End = end,
        };
        
        var result = _sut.IsDateRangeValid(upserRez, rezType);
        
        result.Should().BeFalse();
    }
    
    [Fact]
    public void ReturnTrue_WhenReservationTypeIsNotCustomizableAndReservationHasNotTime()
    {
        var rezType = new ReservationType() { Code = "TEST" };
        var upserRez = new UpsertReservationDto
        {
            Title = "title",
            Day = DateOnly.FromDateTime(DateTime.Today),
        };
        
        var result = _sut.IsDateRangeValid(upserRez, rezType);
        
        result.Should().BeTrue();
    }
    
    [Fact]
    public void ReturnFalse_WhenReservationTypeIsNotCustomizableAndReservationHasTime()
    {
        var rezType = new ReservationType() { Code = "TEST" };
        var upserRez = new UpsertReservationDto
        {
            Title = "title",
            Day = DateOnly.FromDateTime(DateTime.Today),
            Start = new TimeOnly(15, 00 ,00 ),
            End = new TimeOnly(16, 00, 00),
        };
        
        var result = _sut.IsDateRangeValid(upserRez, rezType);
        
        result.Should().BeFalse();
    }
}

public class InvalidTimeOnlyGenerator : TheoryData<TimeOnly?, TimeOnly?>
{
    public InvalidTimeOnlyGenerator()
    {
        Add( null, null );
        Add(new TimeOnly(15, 00, 00), null);
        Add(null, new TimeOnly(15, 00, 00));
        Add(new TimeOnly(15, 00, 00), new TimeOnly(15, 00, 00));
        Add(new TimeOnly(21, 00, 00), new TimeOnly(15, 00, 00));
    }
}