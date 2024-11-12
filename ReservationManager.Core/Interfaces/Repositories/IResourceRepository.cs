using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IResourceRepository : ICrudBaseEntityRepository<Resource>
    {
        Task<IEnumerable<Resource>> GetByTypeAsync(string code);
        Task<IEnumerable<Resource>> GetFiltered(int? typeId, int? resourceId);
    }
}
