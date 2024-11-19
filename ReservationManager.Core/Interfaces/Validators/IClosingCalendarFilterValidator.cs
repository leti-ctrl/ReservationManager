using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Validators
{
    public interface IClosingCalendarFilterValidator
    {
        bool IsLegalDateRange(ClosingCalendarFilterDto entity);
    }
}
