using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Dtos
{
    public class ClosingCalendarFilterDto
    {
        public int? Id { get; set; }
        public DateOnly? StartDay { get; set; }
        public DateOnly? EndDay { get; set; }
        public int? RescourceId { get; set; }
        public int? ResourceTypeId { get; set; }
    }
}
