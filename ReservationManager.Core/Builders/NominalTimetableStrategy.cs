using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Builders
{
    public class NominalTimetableStrategy : IBuildingTimetableStrategy
    {
        private readonly IBuildingTimetableValidator _timetableValidator;
        private readonly IBuildingTimetableRepository _timetableRepository;

        public NominalTimetableStrategy(IBuildingTimetableValidator timetableValidator, 
                                               IBuildingTimetableRepository timetableRepository)
        {
            _timetableValidator = timetableValidator;
            _timetableRepository = timetableRepository;
        }

        public bool IsMatch(UpsertEstabilishmentTimetableDto entity, TimetableTypeDto type)
        {
            return _timetableValidator.IsNominalTimetable(entity, type);
        }

        public async Task<BuildingTimetable> Create(UpsertEstabilishmentTimetableDto entity)
        {
            var existing = await _timetableRepository.GetByTypeId(entity.TypeId);
            if (!existing.Any())
                return entity.Adapt<BuildingTimetable>();

            throw new TimetableExistsException("NominalTimetable already exists.");
        }

        public Task<BuildingTimetable> Update(int id, UpsertEstabilishmentTimetableDto entity)
        {
            return Task.FromResult(entity.Adapt<BuildingTimetable>());
        }
    }
}
