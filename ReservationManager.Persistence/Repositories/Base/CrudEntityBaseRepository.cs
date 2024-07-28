using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationManager.DomainModel.Base;
using ReservationManager.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Repositories.Base
{
    public class CrudEntityBaseRepository<T> : RepositoryBase<T>, ICrudEntityRepository<T>
        where T : BaseEntity
    {
        public CrudEntityBaseRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<T> CreateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public async void DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<T> UpdateAsync(int id, T entity)
        {
            throw new NotImplementedException();
        }
    }
}
