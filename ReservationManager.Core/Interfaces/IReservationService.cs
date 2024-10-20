using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces
{
    public interface IReservationService
    {
        Task<IEnumerable<ReservationDto>> GetUserReservation(int userId);
        Task<ReservationDto> GetById(int id);
        Task<ReservationDto> CreateReservation(UpsertReservationDto reservation);
        Task<ReservationDto> UpdateReservation(UpsertReservationDto reservation);
        Task DeleteReservation(int id);

    }
}
