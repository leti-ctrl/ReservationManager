using Microsoft.EntityFrameworkCore;
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
    public class EstabilishmentTimetableRepository : CrudBaseEntityRepository<EstabilishmentTimetable>, IEstabilishmentTimetableRepository
    {
        public EstabilishmentTimetableRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<EstabilishmentTimetable>> GetByTypeId(int typeId)
        {
            return await base.Context.Set<EstabilishmentTimetable>()
                                     .Where(x => x.TypeId == typeId)
                                     .ToListAsync();    
        }
    }
}
