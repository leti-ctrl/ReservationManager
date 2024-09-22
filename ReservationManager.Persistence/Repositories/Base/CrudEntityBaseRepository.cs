﻿using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationManager.DomainModel.Base;
using ReservationManager.Persistence.Exceptions;
using ReservationManager.Persistence.Interfaces.Base;

namespace ReservationManager.Persistence.Repositories.Base
{
    public class CrudEntityBaseRepository<T> : RepositoryBase<T>, ICrudEntityRepository<T>
        where T : BaseEntity
    {

        protected readonly ReservationManagerDbContext Context;

        public CrudEntityBaseRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
            Context = dbContext;
        }

        public async Task<T> CreateEntityAsync(T entity, CancellationToken cancellationToken = default)
        {
            return await base.AddAsync(entity, cancellationToken);
        }

        public async Task DeleteEntityAsync(int id, CancellationToken cancellationToken = default)
        {
            var dbEntity = await Context.Set<T>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken: cancellationToken) 
                ?? throw new EntityNotFoundException($"Cannot delete Entity id: {id}");

            dbEntity.IsDeleted = DateTime.UtcNow;
            await base.UpdateAsync(dbEntity, cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllEntitiesAsync(CancellationToken cancellationToken = default)
        {
            return await Context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<T?> GetEntityByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await base.GetByIdAsync(id, cancellationToken);
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
