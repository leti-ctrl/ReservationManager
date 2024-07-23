using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.DomainModel.Base
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime? IsDeleted { get; set; }
    }
}
