using ReservationManager.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReservationManager.Core.Commons;

namespace ReservationManager.Core.Interfaces
{
    public interface IBuildingTimetableValidator
    {
        bool IsClosureTimetable(UpsertEstabilishmentTimetableDto timetable, TimetableTypeDto type);
        bool IsNominalTimetable(UpsertEstabilishmentTimetableDto timetable, TimetableTypeDto type);
        bool IsTimeReductionTimetable(UpsertEstabilishmentTimetableDto timetable, TimetableTypeDto type);
        bool IsLegalDateRange(UpsertEstabilishmentTimetableDto entity);
        Task<bool> IsLegalCloseDates(UpsertEstabilishmentTimetableDto entity);
        Task<bool> IsLegalTimeReduction(UpsertEstabilishmentTimetableDto entity);
    }
}
