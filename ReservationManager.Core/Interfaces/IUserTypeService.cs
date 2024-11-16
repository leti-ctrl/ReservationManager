using ReservationManager.Core.Dtos;

namespace ReservationManager.Core.Interfaces
{
    public interface IUserTypeService
    {
        Task<IEnumerable<UserTypeDto>> GetAllUserTypes();
        Task<UserTypeDto> CreateUserType(string code);
        Task<UserTypeDto> UpdateUserType(int id, string userTypeDto);
        Task DeleteUserType(int id);
    }
}
