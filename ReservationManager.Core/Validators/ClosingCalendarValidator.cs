using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Validators;

namespace ReservationManager.Core.Validators;

public class ClosingCalendarValidator : IClosingCalendarValidator
{
    private readonly IClosingCalendarRepository _closingCalendarRepository;

    public ClosingCalendarValidator(IClosingCalendarRepository closingCalendarRepository)
    {
        _closingCalendarRepository = closingCalendarRepository;
    }

    public async Task<bool> ExistingClosignCalendar(int resourceId, DateOnly day, int? closingCalendarId)
    {
        var closingCalendars = await _closingCalendarRepository.GetFiltered(null, day, null, resourceId, null);
        if (closingCalendarId == null)
            return closingCalendars.Any(); 
        return closingCalendars.Any(x => x.Id != closingCalendarId);
    }
}