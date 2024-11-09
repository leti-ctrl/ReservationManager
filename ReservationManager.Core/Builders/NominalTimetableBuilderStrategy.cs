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
    public class NominalTimetableBuilderStrategy : IEstabilishmentTimetableBuilderStrategy
    {
        private readonly IEstabilishmentTimetableValidator _timetableValidator;
        private readonly IEstabilishmentTimetableRepository _timetableRepository;

        public NominalTimetableBuilderStrategy(IEstabilishmentTimetableValidator timetableValidator, 
                                               IEstabilishmentTimetableRepository timetableRepository)
        {
            _timetableValidator = timetableValidator;
            _timetableRepository = timetableRepository;
        }

        public bool IsMatch(UpsertEstabilishmentTimetableDto entity, TimetableTypeDto type)
        {
            return _timetableValidator.IsNominalTimetable(entity, type);
        }

        public async Task<EstabilishmentTimetable> Build(UpsertEstabilishmentTimetableDto entity)
        {
            var existing = await _timetableRepository.GetByTypeId(entity.TypeId);
            if (existing == null || !existing.Any())
                return entity.Adapt<EstabilishmentTimetable>();

            throw new TimetableExistsException("NominalTimetable already exists.");
        }

    }
}
