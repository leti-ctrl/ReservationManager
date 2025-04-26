using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Base;

namespace ReservationManager.Persistence.Repositories.Base
{
    public class CrudEditableTypeRepository<T> : CrudBaseTypeRepository<T>, ICrudEditableTypeRepository<T>
        where T : EditableType
    {


        public CrudEditableTypeRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }


        public override async Task<T> CreateTypeAsync(T typeToCreate, CancellationToken cancellationToken = default)
        {
            return await base.AddAsync(typeToCreate, cancellationToken);
        }


        public override async Task<T?> UpdateTypeAsync(T typeToUpdate, CancellationToken cancellationToken = default)
        {
            var getEntity = await base.GetByIdAsync(typeToUpdate.Id, cancellationToken);
            if (getEntity == null)
                return null;

            var entry = Context.Entry(getEntity);
            entry.CurrentValues.SetValues(typeToUpdate);

            await base.UpdateAsync(getEntity, cancellationToken);

            return getEntity;
        }

        public override async Task DeleteTypeAsync(T typeToDelete, CancellationToken cancellationToken = default)
        {
            typeToDelete.IsDeleted = DateTime.UtcNow;
            await base.UpdateAsync(typeToDelete, cancellationToken);
        }


    }
}
