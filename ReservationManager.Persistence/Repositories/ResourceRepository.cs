using Microsoft.EntityFrameworkCore;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class ResourceRepository : CrudEntityBaseRepository<Resource>, IResourceRepository
    {
        public ResourceRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Resource>> GetByTypeAsync(string code)
        {
            return await Context.Set<Resource>()
                                .Where(x => x.Type.Code == code)
                                .ToListAsync();
        }
    }
}
