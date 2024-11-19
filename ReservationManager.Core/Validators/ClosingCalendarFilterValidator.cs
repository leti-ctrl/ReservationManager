using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Validators;

namespace ReservationManager.Core.Validators
{
    public class ClosingCalendarFilterValidator : IClosingCalendarFilterValidator
    {
        public bool IsLegalDateRange(ClosingCalendarFilterDto entity)
        {
            if (entity is { StartDay: null, EndDay: null } or { StartDay: not null, EndDay: null })
                return true;
            
            if (entity is { StartDay: not null, EndDay: not null })
                return (DateOnly)entity.StartDay <= (DateOnly)entity.EndDay;

            return false;
        }

    }
}
