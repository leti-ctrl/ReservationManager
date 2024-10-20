using Mapster;
using ReservationManager.Core.Builders;
using ReservationManager.Core.Consts;
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
    public class EstabilishementTimetableService : IEstabilishmentTimetableService
    {
        private readonly IEstabilishmentTimetableRepository _estabilishmentTimetableRepository;
        private readonly ITimetableTypeService _timetableTypeService;
        private readonly IEstabilishmentTimetableBuilderStrategyHandler _timetableBuilderHandler;
        private readonly IEstabilishmentTimetableValidator _timetableValidator;

        public EstabilishementTimetableService(IEstabilishmentTimetableRepository estabilishmentTimetableRepository,
                                               ITimetableTypeService timetableTypeService,
                                               IEstabilishmentTimetableValidator timetableValidator,
                                               IEstabilishmentTimetableBuilderStrategyHandler timetableBuilder)
        {
            _estabilishmentTimetableRepository = estabilishmentTimetableRepository;
            _timetableTypeService = timetableTypeService;
            _timetableValidator = timetableValidator;
            _timetableBuilderHandler = timetableBuilder;
        }

        public async Task<EstabilishmentTimetableDto> Create(UpsertEstabilishmentTimetableDto entity)
        {
            var type = await _timetableTypeService.GetById(entity.TypeId) ??
                throw new EntityNotFoundException($"TimetableType {entity.TypeId} not found.");


            // Usa il gestore delle strategie per costruire il modello
            var model = await _timetableBuilderHandler.BuildTimetable(entity, type);


            var created = await _estabilishmentTimetableRepository.CreateEntityAsync(model);
            return created.Adapt<EstabilishmentTimetableDto>();
        }

        public async Task<IEnumerable<EstabilishmentTimetableDto>> GetAll()
        {
            var timetableList = await _estabilishmentTimetableRepository.GetAllEntitiesAsync();
            if(timetableList == null) 
                return Enumerable.Empty<EstabilishmentTimetableDto>();

            return timetableList.Select(x => x.Adapt<EstabilishmentTimetableDto>());
            
        }

        public async Task<IEnumerable<EstabilishmentTimetableDto>> GetByTypeId(int typeId)
        {
            throw new NotImplementedException();
        }
    }
}
