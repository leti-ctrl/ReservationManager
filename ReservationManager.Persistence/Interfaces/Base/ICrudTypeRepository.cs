using ReservationManager.DomainModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Interfaces.Base
{
    public interface ICrudTypeRepository<T> where T : BaseType
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> CreateAsync(T entity);
        Task<T> UpdateAsync(int id, T entity);
        void DeleteAsync(int id);
    }
}
