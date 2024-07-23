using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Dtos
{
    public class FilterDto
    {
        public IEnumerable<int> Ids { get; set; }
        public IEnumerable<string> Types { get; set; }
        public IEnumerable<DateOnly> Days { get; set; }
        public IEnumerable<TimeOnly> Times { get; set; }
    }
}
