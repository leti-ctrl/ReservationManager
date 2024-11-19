using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllRoles();
        Task<RoleDto> CreateRole(string code, string name);
        Task<RoleDto> UpdateRole(int id, string code, string name);
        Task DeleteRole(int id);
    }
}
