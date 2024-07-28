using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManager.Persistence.Repositories
{
    public class ResourceRepository : CrudEntityBaseRepository<Resource>, IResourceRepository
    {
        public ResourceRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<IEnumerable<Resource>> GetByTypeAsync(string code)
        {
            throw new NotImplementedException();
        }
    }
}
