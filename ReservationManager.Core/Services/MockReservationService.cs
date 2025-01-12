using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.Core.Services;

public class MockReservationService : IMockReservationService
{
    private readonly IMockReservationRepository _reservationRepository;

    public MockReservationService(IMockReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public IEnumerable<string> GetAllReservations()
    {
        return _reservationRepository.GetReservations();
    }
}