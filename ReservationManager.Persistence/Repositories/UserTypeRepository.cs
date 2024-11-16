using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class UserTypeRepository : CrudEditableTypeRepository<Role>, IUserTypeRepository
    {
        public UserTypeRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
