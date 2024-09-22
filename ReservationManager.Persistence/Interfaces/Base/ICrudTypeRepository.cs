using ReservationManager.DomainModel.Base;

namespace ReservationManager.Persistence.Interfaces.Base
{
    public interface ICrudTypeRepository<T> where T : BaseType
    {
        Task<T> GetTypeById(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<T>> GetAllTypesAsync(CancellationToken cancellationToken = default);
        Task<T> CreateTypeAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> UpdateTypeAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteTypeAsync(int id, CancellationToken cancellationToken = default);
    }
}
