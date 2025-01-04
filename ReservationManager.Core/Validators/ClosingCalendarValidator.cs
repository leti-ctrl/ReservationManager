using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Validators;

namespace ReservationManager.Core.Validators;

public class ClosingCalendarValidator : IClosingCalendarValidator
{
    private readonly IClosingCalendarRepository _closingCalendarRepository;

    public ClosingCalendarValidator(IResourceValidator resourceValidator, IClosingCalendarRepository closingCalendarRepository)
    {
        _closingCalendarRepository = closingCalendarRepository;
    }

    public async Task<bool> ValidateIfAlreadyExistsClosingCalendar(ClosingCalendarDto closingCalendarDto, int? id)
    {
           
        var closingCalendars = await _closingCalendarRepository.GetFiltered(null, 
            closingCalendarDto.Day, null, closingCalendarDto.ResourceId, null);
        if (id == null)
            return closingCalendars.Any(); 
        return closingCalendars.Any(x => x.Id != id);
    }

    public bool ValidateClosingCalendarBucket(BulkClosingCalendarDto bulkClosingCalendar)
    {
        return bulkClosingCalendar.From <= bulkClosingCalendar.To;
    }
}