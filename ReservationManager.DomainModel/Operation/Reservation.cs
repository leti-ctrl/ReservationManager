using ReservationManager.DomainModel.Base;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.DomainModel.Operation
{
    public class Reservation : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly Day { get; set; }
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ResourceId { get; set; }
        public virtual Resource Resource { get; set; }
        public int TypeId { get; set; }
        public virtual ReservationType Type { get; set; }
    }
}
