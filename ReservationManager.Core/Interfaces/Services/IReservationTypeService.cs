using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IReservationTypeService
    {
        Task<IEnumerable<ReservationTypeDto>> GetAllReservationType();
        Task<ReservationTypeDto> CreateReservationType(UpsertReservationTypeDto request);
        Task<ReservationTypeDto?> UpdateReservationType(int id, UpsertReservationTypeDto model);
        Task DeleteReservationType(int id);
    }
}
