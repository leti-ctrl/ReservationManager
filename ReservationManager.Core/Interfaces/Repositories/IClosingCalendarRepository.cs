using ReservationManager.DomainModel.Operation;
using ReservationManager.Core.Interfaces.Repositories.Base;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IClosingCalendarRepository : ICrudBaseEntityRepository<ClosingCalendar>
    {
        Task<IEnumerable<ClosingCalendar>> GetAllTimetableFromToday();
        Task<IEnumerable<ClosingCalendar>> GetByTypeId(int typeId);
        Task<IEnumerable<ClosingCalendar>> GetByDateRange(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<ClosingCalendar>> GetClosingDateIntersection(DateOnly start, DateOnly end, int typeId);
        Task<IEnumerable<ClosingCalendar>> GetTimeReductionIntersection(DateOnly startDate, DateOnly endDate, 
            TimeOnly startTime, TimeOnly endTime, int typeId);
    }
}
