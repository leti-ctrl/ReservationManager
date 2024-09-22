using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces
{
    public interface IReservationService
    {
        IEnumerable<ReservationDto> GetUserReservation(int userId);
        ReservationDto GetById(int id);
        ReservationDto CreateReservation(UpsertReservationDto reservation);
        ReservationDto UpdateReservation(UpsertReservationDto reservation);
        void DeleteReservation(int id);

    }
}
