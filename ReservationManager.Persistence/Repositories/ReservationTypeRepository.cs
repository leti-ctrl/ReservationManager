using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class ReservationTypeRepository : CrudEditableTypeRepository<ReservationType>, IReservationTypeRepository
    {
        public ReservationTypeRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
