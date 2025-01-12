namespace ReservationManager.Core.Interfaces.Services;

public interface IMockReservationService
{
    IEnumerable<string> GetAllReservations();
}