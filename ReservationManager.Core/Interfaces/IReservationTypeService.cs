using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces
{
    public interface IReservationTypeService
    {
        Task<IEnumerable<ReservationTypeDto>> GetAllReservationType();
        Task<ReservationTypeDto> CreateReservationType(string code, string start, string end);
        Task<ReservationTypeDto> UpdateReservationType(int id, string code, string start, string end);
        Task DeleteReservationType(int id);
    }
}
