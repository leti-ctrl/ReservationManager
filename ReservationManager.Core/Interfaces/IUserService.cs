using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<UserDto> GetUser(int id);
        Task<UserDto> CreateUser(UpsertUserDto userDto);
        Task<UserDto> UpdateUser(int id, UpsertUserDto userDto);
        Task DeleteUser(int id);
    }
}
