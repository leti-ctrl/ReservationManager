using ReservationManager.DomainModel.Base;
using ReservationManager.DomainModel.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.DomainModel.Operation
{
    public class Reservation : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly Day {  get; set; }
        public TimeOnly Start {  get; set; }
        public TimeOnly End { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ResourceId { get; set; }
        public Resource Resource { get; set; }
        public int TypeId { get; set; }
        public ReservationType Type { get; set; }
    }
}
