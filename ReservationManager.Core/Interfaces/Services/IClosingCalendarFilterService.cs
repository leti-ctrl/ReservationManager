using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services;

public interface IClosingCalendarFilterService
{
    Task<IEnumerable<ClosingCalendarDto>> GetFiltered(ClosingCalendarFilterDto filter);
}