using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IUserRepository : ICrudBaseEntityRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByNameAsync(string name, string surname);
    }
}
