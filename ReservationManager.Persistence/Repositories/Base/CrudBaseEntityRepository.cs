﻿using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Base;

namespace ReservationManager.Persistence.Repositories.Base
{
    public class CrudBaseEntityRepository<T>(ReservationManagerDbContext dbContext)
        : RepositoryBase<T>(dbContext), ICrudBaseEntityRepository<T>
        where T : BaseEntity
    {
        protected readonly ReservationManagerDbContext Context = dbContext;

        public virtual async Task<IEnumerable<T>> GetAllEntitiesAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetEntityByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await base.GetByIdAsync(id, cancellationToken);
        }

        public async Task<T> CreateEntityAsync(T entity, CancellationToken cancellationToken = default)
        {
            return await base.AddAsync(entity, cancellationToken);
        }

        public async Task DeleteEntityAsync(T dbEntity, CancellationToken cancellationToken = default)
        {
            dbEntity.IsDeleted = DateTime.UtcNow;
            await base.UpdateAsync(dbEntity, cancellationToken);
        }

        public async Task<T?> UpdateEntityAsync(T entity, CancellationToken cancellationToken = default)
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
