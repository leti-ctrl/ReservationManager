using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Interfaces.Base;

namespace ReservationManager.Persistence.Interfaces
{
    public interface IReservationTypeRepository : ICrudEditableTypeRepository<ReservationType>
    {
    }
}
