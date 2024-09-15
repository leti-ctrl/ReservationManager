using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Core.Dtos
{
    public class ResourceDto : UpsertResourceDto
    {
        public int Id { get; set; }
        public IEnumerable<ResourceReservedDto>? ResourceReservedDtos { get; set; }
    }
}
