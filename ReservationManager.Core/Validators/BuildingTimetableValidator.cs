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
            return timetable is { StartTime: null, EndTime: null,StartDate: not null, EndDate: not null }
                   && type.Code == FixedTimetableType.Closure;
        }

        public bool IsNominalTimetable(UpsertEstabilishmentTimetableDto timetable, TimetableTypeDto type)
        {
            return timetable is { StartTime: not null, EndTime: not null, StartDate: null, EndDate: null }
                   && type.Code == FixedTimetableType.Nominal;
        }

        public bool IsTimeReductionTimetable(UpsertEstabilishmentTimetableDto timetable, TimetableTypeDto type)
        {
            return timetable is { StartTime: not null, EndTime: not null, StartDate: not null, EndDate: not null }
                   && type.Code == FixedTimetableType.Overtime;
        }
        
        public bool IsLegalDateRange(UpsertEstabilishmentTimetableDto entity)
        {
            var start = (DateOnly)entity.StartDate!;
            var end = (DateOnly)entity.EndDate!;

            return start <= end && start >= DateOnly.FromDateTime(DateTime.Now) &&
                   end >= DateOnly.FromDateTime(DateTime.Now);
        }

        public async Task<bool> IsLegalCloseDates(UpsertEstabilishmentTimetableDto entity, bool isCreate, int? id)
        {
            var start = (DateOnly)entity.StartDate!;
            var end = (DateOnly)entity.EndDate!;
            var existingIntersection = 
                await _timetableRepository.GetClosingDateIntersection(start, end, entity.TypeId);
            return isCreate 
                ? existingIntersection.Any()
                : CheckForUpdate(existingIntersection.ToList(), id);
        }

        public async Task<bool> IsLegalTimeReduction(UpsertEstabilishmentTimetableDto entity, bool isCreate, int? id)
        {
            var startDate = (DateOnly)entity.StartDate!;
            var endDate = (DateOnly)entity.EndDate!;
            var startTime = (TimeOnly)entity.StartTime!;
            var endTime = (TimeOnly)entity.EndTime!;

            var existingIntersection = await _timetableRepository.
                GetTimeReductionIntersection(startDate, endDate, startTime, endTime, entity.TypeId);
            return isCreate && id != null
                ? existingIntersection.Any() 
                : CheckForUpdate(existingIntersection.ToList(), id);
        }


        private bool CheckForUpdate(List<BuildingTimetable> existingIntersection, int? id)
        {
            var oldBuildingTimetable = existingIntersection.FirstOrDefault(x => x.Id == id);
            return oldBuildingTimetable != null 
                ? existingIntersection.Any(x => x.Id != id) 
                : existingIntersection.Any();
        }
    }
}
