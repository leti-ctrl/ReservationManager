using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.DapperRepository;

public class DapperReservationRepository : IMockReservationRepository
{
    public IEnumerable<string> GetReservations()
    {
        return new List<string> { "Dapper Reservation A", "Dapper Reservation B" };
    }
}