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
    public class ClosedTimetableStrategy : IBuildingTimetableStrategy
    {
        private readonly IBuildingTimetableValidator _timetableValidator;

        public ClosedTimetableStrategy(IBuildingTimetableValidator validator)
        {
            _timetableValidator = validator;
;
        }

        public bool IsMatch(UpsertEstabilishmentTimetableDto entity, TimetableTypeDto type)
        {
            return _timetableValidator.IsClosureTimetable(entity, type);
        }

        public async Task<BuildingTimetable> Build(UpsertEstabilishmentTimetableDto entity)
        {
            if (!_timetableValidator.IsLegalDateRange(entity))
                throw new CreateBuildingTimetableException(
                    "Date range could not be in the past or start date connot be earlier than end date.");
            if (_timetableValidator.IsLegalCloseDates(entity).Result)
                throw new CreateBuildingTimetableException("New closing dates intersect with existing ones.");

            return entity.Adapt<BuildingTimetable>();
        }

        
    }
}
