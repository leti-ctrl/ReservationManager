using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces
{
    public interface IBuildingTimetableService
    {
        Task<IEnumerable<BuildingTimetableDto>> GetAll();
        Task<IEnumerable<BuildingTimetableDto>> GetByTypeId(int typeId);
        Task<BuildingTimetableDto> Create(UpsertEstabilishmentTimetableDto entity);
        Task<BuildingTimetableDto> Update(int id, UpsertEstabilishmentTimetableDto entity);
        Task Delete(int id);
    }
}
