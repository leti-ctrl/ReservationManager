using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Validators;
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
    public class ClosedTimetableBuilderStrategy : IEstabilishmentTimetableBuilderStrategy
    {
        private readonly IEstabilishmentTimetableValidator _timetableValidator;

        public ClosedTimetableBuilderStrategy(IEstabilishmentTimetableValidator validator)
        {
            _timetableValidator = validator;
;
        }

        public bool IsMatch(UpsertEstabilishmentTimetableDto entity, TimetableTypeDto type)
        {
            return _timetableValidator.IsClosureTimetable(entity, type);
        }

        public async Task<EstabilishmentTimetable> Build(UpsertEstabilishmentTimetableDto entity)
        {
            if (!_timetableValidator.IsLegalDateRange(entity))
                throw new CreateEstabilishmentTimetableException(
                    "Date range could not be in the past or start date connot be earlier than end date.");
            if (_timetableValidator.IsLegalCloseDates(entity).Result)
                throw new CreateEstabilishmentTimetableException("New closing dates intersect with existing ones.");

            return entity.Adapt<EstabilishmentTimetable>();
        }

        
    }
}
