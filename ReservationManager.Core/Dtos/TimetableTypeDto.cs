using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Dtos
{
    public class TimetableTypeDto
    {
        public int Id { get; set; }
        public required string Code { get; set; }
    }
}
