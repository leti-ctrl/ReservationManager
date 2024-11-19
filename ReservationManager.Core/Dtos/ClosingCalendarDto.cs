using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Dtos
{
    public class ClosingCalendarDto
    {
        public int Id { get; set; }
        public DateOnly Day { get; set; }
        public int ResourceId { get; set; }
        public ResourceDto Resource { get; set; }
        public string? Description { get; set; }
    }
}
