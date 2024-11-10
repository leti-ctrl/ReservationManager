using Mapster;
using ReservationManager.Core.Builders;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Services
{
    public class BuildingTimetableService : IBuildingTimetableService
    {
        private readonly IBuildingTimetableRepository _buildingTimetableRepository;
        private readonly ITimetableTypeService _timetableTypeService;
        private readonly IBuildingTimetableStrategyHandler _timetableHandler;

        public BuildingTimetableService(IBuildingTimetableRepository buildingTimetableRepository,
                                        ITimetableTypeService timetableTypeService,
                                        IBuildingTimetableStrategyHandler timetable)
        {
            _buildingTimetableRepository = buildingTimetableRepository;
            _timetableTypeService = timetableTypeService;
            _timetableHandler = timetable;
        }

        public async Task<BuildingTimetableDto> Create(UpsertEstabilishmentTimetableDto entity)
        {
            var type = await _timetableTypeService.GetById(entity.TypeId);
            var model = await _timetableHandler.CreateTimetable(entity, type);

            var created = await _buildingTimetableRepository.CreateEntityAsync(model);
            
            return created.Adapt<BuildingTimetableDto>();
        }

        public async Task<BuildingTimetableDto> Update(int id, UpsertEstabilishmentTimetableDto entity)
        {
            var timetableList = await _buildingTimetableRepository.GetEntityByIdAsync(id) ??
                                throw new EntityNotFoundException($"Timetable {id} does not exist.");
            
            var type = await _timetableTypeService.GetById(entity.TypeId);

            if (timetableList.TypeId != type.Id)
                throw new UpdateNotPermittedException("Timetable type does not match.");

            var model = await _timetableHandler.UpdateTimetable(entity, type, id);
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
            var timetableList = (await _buildingTimetableRepository.GetAllEntitiesAsync()).ToList();
            
            return !timetableList.Any() 
                ? Enumerable.Empty<BuildingTimetableDto>() 
                : timetableList.Select(x => x.Adapt<BuildingTimetableDto>());
        }

        public async Task<IEnumerable<BuildingTimetableDto>> GetByTypeId(int typeId)
        {
            throw new NotImplementedException();
        }
    }
}
