using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Interfaces
{
    public interface IReservationRepository : ICrudRepository<Reservation>
    {
        IEnumerable<Reservation> GetByDay(DateOnly day);
        IEnumerable<Reservation> GetByResource(int resourceId);
        IEnumerable<Reservation> GetByUser(int userId);
        IEnumerable<Reservation> GetByType(string code);
    }
}
