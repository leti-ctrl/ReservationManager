using Mapster;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Exceptions;
using ReservationManager.Core.Interfaces;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Builders
{
    public class OvertimeTimetableStrategy : IBuildingTimetableStrategy
    {
        private readonly IBuildingTimetableValidator _timetableValidator;

        public OvertimeTimetableStrategy(IBuildingTimetableValidator timetableValidator)
        {
            _timetableValidator = timetableValidator;
        }

        public bool IsMatch(UpsertEstabilishmentTimetableDto entity, TimetableTypeDto type)
        {
            return _timetableValidator.IsTimeReductionTimetable(entity, type);
        }

        public async Task<BuildingTimetable> Create(UpsertEstabilishmentTimetableDto entity)
        {
            if (!_timetableValidator.IsLegalDateRange(entity))
                throw new CreateBuildingTimetableException(
                    "Date range could not be in the past or start date connot be earlier than end date.");
            if (await _timetableValidator.IsLegalTimeReduction(entity, true, null))
                throw new CreateBuildingTimetableException("New time reduction intersect with existing ones.");

            return entity.Adapt<BuildingTimetable>();
        }

        public async Task<BuildingTimetable> Update(int id, UpsertEstabilishmentTimetableDto entity)
        {
            if (!_timetableValidator.IsLegalDateRange(entity))
                throw new CreateBuildingTimetableException(
                    "Date range could not be in the past or start date connot be earlier than end date.");
            if (await _timetableValidator.IsLegalTimeReduction(entity, false, id))
                throw new CreateBuildingTimetableException("New time reduction intersect with existing ones.");

            return entity.Adapt<BuildingTimetable>();
        }
    }
}
