using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class ResourceTypeRepository : CrudEditableTypeRepository<ResourceType>, IResourceTypeRepository
    {
        public ResourceTypeRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
