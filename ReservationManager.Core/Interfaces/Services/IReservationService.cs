using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDto>> GetUserReservation(SessionInfo session);
        Task<ReservationDto?> GetById(int id, SessionInfo session);
        Task<ReservationDto> CreateReservation(SessionInfo session, UpsertReservationDto reservation);
        Task<ReservationDto?> UpdateReservation(int reservationId, SessionInfo session,
            UpsertReservationDto reservation);
        Task DeleteReservation(int id, SessionInfo session);

    }
}
