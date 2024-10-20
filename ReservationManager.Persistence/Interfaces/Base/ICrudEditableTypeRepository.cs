using ReservationManager.DomainModel.Base;

namespace ReservationManager.Persistence.Interfaces.Base
{
    public interface ICrudEditableTypeRepository<T> : ICrudBaseTypeRepository<T> where T : EditableType 
    {
        
        Task<T> CreateTypeAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> UpdateTypeAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteTypeAsync(T entity, CancellationToken cancellationToken = default);
    }
}
