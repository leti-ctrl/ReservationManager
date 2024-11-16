using Microsoft.EntityFrameworkCore;
using ReservationManager.Core.Interfaces.Repositories;
using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class UserRepository : CrudBaseEntityRepository<User>, IUserRepository
    {
        public UserRepository(ReservationManagerDbContext dbContext) : base(dbContext)
        {
        }

        public Task<User> GetByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetByNameAsync(string name, string surname)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<User>> GetByTypeAsync(string code)
        {
            return await Context.Set<User>()
                                .Where(x => x.Type.Code == code)
                                .ToListAsync();
        }
    }
}
