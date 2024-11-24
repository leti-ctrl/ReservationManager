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

        public async Task<ReservationType?> GetByCodeAsync(string code)
        {
            return await Context.Set<ReservationType>()
                                .Where(x => x.Code == code)
                                .SingleOrDefaultAsync();
        }
    }
}
