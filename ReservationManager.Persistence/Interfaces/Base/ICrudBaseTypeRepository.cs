using ReservationManager.DomainModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Interfaces.Base
{
    public interface ICrudBaseTypeRepository<T> where T :BaseType
    {
        Task<IEnumerable<T>> GetAllTypesAsync(CancellationToken cancellationToken = default);
        Task<T?> GetTypeById(int id, CancellationToken cancellationToken = default);
        Task<T?> GetTypeByCode(string code, CancellationToken cancellationToken = default);
        Task<T> CreateTypeAsync(T entity, CancellationToken cancellationToken = default);
        Task<T?> UpdateTypeAsync(T entity, CancellationToken cancellationToken = default);
        Task DeleteTypeAsync(T entity, CancellationToken cancellationToken = default);
    }
}
