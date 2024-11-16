using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Validators
{
    public interface IBuildingTimetableValidator
    {
        bool IsClosureTimetable(UpsertClosingCalendarDto timetable);
        bool IsNominalTimetable(UpsertClosingCalendarDto timetable);
        bool IsTimeReductionTimetable(UpsertClosingCalendarDto timetable);
        bool IsLegalDateRange(UpsertClosingCalendarDto entity);
        Task<bool> IsLegalCloseDates(UpsertClosingCalendarDto entity, bool isCreate, int? id);
        Task<bool> IsLegalTimeReduction(UpsertClosingCalendarDto entity, bool isCreate, int? id);
    }
}
