using NSubstitute;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Validators;
using ReservationManager.DomainModel.Operation;

namespace Tests.Validators;

public class ClosingCalendarValidatorShould
{
    private readonly ClosingCalendarValidator _sut;
    
    private readonly IClosingCalendarRepository _closingCalendarRepository;

    public ClosingCalendarValidatorShould()
    {
        _closingCalendarRepository = Substitute.For<IClosingCalendarRepository>();
        
        _sut = new ClosingCalendarValidator(_closingCalendarRepository);
    }

    [Fact]
    private async Task ReturnTrue_ExistingClosignCalendar_WhenExistingClosingCalendar()
    {
        var existingClosingCalendar = new List<ClosingCalendar>() { new ClosingCalendar() };
        _closingCalendarRepository.GetFiltered(Arg.Any<int?>(), 
                                               Arg.Any<DateOnly?>(), 
                                               Arg.Any<DateOnly?>(), 
                                               Arg.Any<int?>(), 
                                               Arg.Any<int?>())
                                  .Returns(existingClosingCalendar);

        var result = await _sut.ExistingClosignCalendar(1, new DateOnly(2020, 1, 1), null);
        
        Assert.True(result);
    }
    
    [Fact]
    private async Task ReturnFalse_ExistingClosignCalendar_WhenNotExistingClosingCalendar()
    {
        _closingCalendarRepository.GetFiltered(Arg.Any<int?>(), 
                                               Arg.Any<DateOnly?>(), 
                                               Arg.Any<DateOnly?>(), 
                                               Arg.Any<int?>(), 
                                               Arg.Any<int?>())
                                  .Returns(Enumerable.Empty<ClosingCalendar>());

        var result = await _sut.ExistingClosignCalendar(1, new DateOnly(2020, 1, 1), null);
        
        Assert.False(result);
    }
    
    [Fact]
    private async Task ReturnFalse_ExistingClosignCalendar_WhenCheckEqualsClosingCalendar()
    {
        var closingCalendarId = 42;
        var existingClosingCalendar = new List<ClosingCalendar>() { new ClosingCalendar() { Id = closingCalendarId } };
        _closingCalendarRepository.GetFiltered( Arg.Any<int?>(),      
            Arg.Any<DateOnly?>(),  
            Arg.Any<DateOnly?>(),  
            Arg.Any<int?>(),       
            Arg.Any<int?>()).Returns(existingClosingCalendar);

        var result = await _sut.ExistingClosignCalendar(1, new DateOnly(2020, 1, 1), closingCalendarId);

        Assert.False(result);
    }
    
    [Fact]
    private async Task ReturnTrue_ExistingClosignCalendar_WhenMultipleClosingCalendarExists()
    {
        var existingClosingCalendar = new List<ClosingCalendar>() { new ClosingCalendar() };
        _closingCalendarRepository.GetFiltered(Arg.Any<int?>(), 
                Arg.Any<DateOnly?>(), 
                Arg.Any<DateOnly?>(), 
                Arg.Any<int?>(), 
                Arg.Any<int?>())
            .Returns(existingClosingCalendar);

        var result = await _sut.ExistingClosignCalendar(1, new DateOnly(2020, 1, 1), null);
        
        Assert.True(result);
    }
}