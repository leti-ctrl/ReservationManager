using ReservationManager.DomainModel.Operation;
using ReservationManager.Persistence.Interfaces.Base;

namespace ReservationManager.Persistence.Interfaces
{
    public interface IUserRepository : ICrudEntityRepository<User>
    {
        Task<User> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetByTypeAsync(string code);
        Task<IEnumerable<User>> GetByNameAsync(string name, string surname);
    }
}
