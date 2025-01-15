using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.EFRepository;

public class ReservationRepository : IMockReservationRepository
{
    public IEnumerable<string> GetReservations()
    {
        return new List<string> { "EF Reservation 1", "EF Reservation 2" };
    }

}