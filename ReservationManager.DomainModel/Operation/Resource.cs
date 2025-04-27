using ReservationManager.DomainModel.Base;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.DomainModel.Operation
{
    public class Resource : BaseEntity
    {
        public string Description { get; set; }
        public int TypeId { get; set; }
        public virtual ResourceType Type { get; set; }
        
        public virtual List<Reservation> Reservations { get; set; } = new();
        public virtual List<ClosingCalendar> ClosingCalendars { get; set; } = new();
    }
}
