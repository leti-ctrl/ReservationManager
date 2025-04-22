using Microsoft.EntityFrameworkCore;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Meta;
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
