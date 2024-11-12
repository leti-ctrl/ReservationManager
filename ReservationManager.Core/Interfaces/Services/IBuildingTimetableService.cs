using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IBuildingTimetableService
    {
        Task<IEnumerable<BuildingTimetableDto>> GetAll();
        Task<IEnumerable<BuildingTimetableDto>> GetByTypeId(int typeId);
        Task<IEnumerable<BuildingTimetableDto>> GetByDateRange(DateOnly startDate, DateOnly endDate);
        Task<BuildingTimetableDto> Create(UpsertEstabilishmentTimetableDto entity);
        Task<BuildingTimetableDto> Update(int id, UpsertEstabilishmentTimetableDto entity);
        Task Delete(int id);
    }
}
