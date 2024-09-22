using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class ResourceTypeRepository : CrudTypeBaseRepository<ResourceType>, IResourceTypeRepository
    {
        public ResourceTypeRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
