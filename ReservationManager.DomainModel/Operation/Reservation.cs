using ReservationManager.DomainModel.Base;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.DomainModel.Operation
{
    public class Reservation : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateOnly Day { get; set; }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ResourceId { get; set; }
        public Resource Resource { get; set; }
        public int TypeId { get; set; }
        public ReservationType Type { get; set; }
    }
}
