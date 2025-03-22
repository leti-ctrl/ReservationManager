using NSubstitute;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Validators;
using ReservationManager.DomainModel.Operation;

namespace Tests.Validators;

[Trait("Category", "Unit")]
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
        var resourceId = 1;
        var fromDate = new DateOnly(2020, 1, 1);
        var existingClosingCalendar = new List<ClosingCalendar>() { new ClosingCalendar() };
        _closingCalendarRepository.GetFiltered(null, fromDate, null, resourceId, null)
                                  .Returns(existingClosingCalendar);

        var result = await _sut.ExistingClosignCalendar(resourceId, fromDate, null);
        
        Assert.True(result);
    }
    
    [Fact]
    private async Task ReturnFalse_ExistingClosignCalendar_WhenNotExistingClosingCalendar()
    {
        var resourceId = 1;
        var fromDate = new DateOnly(2020, 1, 1);
        _closingCalendarRepository.GetFiltered(null, fromDate, null, resourceId, null)
                                  .Returns(Enumerable.Empty<ClosingCalendar>());

        var result = await _sut.ExistingClosignCalendar(resourceId, fromDate, null);
        
        Assert.False(result);
    }
    
    [Fact]
    private async Task ReturnFalse_ExistingClosignCalendar_WhenCheckEqualsClosingCalendar()
    {
        var closingCalendarId = 42;
        var fromDate = new DateOnly(2020, 1, 1);
        var existingClosingCalendar = new List<ClosingCalendar>() { new ClosingCalendar() { Id = closingCalendarId } };
        _closingCalendarRepository.GetFiltered( closingCalendarId, fromDate, null, null, null )
                                  .Returns(existingClosingCalendar);

        var result = await _sut.ExistingClosignCalendar(1, fromDate, closingCalendarId);

        Assert.False(result);
    }
    
    [Fact]
    private async Task ReturnTrue_ExistingClosignCalendar_WhenMultipleClosingCalendarExists()
    {
        var resourceId = 1;
        var fromDate = new DateOnly(2020, 1, 1);
        var existingClosingCalendar = new List<ClosingCalendar>() { new ClosingCalendar() };
        _closingCalendarRepository.GetFiltered(null, fromDate, null, resourceId, null)
            .Returns(existingClosingCalendar);

        var result = await _sut.ExistingClosignCalendar(resourceId, fromDate, null);
        
        Assert.True(result);
    }
}