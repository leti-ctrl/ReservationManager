using ReservationManager.DomainModel.Base;

namespace ReservationManager.DomainModel.Meta
{
    public class ReservationType : BaseType
    {
        public TimeOnly Start { get; set; }
        public TimeOnly End { get; set; }
    }
}
