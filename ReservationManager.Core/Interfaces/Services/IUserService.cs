using ReservationManager.Core.Dtos;
using ReservationManager.DomainModel.Meta;

namespace ReservationManager.Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto?> GetUserById(int id);
        Task<UserDto?> GetUserByEmail(string email);
        Task<UserDto> CreateUser(UpsertUserDto userDto);
        Task<UserDto?> UpdateUser(int id, UpsertUserDto userDto);
        Task<UserDto?> UpdateUserRoles(int userId, Role[] newRoles);
        Task DeleteUser(int id);
    }
}
