using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Validators
{
    public interface IClosingCalendarValidator
    {
        bool IsClosureTimetable(UpsertClosingCalendarDto timetable);
        bool IsNominalTimetable(UpsertClosingCalendarDto timetable);
        bool IsTimeReductionTimetable(UpsertClosingCalendarDto timetable);
        bool IsLegalDateRange(UpsertClosingCalendarDto entity);
    }
}
