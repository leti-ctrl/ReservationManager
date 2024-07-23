using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Dtos
{
    public class UpsertResourceDto
    {
        public string Description { get; set; }
        public ResourceTypeDto Type { get; set; }
    }
}
