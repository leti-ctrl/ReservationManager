using ReservationManager.Core.Consts;
using ReservationManager.Core.Dtos;
using ReservationManager.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Validators
{
    public class EstabilishmentTimetableValidator : IEstabilishmentTimetableValidator
    {
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
                && type.Code == FixedTimetableType.TimeReduction)
                return true;
            return false;
        }
    }
}
