using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationManager.DomainModel.Base;
using ReservationManager.Persistence.Exceptions;
using ReservationManager.Persistence.Interfaces.Base;

namespace ReservationManager.Persistence.Repositories.Base
{
    public class CrudTypeBaseRepository<T> : RepositoryBase<T>, ICrudTypeRepository<T>
        where T : BaseType
    {

        protected readonly ReservationManagerDbContext Context;

        public CrudTypeBaseRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
            Context = dbContext;
        }

        public async Task<T> CreateTypeAsync(T entity, CancellationToken cancellationToken = default)
        {
            return await base.AddAsync(entity, cancellationToken);
        }

        public async Task DeleteTypeAsync(int id, CancellationToken cancellationToken = default)
        {
            var dbEntity = await Context.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);
            if (dbEntity == null)
                throw new EntityNotFoundException($"Cannot delete Type id: {id}");

            dbEntity.IsDeleted = DateTime.UtcNow;
            await base.UpdateAsync(dbEntity, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllTypesAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T> GetTypeById(int id, CancellationToken cancellationToken = default)
        {
            var entity = await Context.Set<T>()
                                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken);

            if (entity == null)
                throw new EntityNotFoundException($"Cannot find Type id: {id}");
            return entity;
        }

        public async Task<T?> UpdateTypeAsync(T entity, CancellationToken cancellationToken = default)
        {
            var getEntity = await base.GetByIdAsync(entity.Id, cancellationToken);
            if (getEntity == null) return null;
            var entry = Context.Entry(getEntity);
            entry.CurrentValues.SetValues(entity);
            await base.UpdateAsync(getEntity, cancellationToken);
            return getEntity;
        }
    }
}
