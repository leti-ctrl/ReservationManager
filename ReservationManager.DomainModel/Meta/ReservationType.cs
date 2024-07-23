using ReservationManager.DomainModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.DomainModel.Meta
{
    public class ReservationType : BaseType
    {
        public TimeOnly Start {  get; set; }
        public TimeOnly End { get; set; }
    }
}
