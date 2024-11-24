using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IReservationTypeRepository : ICrudEditableTypeRepository<ReservationType>
    {
        Task<ReservationType?> GetByCodeAsync(string code);
    }
}
