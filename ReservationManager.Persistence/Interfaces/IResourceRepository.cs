using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces.Base;

namespace ReservationManager.Persistence.Interfaces
{
    public interface IResourceRepository : ICrudBaseEntityRepository<Resource>
    {
        Task<IEnumerable<Resource>> GetByTypeAsync(string code);
    }
}
