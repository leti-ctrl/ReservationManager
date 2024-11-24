using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDto>> GetUserReservation(int userId);
        Task<ReservationDto?> GetById(int id, int userId);
        Task<ReservationDto> CreateReservation(int userId, UpsertReservationDto reservation);
        Task<ReservationDto?> UpdateReservation(int reservationId, int userId, UpsertReservationDto reservation);
        Task DeleteReservation(int id, int userId);

    }
}
