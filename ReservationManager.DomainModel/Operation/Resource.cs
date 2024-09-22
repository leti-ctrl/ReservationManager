using ReservationManager.DomainModel.Base;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.DomainModel.Operation
{
    public class Resource : BaseEntity
    {
        public string Description { get; set; }
        public int TypeId { get; set; }
        public ResourceType Type { get; set; }
    }
}
