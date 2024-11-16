using ReservationManager.DomainModel.Meta;
using ReservationManager.Persistence.Repositories.Base;
using ReservationManager.Core.Interfaces.Repositories;

namespace ReservationManager.Persistence.Repositories
{
    public class TimetableTypeRepository : CrudBaseTypeRepository<TimetableType>, ITimetableTypeRepository
    {
        public TimetableTypeRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }
    }
}
