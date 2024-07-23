using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Dtos
{
    public class ReservationDto: UpsertReservationDto
    {
        public int Id { get; set; }
    }
}
