using ReservationManager.DomainModel.Operation;
using ReservationManager.Core.Interfaces.Repositories.Base;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IClosingCalendarRepository : ICrudBaseEntityRepository<ClosingCalendar>
    {
        Task<IEnumerable<ClosingCalendar>> GetAllFromToday();
        
        /// <summary>
        /// Used by FilterService and Validator for retrieve filtered ClosingCalendars.
        /// Not cached because need always data from db. 
        /// </summary>
        /// <remarks>Not cached</remarks>
        Task<IEnumerable<ClosingCalendar>> GetFiltered(int? id, DateOnly? fromDate, DateOnly? toDate,
            int? resourceId, int? resourceTypeId);
        
        Task<IEnumerable<ClosingCalendar>> BulkCreateEntitiesAsync(IEnumerable<ClosingCalendar> entities);
        
        /// <summary>
        /// Used by BulkCreateClosingCalendars.
        /// Not cached because need always data from db. 
        /// </summary>
        /// <remarks>Not cached</remarks>
        Task<IEnumerable<ClosingCalendar>> GetExistingClosingCalendars(IEnumerable<int> resourceIds,
            IEnumerable<DateOnly> days);
    }
}
