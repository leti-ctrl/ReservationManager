using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Dtos
{
    public class ResourceReservedDto
    {
        public DateOnly Day { get; set; }
        public IEnumerable<TimeOnly> reservedTimes { get; set; }
    }
}
