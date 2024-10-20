using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationManager.DomainModel.Base;
using ReservationManager.Persistence.Exceptions;
using ReservationManager.Persistence.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Repositories.Base
{
    public class CrudBaseTypeRepository<T> : RepositoryBase<T>, ICrudBaseTypeRepository<T>
        where T : BaseType
    {
        protected readonly ReservationManagerDbContext Context;

        public CrudBaseTypeRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
            Context = dbContext;
        }


        public async Task<IEnumerable<T>> GetAllTypesAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetTypeById(int id, CancellationToken cancellationToken = default)
        {
            var entity = await Context.Set<T>()
                                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

            return entity;
        }

        public async Task<T?> GetTypeByCode(string code, CancellationToken cancellationToken = default)
        {
            var entity = await Context.Set<T>()
                                .FirstOrDefaultAsync(x => x.Code == code, cancellationToken: cancellationToken);

            return entity;
        }

        public virtual Task<T> CreateTypeAsync(T entity, CancellationToken cancellationToken = default)
        {
            throw new NotEditableTypeException("Create not permitted");
        }

        public virtual Task<T?> UpdateTypeAsync(T entity, CancellationToken cancellationToken = default)
        {
            throw new NotEditableTypeException("Update not permitted");
        }

        public virtual Task DeleteTypeAsync(T entity, CancellationToken cancellationToken = default)
        {
            throw new NotEditableTypeException("Delete not permitted");
        }
    }
}
