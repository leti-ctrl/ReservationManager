using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Interfaces
{
    public interface IEstabilishmentTimetableRepository : ICrudBaseEntityRepository<EstabilishmentTimetable>
    {
        Task<IEnumerable<EstabilishmentTimetable>> GetByTypeId(int typeId);
        Task<IEnumerable<EstabilishmentTimetable>> GetClosingDateIntersection(DateOnly start, DateOnly end, int typeId);
        Task<IEnumerable<EstabilishmentTimetable>> GetTimeReductionIntersection(DateOnly startDate, DateOnly endDate, 
            TimeOnly startTime, TimeOnly endTime, int typeId);
    }
}
