using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IUserRepository : ICrudBaseEntityRepository<User>
    {
        Task<User> AddUserAsync(User user);
        Task<User> UpdateUserRolesAsync(User user, Role[] roles);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
