using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class RoleRepository : CrudEditableTypeRepository<Role>, IRoleRepository
    {
        public RoleRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
