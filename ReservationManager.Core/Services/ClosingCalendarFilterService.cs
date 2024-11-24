using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;

namespace ReservationManager.Core.Services;

public class ClosingCalendarFilterService : IClosingCalendarFilterService
{
    private readonly IClosingCalendarFilterValidator _closingCalendarFilterValidator;
    private readonly IClosingCalendarRepository _closingCalendarRepository;


    public ClosingCalendarFilterService(IClosingCalendarFilterValidator closingCalendarFilterValidator, IClosingCalendarRepository closingCalendarRepository)
    {
        _closingCalendarFilterValidator = closingCalendarFilterValidator;
        _closingCalendarRepository = closingCalendarRepository;
    }
    
    public async Task<IEnumerable<ClosingCalendarDto>> GetFiltered(ClosingCalendarFilterDto filter)
    {
        if (!_closingCalendarFilterValidator.IsLegalDateRange(filter))
            throw new InvalidFiltersException("You cannot set end date without a start date.");

        var closingCalendars = (await _closingCalendarRepository.GetFiltered(filter.Id,
            filter.StartDay, filter.EndDay, filter.RescourceId, filter.ResourceTypeId)).ToList();

        if (closingCalendars.Any())
            return closingCalendars.Select(x => x.Adapt<ClosingCalendarDto>()).OrderBy(x => x.Day);
        return Enumerable.Empty<ClosingCalendarDto>();
    }
}