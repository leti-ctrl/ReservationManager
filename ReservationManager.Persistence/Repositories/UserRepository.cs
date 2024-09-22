using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces;
using ReservationManager.Persistence.Repositories.Base;

namespace ReservationManager.Persistence.Repositories
{
    public class UserRepository : CrudEntityBaseRepository<User>, IUserRepository
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

        public Task<IEnumerable<User>> GetByTypeAsync(string code)
        {
            throw new NotImplementedException();
        }
    }
}
