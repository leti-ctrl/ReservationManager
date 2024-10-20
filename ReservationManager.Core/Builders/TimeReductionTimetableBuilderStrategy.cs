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
    public class TimeReductionTimetableBuilderStrategy : IEstabilishmentTimetableBuilderStrategy
    {
        private readonly IEstabilishmentTimetableValidator _timetableValidator;

        public TimeReductionTimetableBuilderStrategy(IEstabilishmentTimetableValidator timetableValidator)
        {
            _timetableValidator = timetableValidator;
        }

        public bool IsMatch(UpsertEstabilishmentTimetableDto entity, TimetableTypeDto type)
        {
            return _timetableValidator.IsTimeReductionTimetable(entity, type);
        }

        public Task<EstabilishmentTimetable> Build(UpsertEstabilishmentTimetableDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
