using FluentValidation;
using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.Core.Interfaces.Validators;

namespace ReservationManager.Core.Services;

public class ClosingCalendarFilterService(
    IClosingCalendarFilterDtoValidator closingCalendarFilterDtoValidator,
    IClosingCalendarRepository closingCalendarRepository)
    : IClosingCalendarFilterService
{
    public async Task<IEnumerable<ClosingCalendarDto>> GetFiltered(ClosingCalendarFilterDto filter)
    {
        if (!closingCalendarFilterDtoValidator.Validate(filter).IsValid)
            throw new InvalidFiltersException("You cannot set end date without a start date.");

        var closingCalendars = (await closingCalendarRepository.GetFiltered(filter.Id,
            filter.StartDay, filter.EndDay, filter.RescourceId, filter.ResourceTypeId)).ToList();

        if (closingCalendars.Any())
            return closingCalendars.Select(x => x.Adapt<ClosingCalendarDto>()).OrderBy(x => x.Day);
        return Enumerable.Empty<ClosingCalendarDto>();
        
    }
}