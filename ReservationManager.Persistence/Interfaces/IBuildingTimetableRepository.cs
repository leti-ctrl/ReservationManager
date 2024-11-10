using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Interfaces
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
