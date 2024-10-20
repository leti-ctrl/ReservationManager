using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces.Base;

namespace ReservationManager.Persistence.Interfaces
{
    public interface IReservationRepository : ICrudBaseEntityRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetByDayAsync(DateOnly day);
        Task<IEnumerable<Reservation>> GetByResourceAsync(int resourceId);
        Task<IEnumerable<Reservation>> GetByUserAsync(int userId);
        Task<IEnumerable<Reservation>> GetByTypeAsync(string code);
    }
}
