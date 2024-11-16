using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.Core.Interfaces.Validators;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Validators
{
    public class ClosingTimetableValidator : IClosingCalendarValidator
    {
        private readonly IClosingCalendarRepository _timetableRepository;

        public ClosingTimetableValidator(IClosingCalendarRepository timetableRepository)
        {
            _timetableRepository = timetableRepository;
        }

        public bool IsClosureTimetable(UpsertClosingCalendarDto timetable)
        {
            return timetable is { StartTime: null, EndTime: null,StartDate: not null, EndDate: not null };
        }

        public bool IsNominalTimetable(UpsertClosingCalendarDto timetable)
        {
            return timetable is { StartTime: not null, EndTime: not null, StartDate: null, EndDate: null };
        }

        public bool IsTimeReductionTimetable(UpsertClosingCalendarDto timetable)
        {
            return timetable is { StartTime: not null, EndTime: not null, StartDate: not null, EndDate: not null };
        }
        
        public bool IsLegalDateRange(UpsertClosingCalendarDto entity)
        {
            var start = (DateOnly)entity.StartDate!;
            var end = (DateOnly)entity.EndDate!;

            return start <= end && start >= DateOnly.FromDateTime(DateTime.Now) &&
                   end >= DateOnly.FromDateTime(DateTime.Now);
        }

       


        private bool CheckForUpdate(List<ClosingCalendar> existingIntersection, int? id)
        {
            var oldBuildingTimetable = existingIntersection.FirstOrDefault(x => x.Id == id);
            return oldBuildingTimetable != null 
                ? existingIntersection.Any(x => x.Id != id) 
                : existingIntersection.Any();
        }
    }
}
