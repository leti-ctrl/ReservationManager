using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUserInfo();
        Task<UserDto> CreateUser(UpsertUserDto userDto);
        Task<UserDto?> UpdateUserRoles(int userId, Role[] newRoles);
        Task DeleteUser(int id);
    }
}
