using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationManager.Core.Commons;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;

namespace ReservationManager.Core.Validators
{
    public class BuildingTimetableValidator : IBuildingTimetableValidator
    {
        private readonly IBuildingTimetableRepository _timetableRepository;

        public BuildingTimetableValidator(IBuildingTimetableRepository timetableRepository)
        {
            _timetableRepository = timetableRepository;
        }

        public bool IsClosureTimetable(UpsertEstabilishmentTimetableDto timetable, TimetableTypeDto type)
        {
            if (timetable.StartTime == null && timetable.EndTime == null
                && timetable.StartDate != null && timetable.EndDate != null
                && type.Code == FixedTimetableType.Closure)
                return true;
            return false;
        }

        public bool IsNominalTimetable(UpsertEstabilishmentTimetableDto timetable, TimetableTypeDto type)
        {
            if (timetable.StartTime != null && timetable.EndTime != null
                && timetable.StartDate == null && timetable.EndDate == null
                && type.Code == FixedTimetableType.Nominal)
                return true;
            return false;
        }

        public bool IsTimeReductionTimetable(UpsertEstabilishmentTimetableDto timetable, TimetableTypeDto type)
        {
            if (timetable.StartTime != null && timetable.EndTime != null
                && timetable.StartDate != null && timetable.EndDate != null
                && type.Code == FixedTimetableType.Overtime)
                return true;
            return false;
        }
        
        public bool IsLegalDateRange(UpsertEstabilishmentTimetableDto entity)
        {
            var start = (DateOnly)entity.StartDate!;
            var end = (DateOnly)entity.EndDate!;

            if (start > end)
                return false;

            if (start < DateOnly.FromDateTime(DateTime.Now) || end < DateOnly.FromDateTime(DateTime.Now))
                return false;

            return true;
        }

        public async Task<bool> IsLegalCloseDates(UpsertEstabilishmentTimetableDto entity)
        {
            var start = (DateOnly)entity.StartDate!;
            var end = (DateOnly)entity.EndDate!;
            var existingIntersection = 
                await _timetableRepository.GetClosingDateIntersection(start, end, entity.TypeId)
                ?? Enumerable.Empty<BuildingTimetable>();
            return existingIntersection.Any();
        }

        public async Task<bool> IsLegalTimeReduction(UpsertEstabilishmentTimetableDto entity)
        {
            var startDate = (DateOnly)entity.StartDate!;
            var endDate = (DateOnly)entity.EndDate!;
            var startTime = (TimeOnly)entity.StartTime!;
            var endTime = (TimeOnly)entity.EndTime!;
            var existingIntersection = 
                await _timetableRepository.GetTimeReductionIntersection(startDate, endDate, startTime, endTime, entity.TypeId)
                ?? Enumerable.Empty<BuildingTimetable>();
            return existingIntersection.Any();
        }
    }
}
