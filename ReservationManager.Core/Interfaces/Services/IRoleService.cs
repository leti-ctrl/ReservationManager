using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRoles();
    }
}
