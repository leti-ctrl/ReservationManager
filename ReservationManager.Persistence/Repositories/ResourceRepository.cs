using Microsoft.EntityFrameworkCore;
using ReservationManager.DomainModel;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class ResourceRepository : CrudBaseEntityRepository<Resource>, IResourceRepository
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

        public async Task<IEnumerable<Resource>> GetFiltered(int? typeId, int? resourceId)
        {
            var query = Context.Set<Resource>().AsQueryable();

            if (typeId.HasValue)
                query = query.Where(x => x.Type.Id == typeId);

            if (resourceId.HasValue)
                query = query.Where(x => x.Id == resourceId);

            return await query.ToListAsync();
        }
    }
}