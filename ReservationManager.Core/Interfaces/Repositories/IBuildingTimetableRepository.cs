using ReservationManager.DomainModel.Operation;
using ReservationManager.Core.Interfaces.Repositories.Base;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IBuildingTimetableRepository : ICrudBaseEntityRepository<BuildingTimetable>
    {
        Task<IEnumerable<BuildingTimetable>> GetAllTimetableFromToday();
        Task<IEnumerable<BuildingTimetable>> GetByTypeId(int typeId);
        Task<IEnumerable<BuildingTimetable>> GetByDateRange(DateOnly startDate, DateOnly endDate);
        Task<IEnumerable<BuildingTimetable>> GetClosingDateIntersection(DateOnly start, DateOnly end, int typeId);
        Task<IEnumerable<BuildingTimetable>> GetTimeReductionIntersection(DateOnly startDate, DateOnly endDate, 
            TimeOnly startTime, TimeOnly endTime, int typeId);
    }
}
