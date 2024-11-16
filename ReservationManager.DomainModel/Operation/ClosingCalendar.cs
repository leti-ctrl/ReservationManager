using ReservationManager.DomainModel.Base;
using ReservationManager.DomainModel.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.DomainModel.Operation
{
    public class ClosingCalendar : BaseEntity
    {
        public DateOnly Day { get; set; }
        public int ResourceId { get; set; }
        public Resource Resource { get; set; }
        public string? Description { get; set; }
    }
}
