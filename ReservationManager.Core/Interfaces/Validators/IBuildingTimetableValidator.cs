using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Validators
{
    public interface IBuildingTimetableValidator
    {
        bool IsClosureTimetable(UpsertEstabilishmentTimetableDto timetable);
        bool IsNominalTimetable(UpsertEstabilishmentTimetableDto timetable);
        bool IsTimeReductionTimetable(UpsertEstabilishmentTimetableDto timetable);
        bool IsLegalDateRange(UpsertEstabilishmentTimetableDto entity);
        Task<bool> IsLegalCloseDates(UpsertEstabilishmentTimetableDto entity, bool isCreate, int? id);
        Task<bool> IsLegalTimeReduction(UpsertEstabilishmentTimetableDto entity, bool isCreate, int? id);
    }
}
