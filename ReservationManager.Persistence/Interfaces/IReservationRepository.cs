using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Interfaces
{
    public interface IReservationRepository : ICrudEntityRepository<Reservation>
    {
        Task<IEnumerable<Reservation>> GetByDayAsync(DateOnly day);
        Task<IEnumerable<Reservation>> GetByResourceAsync(int resourceId);
        Task<IEnumerable<Reservation>> GetByUserAsync(int userId);
        Task<IEnumerable<Reservation>> GetByTypeAsync(string code);
    }
}
