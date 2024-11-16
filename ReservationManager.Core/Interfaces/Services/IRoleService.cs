using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllUserTypes();
        Task<RoleDto> CreateUserType(string code, string name);
        Task<RoleDto> UpdateUserType(int id, string code, string name);
        Task DeleteUserType(int id);
    }
}
