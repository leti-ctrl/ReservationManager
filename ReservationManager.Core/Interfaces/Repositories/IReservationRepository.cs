using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IReservationRepository : ICrudBaseEntityRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetReservationByUserIdFromToday(int userId);
        /// <summary>
        /// Userd by ResourceFilterService for retrieve filtered resources.
        /// Not cached because need always data from db. 
        /// </summary>
        /// <remarks>Not cached</remarks>
        Task<IEnumerable<Reservation>> GetReservationByResourceDateTimeAsync(List<int> resourceIds, DateOnly startDate,
            TimeOnly startTime, TimeOnly endTime);
        Task<IEnumerable<Reservation>> GetReservationByResourceIdAfterTodayAsync(int resourceId);
        Task<IEnumerable<Reservation>> GetReservationByTypeIdAfterTodayAsync(int typeId);
    }
}
