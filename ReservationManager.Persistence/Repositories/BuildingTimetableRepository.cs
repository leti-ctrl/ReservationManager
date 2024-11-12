using Microsoft.EntityFrameworkCore;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class BuildingTimetableRepository : CrudBaseEntityRepository<BuildingTimetable>,
        IBuildingTimetableRepository
    {
        public BuildingTimetableRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<BuildingTimetable>> GetByDateRange(DateOnly startDate, DateOnly endDate)
        {
            return await Context.Set<BuildingTimetable>()
                                .Where(x => x.StartDate >= startDate && x.EndDate <= endDate)
                                .ToListAsync();
        }

        public async Task<IEnumerable<BuildingTimetable>> GetClosingDateIntersection(DateOnly start, DateOnly end, int typeId)
        {
            return await Context.Set<BuildingTimetable>()
                                .Where(x => x.TypeId == typeId && !x.IsDeleted.HasValue)
                                .Where(x => x.StartDate <= end && x.EndDate >= start)
                                .ToListAsync();
        }

        public async Task<IEnumerable<BuildingTimetable>> GetAllTimetableFromToday()
        {
            return await Context.Set<BuildingTimetable>()
                                .Include(x => x.Type )
                                .Where(x => x.StartDate == null || 
                                                          x.StartDate >= DateOnly.FromDateTime(DateTime.Now))
                                .ToListAsync();
        }

        public async Task<IEnumerable<BuildingTimetable>> GetByTypeId(int typeId)
        {
            return await Context.Set<BuildingTimetable>()
                                .Where(x => x.TypeId == typeId && !x.IsDeleted.HasValue)
                                .ToListAsync();    
        }
        
        public async Task<IEnumerable<BuildingTimetable>> GetTimeReductionIntersection(DateOnly startDate, 
            DateOnly endDate, TimeOnly startTime, TimeOnly endTime, int typeId) 
        {
            return await Context.Set<BuildingTimetable>()
                                .Where(x => x.TypeId == typeId && !x.IsDeleted.HasValue)
                                .Where(x => x.StartDate <= endDate && x.EndDate >= startDate)
                                .Where(x => x.StartTime >= endTime && x.EndTime <= startTime)
                                .ToListAsync();
        }
    }
}
