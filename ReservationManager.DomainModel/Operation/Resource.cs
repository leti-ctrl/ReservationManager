using ReservationManager.DomainModel.Base;
using ReservationManager.DomainModel.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.DomainModel.Operation
{
    public class Resource : BaseEntity
    {
        public string Description { get; set; }
        public int TypeId { get; set; }
        public ResourceType Type { get; set; }
    }
}
