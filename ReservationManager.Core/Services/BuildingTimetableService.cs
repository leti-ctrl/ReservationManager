using Mapster;
using ReservationManager.Core.Builders;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Services;

namespace ReservationManager.Core.Services
{
    public class BuildingTimetableService : IBuildingTimetableService
    {
        private readonly IBuildingTimetableRepository _buildingTimetableRepository;
        private readonly IBuildingTimetableStrategyHandler _timetableHandler;

        public BuildingTimetableService(IBuildingTimetableRepository buildingTimetableRepository,
                                        IBuildingTimetableStrategyHandler timetable)
        {
            _buildingTimetableRepository = buildingTimetableRepository;
            _timetableHandler = timetable;
        }

        public async Task<IEnumerable<BuildingTimetableDto>> GetByDateRange(DateOnly startDate, DateOnly endDate)
        {
            var list = await _buildingTimetableRepository.GetByDateRange(startDate, endDate);
            
            return list.Select(t => t.Adapt<BuildingTimetableDto>());
        }

        public async Task<BuildingTimetableDto> Create(UpsertEstabilishmentTimetableDto entity)
        {
            var model = await _timetableHandler.CreateTimetable(entity);

            var created = await _buildingTimetableRepository.CreateEntityAsync(model);
            
            return created.Adapt<BuildingTimetableDto>();
        }

        public async Task<BuildingTimetableDto> Update(int id, UpsertEstabilishmentTimetableDto entity)
        {
            var timetableList = await _buildingTimetableRepository.GetEntityByIdAsync(id) ??
                                throw new EntityNotFoundException($"Timetable {id} does not exist.");
            

            
            var model = await _timetableHandler.UpdateTimetable(entity, id);
            model.Id = id;
            
            var updated = await _buildingTimetableRepository.UpdateEntityAsync(model);
            
            return updated.Adapt<BuildingTimetableDto>();
        }

        public async Task Delete(int id)
        {
            var entity = await _buildingTimetableRepository.GetEntityByIdAsync(id) ??
                         throw new EntityNotFoundException($"Timetable {id} not found.");
            
            await _buildingTimetableRepository.DeleteEntityAsync(entity);
        }

        public async Task<IEnumerable<BuildingTimetableDto>> GetAll()
        {
            var timetableList = (await _buildingTimetableRepository.GetAllTimetableFromToday()).ToList();
            
            return !timetableList.Any() 
                ? Enumerable.Empty<BuildingTimetableDto>() 
                : timetableList.Select(x => x.Adapt<BuildingTimetableDto>());
        }

        public async Task<IEnumerable<BuildingTimetableDto>> GetByTypeId(int typeId)
        {
            var timetableList = await _buildingTimetableRepository.GetByTypeId(typeId);
            
            return timetableList.Select(item => item.Adapt<BuildingTimetableDto>());
        }
    }
}
