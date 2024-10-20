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
    public class ClosedTimetableBuilderStrategy : IEstabilishmentTimetableBuilderStrategy
    {
        private readonly IEstabilishmentTimetableValidator _timetableValidator;

        public ClosedTimetableBuilderStrategy(IEstabilishmentTimetableValidator validator)
        {
            _timetableValidator = validator;
        }

        public bool IsMatch(UpsertEstabilishmentTimetableDto entity, TimetableTypeDto type)
        {
            return _timetableValidator.IsClosureTimetable(entity, type);
        }

        public Task<EstabilishmentTimetable> Build(UpsertEstabilishmentTimetableDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
