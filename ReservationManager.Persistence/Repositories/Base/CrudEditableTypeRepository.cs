using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReservationManager.DomainModel.Base;
using ReservationManager.Persistence.Interfaces.Base;

namespace ReservationManager.Persistence.Repositories.Base
{
    public class CrudEditableTypeRepository<T> : CrudBaseTypeRepository<T>, ICrudEditableTypeRepository<T>
        where T : EditableType
    {


        public CrudEditableTypeRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }


        public override async Task<T> CreateTypeAsync(T entity, CancellationToken cancellationToken = default)
        {
            return await base.AddAsync(entity, cancellationToken);
        }


        public override async Task<T?> UpdateTypeAsync(T entity, CancellationToken cancellationToken = default)
        {
            var getEntity = await base.GetByIdAsync(entity.Id, cancellationToken);
            if (getEntity == null)
                return null;

            var entry = Context.Entry(getEntity);
            entry.CurrentValues.SetValues(entity);

            await base.UpdateAsync(getEntity, cancellationToken);

            return getEntity;
        }

        public override async Task DeleteTypeAsync(T dbEntity, CancellationToken cancellationToken = default)
        {
            dbEntity.IsDeleted = DateTime.UtcNow;
            await base.UpdateAsync(dbEntity, cancellationToken);
        }


    }
}
