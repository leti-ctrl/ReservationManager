using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Validators
{
    public interface IBuildingTimetableValidator
    {
        bool IsClosureTimetable(UpsertEstabilishmentTimetableDto timetable, TimetableTypeDto type);
        bool IsNominalTimetable(UpsertEstabilishmentTimetableDto timetable, TimetableTypeDto type);
        bool IsTimeReductionTimetable(UpsertEstabilishmentTimetableDto timetable, TimetableTypeDto type);
        bool IsLegalDateRange(UpsertEstabilishmentTimetableDto entity);
        Task<bool> IsLegalCloseDates(UpsertEstabilishmentTimetableDto entity, bool isCreate, int? id);
        Task<bool> IsLegalTimeReduction(UpsertEstabilishmentTimetableDto entity, bool isCreate, int? id);
    }
}
