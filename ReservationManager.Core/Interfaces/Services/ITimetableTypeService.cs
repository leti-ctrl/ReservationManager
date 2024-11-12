using ReservationManager.Core.Dtos;


namespace ReservationManager.Core.Interfaces.Services
{
    public interface ITimetableTypeService
    {
        Task<IEnumerable<TimetableTypeDto>> GetAllTypes();
        Task<TimetableTypeDto> GetById(int id);
    }
}
