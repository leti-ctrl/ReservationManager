using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;
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

        public NominalTimetableBuilderStrategy(IEstabilishmentTimetableValidator timetableValidator)
        {
            _timetableValidator = timetableValidator;
        }

        public bool IsMatch(UpsertEstabilishmentTimetableDto entity, TimetableTypeDto type)
        {
            return _timetableValidator.IsNominalTimetable(entity, type);
        }

        public Task<EstabilishmentTimetable> Build(UpsertEstabilishmentTimetableDto entity)
        {
            return Task.FromResult(entity.Adapt<EstabilishmentTimetable>());
        }
    }
}
