using ReservationManager.DomainModel.Operation;
using ReservationManager.Core.Interfaces.Repositories.Base;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IClosingCalendarRepository : ICrudBaseEntityRepository<ClosingCalendar>
    {
        Task<IEnumerable<ClosingCalendar>> GetAllFromToday();
        Task<IEnumerable<ClosingCalendar>> BulkCreateEntitiesAsync(IEnumerable<ClosingCalendar> entities);
        
        /// <summary>
        /// GetFiltered: used by FilterService and Validator for retrieve filtered ClosingCalendars.
        /// Not cached because need always data from db. 
        /// </summary>
        /// <remarks>Not cached</remarks>
        /// <returns>List of ClosingCalendar</returns>
        Task<IEnumerable<ClosingCalendar>> GetFiltered(int? id, DateOnly? fromDate, DateOnly? toDate,
            int? resourceId, int? resourceTypeId);
        
        
        /// <summary>
        /// GetExistingClosingCalendars: used by BulkCreateClosingCalendars.
        /// Not cached because need always data from db. 
        /// </summary>
        /// <remarks>Not cached</remarks>
        /// <returns>List of ClosingCalendar</returns>

        Task<IEnumerable<ClosingCalendar>> GetExistingClosingCalendars(IEnumerable<int> resourceIds,
            IEnumerable<DateOnly> days);
    }
}
