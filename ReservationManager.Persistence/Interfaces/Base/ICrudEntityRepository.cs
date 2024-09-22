using ReservationManager.DomainModel.Base;

namespace ReservationManager.Persistence.Interfaces.Base
{
    public interface ICrudEntityRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAllEntitiesAsync(CancellationToken cancellationToken = default);
        Task<T?> GetEntityByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<T> CreateEntityAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> UpdateEntityAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteEntityAsync(int id, CancellationToken cancellationToken = default);
    }
}
