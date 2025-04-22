using ReservationManager.Core.Interfaces.Repositories.Base;
using ReservationManager.DomainModel.Meta;
using ReservationManager.DomainModel.Operation;

namespace ReservationManager.Core.Interfaces.Repositories
{
    public interface IUserRepository : ICrudBaseEntityRepository<User>
    {
        /// <summary>
        /// UpdateUserRolesAsync: Not cached for teaching purposes
        /// </summary>
        /// <remarks>Not cached</remarks>
        Task<User> UpdateUserRolesAsync(User user, Role[] roles);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> GetUserByIdWithRoleAsync(int userId);
    }
}
