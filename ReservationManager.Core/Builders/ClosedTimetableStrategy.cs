using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Operation;

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

        public async Task<BuildingTimetable> Create(UpsertEstabilishmentTimetableDto entity)
        {
            if (!_timetableValidator.IsLegalDateRange(entity))
                throw new CreateBuildingTimetableException(
                    "Date range could not be in the past or start date connot be earlier than end date.");
            if (await _timetableValidator.IsLegalCloseDates(entity, true, null))
                throw new CreateBuildingTimetableException("New closing dates intersect with existing ones.");

            return entity.Adapt<BuildingTimetable>();
        }

        public async Task<BuildingTimetable> Update(int id, UpsertEstabilishmentTimetableDto entity)
        {
            if (!_timetableValidator.IsLegalDateRange(entity))
                throw new CreateBuildingTimetableException(
                    "Date range could not be in the past or start date connot be earlier than end date.");
            if (await _timetableValidator.IsLegalCloseDates(entity, false, id))
                throw new CreateBuildingTimetableException("New closing dates intersect with existing ones.");
            
            
            return entity.Adapt<BuildingTimetable>();
        }
    }
}
