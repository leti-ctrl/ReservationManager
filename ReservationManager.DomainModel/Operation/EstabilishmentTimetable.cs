using ReservationManager.DomainModel.Base;
using ReservationManager.DomainModel.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.DomainModel.Operation
{
    public class EstabilishmentTimetable : BaseEntity
    {
        public int TypeId { get; set; }
        public required TimetableType Type { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public string? Description { get; set; }
    }
}
