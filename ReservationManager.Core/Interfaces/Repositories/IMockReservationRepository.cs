namespace ReservationManager.Core.Interfaces.Repositories;

public interface IMockReservationRepository
{
    public IEnumerable<string> GetReservations();
}